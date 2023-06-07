using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ToggleSound : MonoBehaviour
{
    public void Toggle()
    {
        AudioSource speaker = GetComponent<AudioSource>();
        if (speaker.isPlaying)
        {
            speaker.Pause();
        }
        else
        {
            speaker.UnPause();
        }
    }
}
