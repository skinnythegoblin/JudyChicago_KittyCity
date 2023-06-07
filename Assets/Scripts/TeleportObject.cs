using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportObject : MonoBehaviour
{
    [SerializeField] GameObject _objectToTeleport;
    [SerializeField] bool _useThisTransformAsDestination = true;
    [SerializeField] Transform _destination;

    void Start()
    {
        if (_useThisTransformAsDestination)
            {
                _destination = transform;
            }
    }
    
    public void Teleport()
    {
        if (_objectToTeleport != null)
        {
            _objectToTeleport.transform.position = _destination.position;
        }
    }
}
