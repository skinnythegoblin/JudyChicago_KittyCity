using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleTransform : MonoBehaviour
{
    [SerializeField] bool _togglePosition = false;
    [SerializeField] Vector3 _position0 = Vector3.zero;
    [SerializeField] Vector3 _position1 = Vector3.zero;

    [SerializeField] bool _toggleRotation = false;
    [SerializeField] Vector3 _rotation0 = Vector3.zero;
    [SerializeField] Vector3 _rotation1 = Vector3.zero;

    public void ToggleAll()
    {
        TogglePosition();
        ToggleRotation();
    }

    public void TogglePosition()
    {
        if (_togglePosition)
        {
            if (transform.localPosition == _position0)
            {
                transform.localPosition = _position1;
            }
            else if (transform.localPosition == _position1)
            {
                transform.localPosition = _position0;
            }
        }
    }

    public void ToggleRotation()
    {
        if (_toggleRotation)
        {
            if (transform.localRotation == Quaternion.Euler(_rotation0))
            {
                transform.localRotation = Quaternion.Euler(_rotation1);
            }
            else if (transform.localRotation == Quaternion.Euler(_rotation1))
            {
                transform.localRotation = Quaternion.Euler(_rotation0);
            }
        }
    }
}
