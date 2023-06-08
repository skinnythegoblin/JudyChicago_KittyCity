using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FloorMappingController : MonoBehaviour
{
	[SerializeField] private bool _active = true;
	[SerializeField] private float _tickRate = 2.0f;
	
	[Header("Markers")]
	[SerializeField] private bool _markersEnabled = true;
	[SerializeField] private GameObject _floorMarker;
	
	[Header("Mesh")]
	[SerializeField] private bool _meshEnabled = true;
	[SerializeField] private MeshFilter _meshFilter;
	[SerializeField] private MeshRenderer _meshRenderer;
	
	private float _tickTimer;
	private Vector3[] _floorPoints;
	
	private List<GameObject> _floorMarkers = new List<GameObject>();
	
	private Mesh _mesh;
	
	private void OnEnable()
	{
		_mesh = new Mesh { name = "Custom Floor Mesh" };
	}
	
	private void Start()
	{
		_floorMarker.SetActive(false);
	}
	
	private void Update()
	{
		if (!_active) return;
		if (_tickTimer < 0)
		{
			_tickTimer = _tickRate;
			UpdateFloorMapping();
		}
		else
		{
			_tickTimer -= Time.deltaTime;
		}
	}
	
	private void UpdateFloorMapping()
	{
		// These points are in LOCAL / TRACKING space and are placed as if they were in WORLD space. This means they do not 
		// appear in the correct position and I am not sure how to transform them into the correct world positions. -Brandon
		// https://developer.oculus.com/reference/unity/v53/class_o_v_r_boundary/#a53a9973b2e69ea3b289daa633c71d4e9
		_floorPoints = OVRManager.boundary.GetGeometry(OVRBoundary.BoundaryType.PlayArea);

		// ALSO! BoundaryType.OuterBoundary is the actual function that returns ALL points along the border, but it has been
		// deprecated and no longer works on Oculus. This means we can only use PlayArea, which is a 4 point rectangle of
		// the playable area, so only expect _floorPoints to be an array of size 4.
		
		if (_markersEnabled) UpdateMarkers();
		if (_meshEnabled) UpdateMesh();
	}
	
	private void UpdateMarkers()
	{
		int i = 0;
		foreach (Vector3 point in _floorPoints)
		{
			if (i >= _floorMarkers.Count) CreateNewFloorMarker();
			
			var marker = _floorMarkers[i++];
			marker.transform.position = point;
			marker.SetActive(true);
		}
		for (; i < _floorMarkers.Count; i++)
		{
			_floorMarkers[i].SetActive(false);
		}
	}
	
	private void CreateNewFloorMarker()
	{
		_floorMarkers.Add(Instantiate(_floorMarker, _floorMarker.transform.parent));
	}
	
	private void UpdateMesh()
	{
		// Not currently working, focused on getting the points to be positioned correctly first. But in theory this would generate
		// a floor mesh so you could see where the floor actually is

		// 0 2 1 , 1 2 3
		_mesh.vertices = new Vector3[] { _floorPoints[0], _floorPoints[2], _floorPoints[1], _floorPoints[1], _floorPoints[2], _floorPoints[3], };
		_mesh.normals = new Vector3[] { Vector3.up, Vector3.up, Vector3.up, Vector3.up, Vector3.up, Vector3.up };
		_meshFilter.mesh = _mesh;
	}
}
