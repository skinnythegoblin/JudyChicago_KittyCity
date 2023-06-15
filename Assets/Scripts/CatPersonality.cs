using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CatController))]
public class CatPersonality : MonoBehaviour
{
    [Header("Hand Reaction")]
    [Tooltip("If enabled, this cat stops in place when a user touches it.")]
    [SerializeField] bool _stopsWhenTouched = true;
    [Tooltip("If enabled, this cat cannot be beckoned with hand gestures.")]
    [SerializeField] bool _detectHandGestures = true;
    [Tooltip("The transform that detection checks should originate from.")]
    [SerializeField] Transform _detectionOrigin;
    [Tooltip("The chance per hand gesture that the cat will respond to a gesture.")]
    [SerializeField] float _detectionProbability = 0.5f;
    [Tooltip("The maximum distance that the cat will respond to a gesture at.")]
    [SerializeField] float _detectionRange = 1f;
    [Tooltip("The maximum difference in height the cat will respond to a gesture at.")]
    [SerializeField] float _detectionHeight = 0.5f;
    [Tooltip("The maximum angle that the cat will respond to a gesture at.")]
    [SerializeField] float _detectionFOV = 90f;

    private CatController _controller;

    public bool IsTouched { get; private set; } = false;
    
    protected virtual void OnEnable()
    {
        GestureTracker.OnPinchPerformed += TryApproachHand;
    }

    protected virtual void OnDisable()
    {
        GestureTracker.OnPinchPerformed -= TryApproachHand;
    }

    void Start()
    {
        _controller = GetComponent<CatController>();
    }

    protected virtual void TryApproachHand(Vector3 handPosition)
    {
        if (_detectHandGestures && Random.Range(0.0f, 1.0f) < _detectionProbability &&
            Vector3.Distance(_detectionOrigin.position, handPosition) <= _detectionRange && IsDetectable(handPosition))
        {
            _controller.MoveToPosition(handPosition);
        }
    }

    bool IsDetectable(Vector3 position)
    {
        // inRange is not used for now, as the distance check can prevent cats from doing unnecessary expensive operations when checked on its own
        // bool inRange = Vector3.Distance(_detectionOrigin.position, position) <= _detectionRange;
        bool inHeight = Mathf.Abs(_detectionOrigin.position.y - position.y) <= _detectionHeight;

        // Height difference is handled separately, so we want to make sure there is no difference in height before checking the FOV.
        position.y = _detectionOrigin.position.y;
        bool inFOV = Vector3.Angle(_detectionOrigin.forward, position - _detectionOrigin.position) <= _detectionFOV;

        if (!inTime)
        {
            Debug.Log("Hand is too soon!");
        }

        return inHeight && inFOV;
    }

    void OnTriggerEnter(Collider other)
	{
		if (_stopsWhenTouched && other.GetComponent<OVRHand>() != null)
		{
			IsTouched = true;
			if (_controller.State == CatState.Walking) GetComponent<DirectedNavMeshAgent>().Stop();
			_controller.SetRandomStateStopped();
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (_stopsWhenTouched && other.GetComponent<OVRHand>() != null)
		{
			IsTouched = false;
		}
	}
}
