using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class FloorMappingController : MonoBehaviour
{
	[SerializeField] private bool _active = true;
	[SerializeField] private float _tickRate = 0.5f;
	
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
		_meshFilter.mesh = _mesh;
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
		_floorPoints = OVRManager.boundary.GetGeometry(OVRBoundary.BoundaryType.PlayArea);
		
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
		_mesh.vertices = _floorPoints;
	}
}
