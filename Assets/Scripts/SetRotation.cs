using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetRotation : MonoBehaviour
{
    [SerializeField] bool _setX = false;
    [SerializeField] bool _setY = false;
    [SerializeField] bool _setZ = false;

    public void SetTo(float value)
    {
        Vector3 _newRotation = Vector3.zero;
        if (_setX)
        {
            _newRotation.x = value;
        }
        if (_setY)
        {
            _newRotation.y = value;
        }
        if (_setZ)
        {
            _newRotation.z = value;
        }

        transform.rotation = Quaternion.Euler(_newRotation);
    }
}
