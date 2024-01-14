using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcCharacterHunter : NpcCharacter
{
    private const string ATTACK_TRIGGER = "Attack";

    [SerializeField] private float _timeCooldownToAttack = 1.5f;

    private float _timer;

    public override void Start()
    {
        base.Start();
    }

    private protected override void HandleInteraction()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit raycastHit, _raycastDistanceToPointInteract,
          _pointInteractLayer))
        {
            if (raycastHit.transform.TryGetComponent(out SelectableCollider selectableCollider))
            {
                if (_currentState != State.Warning && _currentState == State.RunAway)
                {
                    _timer -= Time.deltaTime;
                    if (_timer < 0)
                    {
                        _animator.SetTrigger(ATTACK_TRIGGER);
                        _timer = _timeCooldownToAttack;
                    }
                }
            }
        }

        if (Physics.BoxCast(transform.position, Vector3.one * _npcRaycastRadius, transform.forward,
                Quaternion.identity, _raycastDistanceToPlayer, _playerLayer))
        {
            if (_currentState == State.Move)
                ChangeState(State.Warning);
        }
    }

    private protected override void InStateRunAway()
    {
        if (_isStartDie)
        {
            ChangeState(State.Idle);
            return;
        }

        _isRunAway = true;
        _navMeshAgent.isStopped = false;
        _animator.SetBool(IS_MOVE_BOOL, true);

        _navMeshAgent.SetDestination(PlayerVampire.Instance.transform.position);

        StartCoroutine(WaitTimeToStopRunAway());
    }

    private IEnumerator WaitTimeToStopRunAway()
    {
        yield return new WaitUntil(() =>
        {
            if (_currentState != State.Die)
            {
                float distanceToPlayer = Vector3.Distance(transform.position, PlayerVampire.Instance.transform.position);
                return distanceToPlayer > _fleeDistance;
            }

            return false;
        });

        ChangeState(State.Move);
    }
}
