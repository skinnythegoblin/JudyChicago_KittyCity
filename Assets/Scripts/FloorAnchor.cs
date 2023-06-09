using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorAnchor : MonoBehaviour
{
    private void OnEnable()
    {
        FloorMappingController.OnFloorUpdated += SnapToFloor;
    }

    private void OnDisable()
    {
        FloorMappingController.OnFloorUpdated -= SnapToFloor;
    }
    
    private void SnapToFloor()
    {
        Vector3 snappedPosition = transform.position;
        snappedPosition.y = FloorMappingController.FloorLevel;
        transform.position = snappedPosition;
    }
}
