using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
	[SerializeField] private float _actionTimer;
	[SerializeField] private CatState _state;
	[SerializeField] private Vector3 _destination;
	
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
					_actionTimer = SetRandomCatState();
				}
				else
				{
					WalkToDestination();
				}
			}
			else
			{
				_actionTimer = SetRandomCatState();
			}
		}
	}
	
	private float SetRandomCatState()
	{
		int nextAction = Random.Range(0, 3); // 0, 1, 2
		
		CatState prevState = _state;
		switch (nextAction)
		{
		case 0:
			// Idle
			_state = CatState.Idle;
			SetAnimatorBools(false, false);
			return IdleTime;
		case 1:
			// Sit
			_state = CatState.Sitting;
			SetAnimatorBools(true, false);
			return SittingTime;
		case 2:
			// Walk
			_state = CatState.Walking;
			SetAnimatorBools(false, true);
			SetNewDestination();
			return prevState == CatState.Sitting ? 2 : 0;
		default:
			return -1;
		}
	}
	
	private void WalkToDestination()
	{
		//Vector3 pos = Vector3.Slerp(transform.position, _destination, _walkingSpeed * Time.deltaTime);
		Vector3 pos = transform.position + transform.forward * _walkingSpeed * Time.deltaTime;
		
		Quaternion originalRot = transform.rotation;
		transform.LookAt(_destination, Vector3.up);
		Quaternion rot = Quaternion.Slerp(originalRot, transform.rotation, _turningSpeed * Time.deltaTime);
		
		transform.SetPositionAndRotation(pos, rot);
	}
	
	private void SetNewDestination()
	{
		float x = Random.Range(-2f, 2f);
		float z = Random.Range(-2f, 2f);
		_destination = new Vector3(x, 0, z);
	}
	
	private void SetAnimatorBools(bool sit, bool walk)
	{
		_animator.SetBool("Sitting", sit);
		_animator.SetBool("Walking", walk);
	}
}
