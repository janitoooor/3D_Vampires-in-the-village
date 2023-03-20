using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class NpcCharacterFarmer : NpcCharacter
{
    private const string IS_FARMING_BOOL = "IsFarming";

    [Header("Farmer Coroutines Values")]
    [SerializeField] private protected float _timeFarming = 5f;
    [SerializeField] private protected float _waitTimeToCanFarming = 5f;

    private bool _isCanFarming;

    public override void Start()
    {
        base.Start();
        _isCanFarming = true;
    }

    private protected override void HandleInteraction()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit raycastHit, _raycastDistanceToPointInteract,
           _pointInteractLayer))
        {
            if (raycastHit.transform.TryGetComponent(out InteractPoint interactPoint))
            {
                if (_currentState != State.Farming && _isCanFarming && !_isRunAway && _currentState != State.Warning && !_isStartDie)
                {
                    _navMeshAgent.isStopped = true;
                    ChangeState(State.Farming);
                }
            }
        }
        if (Physics.BoxCast(transform.position, Vector3.one * _npcRaycastRadius, transform.forward,
                Quaternion.identity, _raycastDistanceToPlayer, _playerLayer))
        {

            if (_currentState == State.Move || _currentState == State.Farming)
                ChangeState(State.Warning);
        }
    }

    private protected override void StateMachine()
    {
        if (_currentState != State.Farming)
            _animator.SetBool(IS_FARMING_BOOL, false);

        base.StateMachine();

        switch (_currentState)
        {
            case State.Farming:
                InStateFarming();
                break;
        }
    }

    private protected override void InStateWarning()
    {
        base.InStateWarning();
        _isCanFarming = false;
    }

    private protected override void InStateDie()
    {
        _isCanFarming = false;
        base.InStateDie();
    }

    public override void StartDie()
    {
        _isCanFarming = false;
        base.StartDie();
    }

    private protected override void InStateRunAway()
    {
        base.InStateRunAway();
        _isCanFarming = false;
    }

    private IEnumerator StopFarmingStartMove()
    {
        yield return new WaitForSeconds(_timeFarming);
        _isCanFarming = false;

        if (!_isStartDie)
            ChangeState(State.Move);

        StartCoroutine(TimeToCanFarming());
    }

    private IEnumerator TimeToCanFarming()
    {
        yield return new WaitForSeconds(_waitTimeToCanFarming);
        _isCanFarming = true;
    }

    private void InStateFarming()
    {
        if (_navMeshAgent.velocity.magnitude < _valueToStopAnimation)
        {
            _animator.SetBool(IS_MOVE_BOOL, false);
            _animator.SetBool(IS_FARMING_BOOL, true);
            StartCoroutine(StopFarmingStartMove());
        }
    }
}
