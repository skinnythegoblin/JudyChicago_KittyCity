using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Broadcaster : MonoBehaviour
{
    public static event Action<string> Broadcast;

    public void SendBroadcast(string broadcastMessage)
    {
        Broadcast?.Invoke(broadcastMessage);
    }
}
