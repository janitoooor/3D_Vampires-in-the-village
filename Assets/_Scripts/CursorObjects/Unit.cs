using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Unit : SelectableObject
{
    private const string IS_MOVE_BOOL = "IsMove";

    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _navMeshAgent;

    private readonly float _valueToStopAnimation = 0.1f;

    public override void Start()
    {
        base.Start();
        _animator.SetBool(IS_MOVE_BOOL, false);
    }

    private void Update()
    {
        if (_navMeshAgent.velocity.magnitude < _valueToStopAnimation)
            _animator.SetBool(IS_MOVE_BOOL, false);
        else
            _animator.SetBool(IS_MOVE_BOOL, true);
    }

    public override void ClickOnGround(Vector3 point)
    {
        _navMeshAgent.SetDestination(point);
    }
}
