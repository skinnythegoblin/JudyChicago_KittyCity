using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(OVRHand))]
public class GestureTracker : MonoBehaviour
{
    private OVRHand _hand;
    private bool _isPinching = false;

    public static event Action<Vector3> OnPinchPerformed;
    
    void Start()
    {
        _hand = GetComponent<OVRHand>();
    }

    // I couldn't find way to get these gestures as events from the OVR plugin, so I'm polling for input, but if there's a better way, feel free to replace this
    void Update()
    {
        if (_hand.GetFingerIsPinching(OVRHand.HandFinger.Thumb))
        {
            if (!_isPinching)
            {
                _isPinching = true;
                OnPinchPerformed?.Invoke(transform.position);
            }
        }
        else if (_isPinching) _isPinching = false;
    }
}
