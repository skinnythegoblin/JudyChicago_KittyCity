using CoffeyUtils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum CatState
{
	Idle,
	Walking,
	Sitting
}

[RequireComponent(typeof(DirectedNavMeshAgent))]
[RequireComponent(typeof(CatPersonality))]
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

	private CatPersonality _personality;

	public CatState State => _state;
	
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
		NavBaker.OnNavigationBaked += SnapDestinationToFloor;
	}

	private void OnDisable()
	{
		NavBaker.OnNavigationBaked -= SnapDestinationToFloor;
	}

	private void Start()
	{
		_personality = GetComponent<CatPersonality>();
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
					// Reached destination. Set new state
					SetRandomState();
				}
			}
			else
			{
				if (!_personality.IsTouched) SetRandomState();
				else SetRandomStateStopped();
			}
		}
	}
	
	[Button(Mode = RuntimeMode.OnlyPlaying, Spacing = 24)]
	public void SetRandomState()
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

	// Forces cat to choose a non-moving state
	[Button(Mode = RuntimeMode.OnlyPlaying, Spacing = 24)]
	public void SetRandomStateStopped()
	{
		// The cat cannot stand up if it is already sitting. This is completely arbitrary, feel free to change if wanted.
		if (_state == CatState.Sitting) SetSitting();
		else
		{
			int nextAction = Random.Range(0, 2);
			switch (nextAction)
			{
				case 0:
					SetIdle();
					break;
				case 1:
					SetSitting();
					break;
			}
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

	public void MoveToPosition(Vector3 position)
	{
		CatState prevState = _state;
		_state = CatState.Walking;
		SetAnimatorBools(false, true);
		SetNewDestination(position);
		_actionTimer = prevState == CatState.Sitting ? 2 : 0;

		if (_useDestinationDebug) _destinationDebug.SetActive(true);
	}
	
	private void SetNewDestination()
	{
		float x, z;
		// TODO: Once the gallery floor plan is added and baked, this should be modified to choose a point on the baked NavMesh as opposed to a point within the play area
		if (FloorMappingController.FloorBounds[0] != FloorMappingController.FloorBounds[1] &&
			FloorMappingController.FloorBounds[2] != FloorMappingController.FloorBounds[3])
		{
			x = Random.Range(FloorMappingController.FloorBounds[0], FloorMappingController.FloorBounds[1]);
			z = Random.Range(FloorMappingController.FloorBounds[2], FloorMappingController.FloorBounds[3]);
		}
		else
		{
			x = Random.Range(-2f, 2f);
			z = Random.Range(-2f, 2f);
		}

		_destination = new Vector3(x, FloorMappingController.FloorLevel, z);
		if (_useDestinationDebug) _destinationDebug.transform.position = _destination + new Vector3(0, 0.05f, 0);
		StartCoroutine(TryMove());
	}

	// Sets the destination directly, throwing an error if the cat can't get to that point.
	private void SetNewDestination(Vector3 position)
	{
		position.y = FloorMappingController.FloorLevel;
		if (!GetComponent<NavMeshAgent>().CalculatePath(position, new NavMeshPath()))
		{
			Debug.LogError("Destination is not navigable to this NavMeshAgent via the existing NavMesh.", this);
			return;
		}
		else
		{
			_destination = position;
			if (_useDestinationDebug) _destinationDebug.transform.position = _destination + (Vector3.up * 0.05f);
			StartCoroutine(TryMove());
		}
	}

	private void SnapDestinationToFloor()
	{
		if (Mathf.Clamp(_destination.x, FloorMappingController.FloorBounds[0], FloorMappingController.FloorBounds[1]) != _destination.x ||
			Mathf.Clamp(_destination.z, FloorMappingController.FloorBounds[2], FloorMappingController.FloorBounds[3]) != _destination.z)
		{
			SetNewDestination();
		}
		else
		{
			_destination.y = FloorMappingController.FloorLevel;
			GetComponent<DirectedNavMeshAgent>().MoveToLocation(_destination);
		}
	}

	private IEnumerator TryMove()
	{
		// NOTE: This function should be phased out once the official gallery floor plan is implemented

		// Attempts to moveto a destination if there is an available NavMesh. Waits if no NavMesh has been baked yet.
		if (NavBaker.HasBaked)
		{
			GetComponent<DirectedNavMeshAgent>().MoveToLocation(_destination);
		}
		else
		{
			yield return new WaitForSeconds(1);
			TryMove();
		}
	}
	
	private void SetAnimatorBools(bool sit, bool walk)
	{
		_animator.SetBool("Sitting", sit);
		_animator.SetBool("Walking", walk);
	}

	
}
