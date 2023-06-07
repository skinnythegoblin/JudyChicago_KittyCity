using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EventListener : MonoBehaviour
{
    [SerializeField] string _listenForMessage;
    [SerializeField] UnityEvent OnBroadcast;
    
    void OnEnable()
    {
        Broadcaster.Broadcast += Listen;
    }

    void OnDisable()
    {
        Broadcaster.Broadcast -= Listen;
    }

    void Listen(string message)
    {
        if (message == _listenForMessage)
        {
            OnBroadcast?.Invoke();
        }
    }
}
