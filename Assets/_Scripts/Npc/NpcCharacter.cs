using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NpcCharacter : MonoBehaviour
{
    public event EventHandler OnNpcTriggerEnter;
    public event EventHandler OnNpcTriggerExit;

    private const string IS_MOVE_BOOL = "IsMove";
    private const string IS_FARMING_BOOL = "IsFarming";

    [SerializeField] private List<Transform> _transformPoints;
    [Space]
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [Space]
    [SerializeField] private float _raycastDistance;
    [SerializeField] private LayerMask _pointInteractLayer;

    private State _currentState;

    private float _valueToStopAnimation = 0.1f;
    private int _currentTransformPointIndex;

    public void Start()
    {
        _currentTransformPointIndex = _transformPoints.Count - 1;
        _currentState = State.Move;
    }

    private void Update()
    {
        StateMachine();
        HandleInteraction();
    }

    public void ShowAttackPopup()
    {
        OnNpcTriggerEnter?.Invoke(this, EventArgs.Empty);
    }

    internal void HideAttackPopup()
    {
        OnNpcTriggerExit?.Invoke(this, EventArgs.Empty);
    }

    private void StateMachine()
    {
        switch (_currentState)
        {
            case State.Idle:
                InStateIdle();
                break;
            case State.Move:
                InStateMove();
                break;
            case State.Farming:
                InStateFarming();
                break;
            case State.Attack:
                break;
        }
    }

    private void InStateIdle()
    {
        _animator.SetBool(IS_MOVE_BOOL, false);
        _navMeshAgent.isStopped = true;
    }

    private void InStateFarming()
    {
        if (_navMeshAgent.velocity.magnitude < _valueToStopAnimation)
        {
            _animator.SetBool(IS_MOVE_BOOL, false);
            _animator.SetBool(IS_FARMING_BOOL, true);
        }
    }

    private void InStateMove()
    {
        _navMeshAgent.isStopped = false;
        _animator.SetBool(IS_FARMING_BOOL, false);
        _animator.SetBool(IS_MOVE_BOOL, true);

        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            _currentTransformPointIndex = (_currentTransformPointIndex + 1) % _transformPoints.Count;
            _navMeshAgent.SetDestination(_transformPoints[_currentTransformPointIndex].position);
        }
    }

    private void HandleInteraction()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit raycastHit, _raycastDistance,
           _pointInteractLayer))
        {
            if (raycastHit.transform.TryGetComponent(out InteractPoint interactPoint))
            {
                if (_currentState != State.Farming)
                {
                    _navMeshAgent.isStopped = true;
                    _currentState = State.Farming;
                }
            }
        }
    }
}
