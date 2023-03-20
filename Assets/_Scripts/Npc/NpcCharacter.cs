using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class NpcCharacter : MonoBehaviour, IHoverObject
{
    public event EventHandler OnNpcTriggerEnter;
    public event EventHandler OnNpcTriggerExit;
    public event EventHandler OnNpcWarningStateExit;
    public event EventHandler OnNpcWarningStateEnter;

    private protected const string IS_MOVE_BOOL = "IsMove";
    private protected const string DIE_TRIGGER = "Die";
    private protected const string WARNING_BOOL = "Warning";

    [SerializeField] private protected List<Transform> _transformPoints;
    [Space]
    [SerializeField] private protected Animator _animator;
    [SerializeField] private protected NavMeshAgent _navMeshAgent;
    [SerializeField] private protected GameObject _selectionsIndicator;
    [Space]
    [Header("Raycast to pointsInteract")]
    [SerializeField] private protected float _raycastDistanceToPointInteract;
    [SerializeField] private protected LayerMask _pointInteractLayer;
    [Space]
    [Header("Box raycast to player")]
    [SerializeField] private protected float _raycastDistanceToPlayer;
    [SerializeField] private protected float _npcRaycastRadius;
    [SerializeField] private protected LayerMask _playerLayer;
    [Space]
    [Header("Coroutines Values")]
    [SerializeField] private protected float _waitTimeToRunAway = 1.5f;
    [Space]
    [SerializeField] private protected float _fleeDistance = 5f;

    private protected State _currentState;

    private protected readonly float _valueToStopAnimation = 0.1f;
    private int _currentTransformPointIndex;

    private protected bool _isStartDie;
    private protected bool _isRunAway;

    public virtual void Start()
    {
        _currentTransformPointIndex = _transformPoints.Count - 1;
        HideSelectionIndicator();
        HideInteract();

        ChangeState(State.Move);
    }

    public virtual void Update()
    {
        HandleInteraction();
        StateMachine();
    }

    private protected virtual void OnDisable()
    {
        PlayerVampire.Instance.TakeHealth();
    }

    public virtual void StartDie()
    {
        ChangeState(State.Die);
        _animator.SetTrigger(DIE_TRIGGER);
    }

    public virtual void ShowSelectionIndicator()
    {
        _selectionsIndicator.SetActive(true);
    }

    public virtual void HideSelectionIndicator()
    {
        _selectionsIndicator.SetActive(false);
    }

    public virtual void ShowInteract()
    {
        if (!_isStartDie)
            OnNpcTriggerEnter?.Invoke(this, EventArgs.Empty);
    }

    public virtual void HideInteract()
    {
        if (!_isStartDie)
            OnNpcTriggerExit?.Invoke(this, EventArgs.Empty);
    }

    private protected virtual void StateMachine()
    {
        switch (_currentState)
        {
            case State.Idle:
                InStateIdle();
                break;
            case State.Move:
                InStateMove();
                break;
            case State.Die:
                InStateDie();
                break;
            case State.RunAway:
                InStateRunAway();
                break;
            case State.Warning:
                InStateWarning();
                break;
        }
    }

    private protected virtual void ChangeState(State state)
    {
        _currentState = state;
    }

    private protected virtual void InStateWarning()
    {
        OnNpcWarningStateEnter?.Invoke(this, EventArgs.Empty);
        _animator.SetBool(IS_MOVE_BOOL, false);
        _animator.SetBool(WARNING_BOOL, true);
        _navMeshAgent.isStopped = true;

        if (!_isStartDie)
            StartCoroutine(WarningWaitToRunAway());
    }

    private IEnumerator WarningWaitToRunAway()
    {
        yield return new WaitForSeconds(_waitTimeToRunAway);

        if (_navMeshAgent.velocity.magnitude > _valueToStopAnimation)
            _animator.SetBool(WARNING_BOOL, false);

        float distanceToPlayer = Vector3.Distance(transform.position, PlayerVampire.Instance.transform.position);

        if (distanceToPlayer < _fleeDistance)
            ChangeState(State.RunAway);
        else
            ChangeState(State.Move);

        OnNpcWarningStateExit?.Invoke(this, EventArgs.Empty);
    }

    private protected virtual void InStateDie()
    {
        _animator.SetBool(IS_MOVE_BOOL, false);
        _animator.SetBool(WARNING_BOOL, false);
        _navMeshAgent.isStopped = true;
    }

    private protected virtual void InStateIdle()
    {
        _animator.SetBool(IS_MOVE_BOOL, false);
        _animator.SetBool(WARNING_BOOL, false);
        _navMeshAgent.isStopped = true;
    }

    private protected virtual void InStateMove()
    {
        _navMeshAgent.isStopped = false;
        _animator.SetBool(IS_MOVE_BOOL, true);

        if (_navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            _currentTransformPointIndex = (_currentTransformPointIndex + 1) % _transformPoints.Count;
            _navMeshAgent.SetDestination(_transformPoints[_currentTransformPointIndex].position);
        }
    }

    private IEnumerator WaitToStopRunAway()
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

    private protected virtual void InStateRunAway()
    {
        if (_isStartDie)
        {
            ChangeState(State.Idle);
            return;
        }

        _isRunAway = true;
        _navMeshAgent.isStopped = false;
        _animator.SetBool(IS_MOVE_BOOL, true);

        Vector3 direction = (transform.position - PlayerVampire.Instance.transform.position);
        direction.Normalize();
        Vector3 fleePosition = transform.position + direction * _fleeDistance;
        _navMeshAgent.SetDestination(fleePosition);

        StartCoroutine(WaitToStopRunAway());
    }

    private protected abstract void HandleInteraction();

    public void StopMoveToStartDie()
    {
        ChangeState(State.Idle);
        _navMeshAgent.isStopped = true;
        HideInteract();
        _isStartDie = true;
    }
}
