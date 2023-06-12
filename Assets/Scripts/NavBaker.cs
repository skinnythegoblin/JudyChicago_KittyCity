using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavBaker : MonoBehaviour
{
    [SerializeField] private NavMeshSurface[] _surfaces;
    [Tooltip("If enabled, a NavMesh will only be baked the first time the floor mesh is updated.")]
    [SerializeField] private bool _bakeOnce = false;
    [Tooltip("After this many floor mesh changes, stop checking for changes to the floor mesh. A value of 0 will always check for changes. This setting will not be used if Bake Once is enabled.")]
    [SerializeField] private int _stopBakingAfterUpdates = 3;
    [Tooltip("After this many ticks, check for changes to the floor mesh. This setting will not be used if Bake Once is enabled")]
    [SerializeField] private int _bakeAfterTicks = 5;

    public static bool HasBaked { get; private set; } = false;

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
        if (_ticks <= 0 || (_stopBakingAfterUpdates > 0 &&_updates < _stopBakingAfterUpdates))
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
        if (_bakeOnce) FloorMappingController.OnFloorUpdated -= Tick;
    }
}
