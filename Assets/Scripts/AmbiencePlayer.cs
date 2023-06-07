using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AmbiencePlayer : MonoBehaviour
{
    [SerializeField] List<AudioClip> _sfx = new List<AudioClip>();
    [SerializeField] float _minVolume = 0.6f;
    [SerializeField] float _maxVolume = 1f;
    [SerializeField] float _minDelay = 0.5f;
    [SerializeField] float _maxDelay = 4f;
    [Tooltip("How often to try to play a sound effect. Should be a multiple of the physics step.")]
    [SerializeField] float _checkInterval = 0.5f;

    AudioSource _player;
    float _timer = 0f;

    void Awake()
    {
        _player = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        _timer += Time.fixedDeltaTime;
        _timer = Mathf.Round(_timer * 100) / 100;

        if (_timer % _checkInterval == 0 && _timer > _minDelay)
        {
            if (Random.Range(0f, 1f) <= _timer / _maxDelay)
            {
                _player.PlayOneShot(_sfx[Random.Range(0, _sfx.Count)], Random.Range(_minVolume, _maxVolume));
                _timer = 0;
            }
        }
    }
}
