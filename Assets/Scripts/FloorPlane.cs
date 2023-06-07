using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorPlane : MonoBehaviour
{
    [SerializeField] GameObject _floorPlane = null;
    [SerializeField] GameObject _floorPointMarker = null;

    Vector3[] _geometry = null;

    void Update()
    {
        PlaceFloorPlane();
        ResizeFloorPlane();
    }
    
    void PlaceFloorPlane()
    {
        if (_geometry == null || _geometry.Length == 0)
        {
            _geometry = OVRManager.boundary.GetGeometry(OVRBoundary.BoundaryType.PlayArea);

            if (_geometry.Length >= 0)
            {
                transform.position = new Vector3(transform.position.x, _geometry[0].y, transform.position.z);
            }
        }
    }

    void ResizeFloorPlane()
    {
        //
    }
}
