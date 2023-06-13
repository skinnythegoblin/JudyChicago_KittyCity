using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavBaker : MonoBehaviour
{
    [SerializeField] private NavMeshSurface[] _surfaces;
    [Tooltip("After this many floor mesh changes, stop checking for changes to the floor mesh. A value of 0 will always check for changes.")]
    [SerializeField] private int _stopBakingAfterUpdates = 3;
    [Tooltip("After this many ticks, check for changes to the floor mesh.")]
    [SerializeField] private int _bakeAfterTicks = 5;

    public static bool HasBaked { get; private set; } = false;
    public static event Action OnNavigationBaked;

    private int _ticks = 0;
    private int _updates = 0;
    
    void OnEnable()
    {
        FloorMappingController.OnFloorUpdated += Tick;
    }

    void OnDisable()
    {
        FloorMappingController.OnFloorUpdated -= Tick;
    }

    void Tick()
    {
        _ticks--;
        if (_ticks <= 0 && (_stopBakingAfterUpdates <= 0 || _updates < _stopBakingAfterUpdates))
        {
            BakeAllNavMeshSurfaces();
            _ticks = _bakeAfterTicks;
        }
    }
    
    void BakeAllNavMeshSurfaces()
    {
        foreach (NavMeshSurface surface in _surfaces) surface.BuildNavMesh();

        HasBaked = true;
        _updates++;

        OnNavigationBaked?.Invoke();
    }
}
