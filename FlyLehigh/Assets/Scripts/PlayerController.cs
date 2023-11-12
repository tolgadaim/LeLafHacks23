using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Required links
    [SerializeField]
    private AerodynamicController _AC;

    // Air control surface max values
    [SerializeField]
    private int _angleRotSpeed = 30;
    [SerializeField]
    private int _maxRudderAngle = 10;
    [SerializeField]
    private int _maxAileronAngle = 30;
    [SerializeField]
    private int _maxElevatorAngle = 30;
    [SerializeField]
    private int _maxThrottle = 100;
    [SerializeField]
    private int _minThrottle = 0;
    
    // Roll controls
    private int _rudderCondition;
    private int _aileronCondition;
    private int _elevatorCondition;
    private int _throttleCondition;

    void OnCollisionEnter(Collision collision)
    {
        FindObjectOfType<DeathMenu>(true).gameObject.SetActive(true);
        FindObjectOfType<TimeManager>().PauseTime();
    }

    void Update()
    {
        // Get input values
        _rudderCondition = BoolToInt(Input.GetKey(KeyCode.E)) + BoolToInt(Input.GetKey(KeyCode.Q), -1);
        _aileronCondition = BoolToInt(Input.GetKey(KeyCode.D)) + BoolToInt(Input.GetKey(KeyCode.A), -1);
        _elevatorCondition = BoolToInt(Input.GetKey(KeyCode.S)) + BoolToInt(Input.GetKey(KeyCode.W), -1);
        _throttleCondition =  BoolToInt(Input.GetKey(KeyCode.LeftShift)) + BoolToInt(Input.GetKey(KeyCode.LeftControl), -1);

        ManageControlSurfaces();
        ManageThrottle();
        
    }

    private int BoolToInt(bool condition, int multiplier=1)
    {
        return multiplier * (condition ? 1 : 0); 
    }

    private void ManageControlSurfaces()
    {
        foreach (AerodynamicController.ControlSurface surface in _AC.ControlSurfaces)
        {
            Vector3 newEulerDegrees = surface.transform.localEulerAngles;
            switch (surface.surfaceType) {
                case AerodynamicController.ControlSurface.SurfaceType.LeftAileron:
                    newEulerDegrees = new Vector3(_aileronCondition * _maxAileronAngle, newEulerDegrees.y, newEulerDegrees.z);
                    break;
                case AerodynamicController.ControlSurface.SurfaceType.RightAileron:
                    newEulerDegrees = new Vector3(-_aileronCondition * _maxAileronAngle, newEulerDegrees.y, newEulerDegrees.z);
                    break;
                case AerodynamicController.ControlSurface.SurfaceType.Elevator:
                    newEulerDegrees = new Vector3(_elevatorCondition * _maxElevatorAngle, newEulerDegrees.y, newEulerDegrees.z);
                    break;
                case AerodynamicController.ControlSurface.SurfaceType.Stabilizer:
                    newEulerDegrees = new Vector3(newEulerDegrees.x, -_rudderCondition * _maxRudderAngle, newEulerDegrees.z);
                    break;
                case AerodynamicController.ControlSurface.SurfaceType.Flaps:
                    // We're not gonna use the flaps for this game
                    break;
                case AerodynamicController.ControlSurface.SurfaceType.Brake:
                    // No need for brakes, do we?
                    break;
            }
            surface.transform.localRotation = Quaternion.RotateTowards(surface.transform.localRotation, Quaternion.Euler(newEulerDegrees), _angleRotSpeed * Time.deltaTime);
        }
    }

    private void ManageThrottle()
    {
        if (_throttleCondition > 0 && _AC.EnginePercentage < _maxThrottle)
        {
            _AC.EnginePercentage++;
        }
        else if (_throttleCondition < 0 && _AC.EnginePercentage > _minThrottle)
        {
            _AC.EnginePercentage--;
        }
    }
}
