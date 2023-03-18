using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

public class Vampire : SelectableObject
{
    public static Vampire Instance { get; private set; }

    private const string IS_MOVE_BOOL = "IsMove";
    private const string ATTACK_TRIGGER = "Attack";

    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [Space]
    [SerializeField] private LayerMask _npcInteractLayer;
    [Space]
    [SerializeField] private float _raycastDistance = 3f;

    private State _currentState;

    private readonly float _valueToStopAnimation = 0.1f;
    private NpcCharacter _currentNpc;

    private void Awake()
    {
        Instance = this;
    }

    public override void Start()
    {
        base.Start();
        _animator.SetBool(IS_MOVE_BOOL, false);
    }

    private void Update()
    {
        StateMachine();
        HandleInteract();
    }

    public override void ClickOnGround(Vector3 point)
    {
        _navMeshAgent.SetDestination(point);
    }

    private void HandleInteract()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit raycastHit, _raycastDistance,
          _npcInteractLayer))
        {
            if (raycastHit.transform.TryGetComponent(out NpcCharacter npcCharacter))
            {
                if (_currentNpc == null)
                {
                    _currentNpc = npcCharacter;
                    _currentNpc.ShowAttackPopup();
                }

            }
            else if (_currentNpc != null)
            {
                _currentNpc.HideAttackPopup();
                _currentNpc = null;

            }
        }
        else if (_currentNpc != null)
        {
            _currentNpc.HideAttackPopup();
            _currentNpc = null;
        }
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
            case State.Attack:
                InStateAttack();
                break;
        }
    }

    public void StartStateAttack()
    {
        if (_currentState != State.Attack)
        {
            _navMeshAgent.isStopped = true;
            _currentState = State.Attack;
        }
    }

    private void InStateAttack()
    {
        _animator.SetBool(IS_MOVE_BOOL, false);
        _animator.SetTrigger(ATTACK_TRIGGER);
    }

    private void InStateIdle()
    {
        _animator.SetBool(IS_MOVE_BOOL, false);

        if (_navMeshAgent.velocity.magnitude > _valueToStopAnimation)
            _currentState = State.Move;
    }

    private void InStateMove()
    {
        _animator.SetBool(IS_MOVE_BOOL, true);

        if (_navMeshAgent.velocity.magnitude < _valueToStopAnimation)
            _currentState = State.Idle;
    }
}
