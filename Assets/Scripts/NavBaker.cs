using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavBaker : MonoBehaviour
{
    // NOTE: The final version of the program will use the official layout of the gallery, and will not bake a NavMesh at runtime.
    //       This script is only for testing purposes, and should not be relied on by other scripts.
    //         - CatController.TryMove() relies on this script directly
    //         - CatController.SetNewDestination() & CatController.SetNewDestination(Vector3) rely on this CatController.TryMove()
    
    // Changes to the points of the play area often occur within the first few seconds of playtime, but changes are minor after that
    // These settings make sure that the program is not devoting resources to rebaking the NavMesh when there aren't any major changes
    
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
