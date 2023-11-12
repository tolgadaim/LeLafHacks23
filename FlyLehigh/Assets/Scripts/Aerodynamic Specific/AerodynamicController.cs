using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class AerodynamicController : Aerodynamics
{
    [Serializable]
    public class ControlSurface
    {
        public Transform transform;
        public enum SurfaceType
        {
            LeftAileron,
            RightAileron,
            Elevator,
            Stabilizer,
            Flaps,
            Brake
        }
        public SurfaceType surfaceType;
        
        public float Factor;
    }

    [Header("Faux Stall Attributes")]
    [SerializeField]
    private float _stallSpeed = 60f;
    [SerializeField]
    private float _stallFactor = 2f;

    [Header("Throttle Control")]
    [SerializeField]
    private float _afterburnerIncreaseFactor = 0.2f;
    public float EnginePercentage;

    [SerializeField]
    private List<ControlSurface> _controlSurfaces = new List<ControlSurface>();
    public List<ControlSurface> ControlSurfaces => _controlSurfaces;

    protected override void Start()
    {
        base.Start();
    }

    protected override void ApplyThrust()
    {
        if (_currentEngineSpeed >= EnginePercentage - _engineIncreaseSpeed/2 && _currentEngineSpeed <= EnginePercentage + _engineIncreaseSpeed/2)
        {
            if (_currentEngineSpeed <= 100)
                _currentEngineSpeed = EnginePercentage;
            else if (_currentEngineSpeed >= EnginePercentage - (_engineIncreaseSpeed / 2 * _afterburnerIncreaseFactor) &&
                     _currentEngineSpeed <= EnginePercentage + (_engineIncreaseSpeed / 2 * _afterburnerIncreaseFactor))
                _currentEngineSpeed = EnginePercentage;
        }
        else if (_currentEngineSpeed > EnginePercentage)
            _currentEngineSpeed -= (_currentEngineSpeed <= 100) ? _engineIncreaseSpeed : _engineIncreaseSpeed * _afterburnerIncreaseFactor;
        else
            _currentEngineSpeed += (_currentEngineSpeed <= 100) ? _engineIncreaseSpeed : _engineIncreaseSpeed * _afterburnerIncreaseFactor;

        float thrustAmount = CalculateEnginePropulsion(_currentEngineSpeed) * _thrustFactor;
        _rb.AddRelativeForce(Vector3.forward * thrustAmount);
    }

    float CalculateEnginePropulsion(float throttlePercentage)
    {
        if (throttlePercentage <= 20)
            return 0f;
        if (throttlePercentage <= 30)
            return 54 * (throttlePercentage - 20);
        if (throttlePercentage <= 50)
            return 540f;
        if (throttlePercentage <= 60)
            return 540f + 6 * (throttlePercentage - 50);
        if (throttlePercentage <= 80)
            return 600f + 30 * (throttlePercentage - 60);
        if (throttlePercentage <= 95)
            return 1200f + 15 * (throttlePercentage - 80);
        if (throttlePercentage <= 100)
            return 1425f + 55 * (throttlePercentage - 95);
        return 1700f + 80 * (throttlePercentage - 100);
    }

    protected override void AdditionalDragMethods(float dragAmount)
    {
        ApplySurfaceTorques(dragAmount / _maxFacingArea);
    }

    void ApplySurfaceTorques(float dragAmount)
    {
        foreach (ControlSurface controlSurface in _controlSurfaces)
        {
            if (controlSurface.surfaceType == ControlSurface.SurfaceType.LeftAileron)
            {
                float torqueAmount = -1f * VelAngleDiffMultiplier(controlSurface.transform.up) * dragAmount * controlSurface.Factor;
                _rb.AddRelativeTorque(Vector3.back * torqueAmount);
                float forceAmount = Mathf.Cos(Vector3.Angle(controlSurface.transform.up, transform.forward * -1f) * Mathf.Deg2Rad) * torqueAmount;
                _rb.AddRelativeForce(Vector3.forward * forceAmount / 2);
            }

            else if (controlSurface.surfaceType == ControlSurface.SurfaceType.RightAileron)
            {
                float torqueAmount = -1f * VelAngleDiffMultiplier(controlSurface.transform.up) * dragAmount * controlSurface.Factor;
                _rb.AddRelativeTorque(Vector3.forward * torqueAmount);
                float forceAmount = Mathf.Cos(Vector3.Angle(controlSurface.transform.up, transform.forward * -1f) * Mathf.Deg2Rad) * torqueAmount;
                _rb.AddRelativeForce(Vector3.forward * forceAmount / 2);
            }

            else if (controlSurface.surfaceType == ControlSurface.SurfaceType.Elevator)
            {
                float torqueAmount = -1f * VelAngleDiffMultiplier(controlSurface.transform.up) * dragAmount * controlSurface.Factor;
                _rb.AddRelativeTorque(Vector3.left * torqueAmount);
                float forceAmount = Mathf.Cos(Vector3.Angle(controlSurface.transform.up, transform.forward * -1f) * Mathf.Deg2Rad) * torqueAmount;
                _rb.AddRelativeForce(Vector3.forward * forceAmount / 2);

                // Apply fake lift torque
                float liftTorqueAmount = VelAngleDiffMultiplier(transform.up) * dragAmount * controlSurface.Factor;
                if (GetAngleOfAttack() >= Mathf.Abs(20f) && _rb.velocity.magnitude <= _stallSpeed) // If at stall speed
                {
                    /*
                     * TO-DO
                     * Maybe make stall faster depending on AoA
                     */
                    float multiplier = _stallFactor * Mathf.Cos(.5f * (Vector3.Angle(transform.forward, Vector3.Normalize(_rb.velocity)) - 2.5f) * Mathf.Deg2Rad);
                    liftTorqueAmount = multiplier * dragAmount * controlSurface.Factor * Mathf.Min(100f, Mathf.Pow(_stallSpeed / _rb.velocity.magnitude, 2));
                    liftTorqueAmount *= VelAngleDiffMultiplier(transform.up) <= VelAngleDiffMultiplier(transform.up * -1f) ? 1f: -1f;
                }
                _rb.AddRelativeTorque(Vector3.left * liftTorqueAmount);
                
            }

            else if (controlSurface.surfaceType == ControlSurface.SurfaceType.Stabilizer)
            {
                float torqueAmount = VelAngleDiffMultiplier(controlSurface.transform.right) * dragAmount * controlSurface.Factor;
                _rb.AddRelativeTorque(Vector3.down * torqueAmount);
                float forceAmount = Mathf.Abs(Mathf.Cos(Vector3.Angle(controlSurface.transform.right, transform.forward)) * Mathf.Deg2Rad) * torqueAmount;
                _rb.AddRelativeForce(Vector3.back * forceAmount / 2);
            }

            else if (controlSurface.surfaceType == ControlSurface.SurfaceType.Flaps)
            {
                if (controlSurface.transform.localEulerAngles.x != 0) {
                    float torqueAmount = VelAngleDiffMultiplier(controlSurface.transform.up) * dragAmount * controlSurface.Factor;
                    _rb.AddForce(controlSurface.transform.up * torqueAmount);
                }
            }

            else if (controlSurface.surfaceType == ControlSurface.SurfaceType.Brake)
            {
                float torqueAmount = -1f * VelAngleDiffMultiplier(controlSurface.transform.up) * dragAmount * controlSurface.Factor;
                float forceAmount = Mathf.Cos(Vector3.Angle(controlSurface.transform.up, transform.forward * -1f) * Mathf.Deg2Rad) * torqueAmount;
                if (controlSurface.transform.localEulerAngles.x != 0) _rb.AddRelativeForce(Vector3.forward * forceAmount);
            }
        }
    }
    
}