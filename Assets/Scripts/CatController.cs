using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoffeyUtils;

public enum CatState
{
	Idle,
	Walking,
	Sitting
}

public class CatController : MonoBehaviour
{
	[Header("Settings")]
	[SerializeField] private Vector2 _idleTimeMinMax = new Vector2(2f, 4f);
	[SerializeField] private Vector2 _sittingTimeMinMax = new Vector2(3f, 10f);
	[SerializeField] private float _walkingSpeed = 1f;
	[SerializeField] private float _turningSpeed = 1f;
	[SerializeField] private float _distToReachDestination = 0.5f;
	
	[Header("References")]
	[SerializeField] private Animator _animator;
	
	[Header("Debug")]
	[SerializeField, ReadOnly] private float _actionTimer;
	[SerializeField, ReadOnly] private CatState _state;
	[SerializeField, ReadOnly] private Vector3 _destination;
	[SerializeField] bool _useDestinationDebug = true;
	[SerializeField] GameObject _destinationDebug;
	
	private float IdleTime => Random.Range(_idleTimeMinMax.x, _idleTimeMinMax.y);
	private float SittingTime => Random.Range(_sittingTimeMinMax.x, _sittingTimeMinMax.y);
	
	private void OnValidate()
	{
		if (!_animator)
		{
			_animator = GetComponent<Animator>();
			if (!_animator) _animator = GetComponentInChildren<Animator>();
		}
	}

	private void OnEnable()
	{
		FloorMappingController.OnFloorUpdated += SnapDestinationToFloor;
	}

	private void OnDisable()
	{
		FloorMappingController.OnFloorUpdated -= SnapDestinationToFloor;
	}
	
	private void Update()
	{
		_actionTimer -= Time.deltaTime;
		if (_actionTimer <= 0)
		{
			if (_state == CatState.Walking)
			{
				var dist = Vector3.Distance(transform.position, _destination);
				if (dist < _distToReachDestination)
				{
					// Reached Destination. Set New State
					SetRandomState();
				}
				else
				{
					WalkToDestination();
				}
			}
			else
			{
				SetRandomState();
			}
		}
	}
	
	[Button(Mode = RuntimeMode.OnlyPlaying, Spacing = 24)]
	private void SetRandomState()
	{
		int nextAction = Random.Range(0, 3);
		switch (nextAction)
		{
		case 0:
			SetIdle();
			break;
		case 1:
			SetSitting();
			break;
		case 2:
			SetWalking();
			break;
		}
	}
	
	[Button(Mode = RuntimeMode.OnlyPlaying)]
	private void SetIdle()
	{
		_state = CatState.Idle;
		SetAnimatorBools(false, false);
		_actionTimer = IdleTime;

		if (_useDestinationDebug) _destinationDebug.SetActive(false);
	}
	
	[Button(Mode = RuntimeMode.OnlyPlaying)]
	private void SetSitting()
	{
		_state = CatState.Sitting;
		SetAnimatorBools(true, false);
		_actionTimer = SittingTime;

		if (_useDestinationDebug) _destinationDebug.SetActive(false);
	}
	
	[Button(Mode = RuntimeMode.OnlyPlaying)]
	private void SetWalking()
	{
		CatState prevState = _state;
		_state = CatState.Walking;
		SetAnimatorBools(false, true);
		SetNewDestination();
		_actionTimer = prevState == CatState.Sitting ? 2 : 0;

		if (_useDestinationDebug) _destinationDebug.SetActive(true);
	}
	
	private void SetNewDestination()
	{
		float x = Random.Range(-2f, 2f);
		float z = Random.Range(-2f, 2f);
		_destination = transform.TransformPoint(new Vector3(x, 0, z));
		if (_useDestinationDebug) _destinationDebug.transform.position = _destination + new Vector3(0, 0.05f, 0);
	}

	private void SnapDestinationToFloor()
	{
		_destination.y = FloorMappingController.FloorLevel;
	}
	
	private void WalkToDestination()
	{
		if (_destination.y != FloorMappingController.FloorLevel)
		{
			_destination.y = FloorMappingController.FloorLevel;
			if (_useDestinationDebug) _destinationDebug.transform.position = _destination + new Vector3(0, 0.05f, 0);
		}
		
		Vector3 pos = transform.position + transform.forward * _walkingSpeed * Time.deltaTime;
		
		Quaternion originalRot = transform.rotation;
		transform.LookAt(_destination, Vector3.up);
		Quaternion rot = Quaternion.Slerp(originalRot, transform.rotation, _turningSpeed * Time.deltaTime);
		
		transform.SetPositionAndRotation(pos, rot);
	}
	
	private void SetAnimatorBools(bool sit, bool walk)
	{
		_animator.SetBool("Sitting", sit);
		_animator.SetBool("Walking", walk);
	}
}
