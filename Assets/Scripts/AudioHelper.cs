using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AudioHelper
{
    public static AudioSource PlayClip2D(AudioClip clip, float volume, bool randomizePitch = false)
    {
        GameObject _audioObject = new GameObject("Audio2D");
        AudioSource _audioSource = _audioObject.AddComponent<AudioSource>();
        if (randomizePitch)
        {
            _audioSource.pitch += Random.Range(-0.25f, 0.25f);
        }

        _audioSource.clip = clip;
        _audioSource.volume = volume;

        _audioSource.Play();
        Object.Destroy(_audioObject, clip.length);

        return _audioSource;
    }

    public static AudioSource PlayClip3D(AudioClip clip, float volume, Vector3 location, bool randomizePitch = false)
    {
        GameObject _audioObject = new GameObject("Audio2D");
        AudioSource _audioSource = _audioObject.AddComponent<AudioSource>();
        if (randomizePitch)
        {
            _audioSource.pitch += Random.Range(-0.25f, 0.25f);
        }

        _audioSource.clip = clip;
        _audioSource.volume = volume;
        _audioSource.transform.position = location;
        _audioSource.spatialize = true;
        _audioSource.spatialBlend = 1.0f;

        _audioSource.Play();
        Object.Destroy(_audioObject, clip.length);

        return _audioSource;
    }
}
