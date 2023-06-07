using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ToggleSocket : MonoBehaviour
{
    public void Toggle()
    {
        GetComponent<XRSocketInteractor>().socketActive = !GetComponent<XRSocketInteractor>().socketActive;
    }
}
