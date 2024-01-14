using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class Character : MonoBehaviour
{
    private const string IS_MOVE_BOOL = "IsMove";

    [SerializeField] private List<Transform> _transformPoints;
    [Space]
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _navMeshAgent;

    private State _currentState;

    private int _currentTransformPointIndex;
    private readonly float _valueToStopAnimation = 0.1f;

    public void Start()
    {
        _currentTransformPointIndex = _transformPoints.Count - 1;
        _currentState = State.Move;
    }

    private void Update()
    {
        StateMachine();
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
                _animator.SetBool(IS_MOVE_BOOL, false);
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

    private void InStateMove()
    {
        _navMeshAgent.isStopped = false;
        _animator.SetBool(IS_MOVE_BOOL, true);

        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            _currentTransformPointIndex = (_currentTransformPointIndex + 1) % _transformPoints.Count;
            _navMeshAgent.SetDestination(_transformPoints[_currentTransformPointIndex].position);
        }
    }
}
