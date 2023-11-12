using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropellerController : MonoBehaviour
{
    // Required links
    [SerializeField]
    private AerodynamicController _AC;
    
    [SerializeField]
    private int _idleRPM = 1;
    [SerializeField]
    private int _maxRPM = 2400;

    void Update()
    {
        float spinSpeed = Mathf.Max(_idleRPM * ((int)_AC.GetComponent<Rigidbody>().velocity.magnitude / 10), 2400 * (_AC.CurrentEngineSpeed / 100));
        transform.Rotate(Vector3.forward * spinSpeed * 360 * Time.deltaTime);
    }
}
