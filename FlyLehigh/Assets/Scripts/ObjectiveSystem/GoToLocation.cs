using UnityEngine;
using TMPro;

public class GoToLocation : Objective
{
    [SerializeField]
    private Transform _player;
    [SerializeField]
    private FollowPath _flightPath;
    [SerializeField]
    private Transform _navPoint;
    [SerializeField]
    private float _withinRange = 50f;

    [SerializeField]
    private GameObject _waypointRing;
    [SerializeField]
    private GameObject _arrowPointer;
    [SerializeField]
    private Vector3 _arrowPointerOffset = new Vector3(0f, 4f, 0f);

    private int _currentPoints = 0;
    private int _totalPointsNeeded = 0;

    void OnDrawGizmos()
    {
        if (_navPoint != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, _navPoint.position);
        }
        else if (_flightPath != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, _flightPath.GetFirstPoint().position);
        }
    }

    protected override void Start()
    {
        base.Start();

        _player = FindObjectOfType<AerodynamicController>().transform;
        
        if (_flightPath != null)
        {
            if (_flightPath.Contains(_navPoint) == false)
                _navPoint = _flightPath.GetFirstPoint();
        }

        if (_flightPath != null)
        {
            _totalPointsNeeded = _flightPath.transform.childCount;
        }
        else if (_navPoint != null)
        {
            _totalPointsNeeded = 1;
        }
    }

    void Update()
    {
        if (IsWithinRange())
        {
            _currentPoints++;
        }

        if (_currentPoints >= _totalPointsNeeded)
        {
            FinishObjective();
        }

        if (_waypointRing != null)
        {
            UpdateWaypointRing();
            if (_arrowPointer != null)
            {
                UpdatePointerArrow();
            }
        }
    }

    void UpdateWaypointRing()
    {
        if (_navPoint == null)
        {
            _waypointRing.SetActive(false);
            return;
        }
        _waypointRing.SetActive(true);
        _waypointRing.transform.position = _navPoint.position;
        Vector3 relativePos = _player.position - _waypointRing.transform.position;
        _waypointRing.transform.rotation = Quaternion.LookRotation(relativePos, Vector3.up);
        _waypointRing.GetComponentInChildren<TMP_Text>().text = _navPoint.name;
    }

    void UpdatePointerArrow()
    {
        Vector3 offsetVector = _player.transform.right * _arrowPointerOffset.x + _player.transform.up * _arrowPointerOffset.y + _player.transform.forward * _arrowPointerOffset.z;
        _arrowPointer.transform.position = _player.transform.position + offsetVector;
        _arrowPointer.transform.LookAt(_waypointRing.transform, Vector3.forward);
    }

    public override void FinishObjective()
    {
        base.FinishObjective();
    }

    bool IsWithinRange()
    {
        if (_navPoint != null && Vector3.Distance(_navPoint.position, _player.transform.position) <= _withinRange)
        {
            if (_flightPath != null)
            {
                FindObjectOfType<QuestionManager>().EnableQuestionFor(_navPoint.name);
                _navPoint = _flightPath.GetNextPoint(_navPoint);
            }
            else
            {
                _navPoint = null;
            }
            return true;
        }
        return false;
    }
}
