using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    [SerializeField] float _rotationSpeed = 30f;
    [SerializeField] Vector3 _rotationAxis = Vector3.up;
    [SerializeField] bool _startRotating = true;

    bool _isRotating = false;

    void Start()
    {
        _isRotating = _startRotating;
    }

    void Update()
    {
        if (_isRotating)
        {
            transform.rotation *= Quaternion.Euler(_rotationAxis.normalized * _rotationSpeed * Time.deltaTime);
        }
    }

    public void ToggleRotation()
    {
        _isRotating = !_isRotating;
    }
}
