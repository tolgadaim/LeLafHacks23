using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapController : MonoBehaviour
{
    public MapPinLayer _mapPinLayer;
    public GameObject _mapPinPrefab;
    public Vector3[] _mapPinLocations;

    // Start is called before the first frame update
    void Start()
    {
        foreach (var mapPinLocation in _mapPinLocations)
        {
            Vector3 lehighUniversityLocation = new Vector3(-75.3787f, 40.6076f, 0f);

            var mapPin = Instantiate(_mapPinPrefab);
            mapPin.GetComponent<MapPin>().Location = lehighUniversityLocation;
            _mapPinLayer.MapPins.Add(mapPin.GetComponent<MapPin>());
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
