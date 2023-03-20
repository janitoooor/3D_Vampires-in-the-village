using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class PlayerVampire : SelectableObject
{
    public event EventHandler OnPlayerIsTakeDamage;
    public event EventHandler OnPlayerIsTakeHealth;

    public event EventHandler<OnAddIngridientToBoilerEventArgs> OnAddIngridientToBoiler;
    public class OnAddIngridientToBoilerEventArgs : EventArgs
    {
        public InteractBoiler InteractBoiler;
    }

    public event EventHandler<OnAddIngridientToInventoryEventArgs> OnAddIngridientToInventory;
    public class OnAddIngridientToInventoryEventArgs : EventArgs
    {
        public IngridientSO IngridientSO;
    }

    public static PlayerVampire Instance { get; private set; }

    private const string IS_MOVE_BOOL = "IsMove";
    private const string ATTACK_TRIGGER = "Attack";
    private const string TAKE_DAMAGE_TRIGGER = "TakeDamage";

    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [Space]
    [SerializeField] private LayerMask _interactLayer;
    [Space]
    [SerializeField] private float _raycastDistance = 3f;
    [SerializeField] private float _valueLerpDistanceBetweenPlayerAndNpc = 0.75f;
    [Space]
    [Header("Health values")]
    [SerializeField] private int _maxHealthPlayer = 6;
    [SerializeField] private float _timeToTakeDamageByPoison = 5f;

    public int MaxHealthPlayer => _maxHealthPlayer;

    private State _currentState;

    private int _currentHealthPlayer;
    public int CurrentHealthPlayer => _currentHealthPlayer;

    private readonly float _valueToStopAnimation = 0.1f;

    private bool _isPoisoned;
    private bool _isInteract;

    private IHoverObject _currentHoverObject;
    private IHoverObject _currentHoverObjectFollow;

    private Vector3 _currentFollowPosition;

    private void Awake()
    {
        Instance = this;
    }

    public override void Start()
    {
        base.Start();
        GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;

        _animator.SetBool(IS_MOVE_BOOL, false);
        _currentHealthPlayer = _maxHealthPlayer;

        StartCoroutine(WaitPoison());
    }

    private void GameInput_OnInteractAction(object sender, System.EventArgs e)
    {
        if (_currentHoverObject == null || _isInteract)
            return;

        switch (_currentHoverObject)
        {
            case NpcCharacter:
                StartStateAttack();
                break;
            case IngridientObject:
                AddIngridientToInventory();
                break;
            case InteractBoiler:
                InteractWithBoiler();
                break;
        }
    }

    private void InteractWithBoiler()
    {
        if (_currentHoverObject is InteractBoiler interactBoiler && _currentHoverObject != null)
        {
            if (interactBoiler.IsRecipeFinish)
            {
                IngridientSO ingridientSO = interactBoiler.GiveFinishRecipeSO();

                OnAddIngridientToInventory?.Invoke(this, new OnAddIngridientToInventoryEventArgs
                {
                    IngridientSO = ingridientSO,
                });
            }

            OnAddIngridientToBoiler?.Invoke(this, new OnAddIngridientToBoilerEventArgs
            {
                InteractBoiler = interactBoiler
            });
        }

    }

    private void AddIngridientToInventory()
    {
        if (_currentHoverObject is IngridientObject ingridientObject && _currentHoverObject != null)
        {
            OnAddIngridientToInventory?.Invoke(this, new OnAddIngridientToInventoryEventArgs
            {
                IngridientSO = ingridientObject.IngridientSO,
            });

            ingridientObject.gameObject.SetActive(false);
        }
    }

    private void Update()
    {
        StateMachine();
        ShowReadyForInteract();
    }

    public override void ClickOnHoverObject(IHoverObject hoverObject)
    {
        if (!_isInteract)
        {
            if (_currentHoverObjectFollow != null)
            {
                _currentHoverObjectFollow.HideSelectionIndicator();
                _currentHoverObjectFollow = null;
            }

            _currentHoverObjectFollow = hoverObject;
            hoverObject.ShowSelectionIndicator();
            Vector3 targetPosition;

            switch (_currentHoverObjectFollow)
            {
                case IngridientObject:
                    targetPosition = (_currentHoverObjectFollow as IngridientObject).transform.position;
                    _navMeshAgent.SetDestination(targetPosition);
                    break;
                case InteractBoiler:
                    targetPosition = (_currentHoverObjectFollow as InteractBoiler).transform.position;
                    _navMeshAgent.SetDestination(targetPosition);
                    break;
            }

            ChangeState(State.Move);
        }
    }

    public override void ClickOnGround(Vector3 point)
    {
        if (!_isInteract)
        {
            if (_currentHoverObjectFollow != null)
            {
                _currentHoverObjectFollow.HideSelectionIndicator();
                _currentHoverObjectFollow = null;
            }

            _navMeshAgent.SetDestination(point);

            ChangeState(State.Move);
        }
    }

    public void EndStateAttack()
    {
        ChangeState(State.Idle);
        _isInteract = false;
    }

    public void StartStateAttack()
    {
        if (_currentState != State.Attack && !_isInteract)
        {
            if (_currentHoverObject is NpcCharacter npcCharacter && _currentHoverObject != null)
            {
                _isInteract = true;
                npcCharacter.StopMoveToStartDie();
                _navMeshAgent.SetDestination(Vector3.Lerp(transform.position, npcCharacter.transform.position, _valueLerpDistanceBetweenPlayerAndNpc));
                StartCoroutine(AttackMeleeNpc(npcCharacter));
            }
        }
    }

    public void PoisonPlayer()
    {
        _isPoisoned = true;
    }

    public void TakeDamage()
    {
        _currentHealthPlayer--;
        _animator.SetTrigger(TAKE_DAMAGE_TRIGGER);
        OnPlayerIsTakeDamage?.Invoke(this, EventArgs.Empty);
    }

    public void TakeHealth()
    {
        _currentHealthPlayer++;
        OnPlayerIsTakeHealth?.Invoke(this, EventArgs.Empty);
    }

    private void ChangeState(State state)
    {
        _currentState = state;
    }

    private IEnumerator WaitPoison()
    {
        yield return new WaitUntil(() =>
        {
            return _isPoisoned;
        });

        StartCoroutine(ChangeHealthByPoison());
    }

    private IEnumerator ChangeHealthByPoison()
    {
        yield return new WaitForSeconds(_timeToTakeDamageByPoison);
        TakeDamage();

        if (_currentHealthPlayer > 0)
            StartCoroutine(ChangeHealthByPoison());
        else
            print("Player is die");
    }

    private void ShowReadyForInteract()
    {
        if (Physics.Raycast(transform.position, transform.forward, out RaycastHit raycastHit, _raycastDistance,
          _interactLayer))
        {
            if (raycastHit.transform.TryGetComponent(out IHoverObject hoverObject))
            {
                if (_currentHoverObject == null)
                    OnRayCastHitHoverObject(hoverObject);
            }
            else if (_currentHoverObject != null && _currentState != State.Attack)
            {
                OnRayCastUnHitHoverObject();
            }
        }
        else if (_currentHoverObject != null && _currentState != State.Attack)
        {
            OnRayCastUnHitHoverObject();
        }
    }

    private void OnRayCastHitHoverObject(IHoverObject hoverObject)
    {
        _currentHoverObject = hoverObject;
        _currentHoverObject.ShowInteract();
        _currentHoverObject.ShowSelectionIndicator();
    }

    private void OnRayCastUnHitHoverObject()
    {
        _currentHoverObject.HideInteract();
        _currentHoverObject.HideSelectionIndicator();
        _currentHoverObject = null;
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

    private IEnumerator AttackMeleeNpc(NpcCharacter npcCharacter)
    {
        yield return new WaitUntil(() =>
        {
            return _navMeshAgent.velocity.magnitude < _valueToStopAnimation;
        });

        ChangeState(State.Attack);
        _animator.SetTrigger(ATTACK_TRIGGER);
        npcCharacter.StartDie();
    }

    private void InStateAttack()
    {
        _navMeshAgent.isStopped = true;
        _animator.SetBool(IS_MOVE_BOOL, false);
    }

    private void InStateIdle()
    {
        _animator.SetBool(IS_MOVE_BOOL, false);

        if (_navMeshAgent.velocity.magnitude > _valueToStopAnimation)
            ChangeState(State.Move);
    }

    private void InStateMove()
    {
        _navMeshAgent.isStopped = false;
        _animator.SetBool(IS_MOVE_BOOL, true);

        if (_navMeshAgent.velocity.magnitude < _valueToStopAnimation && !_isInteract)
            ChangeState(State.Idle);


        if (_currentHoverObjectFollow != null && !_isInteract && _currentHoverObjectFollow is NpcCharacter npcCharacter)
        {
            if (_currentFollowPosition != npcCharacter.transform.position)
            {
                _currentFollowPosition = npcCharacter.transform.position;
                _navMeshAgent.SetDestination(_currentFollowPosition);
            }
        }
    }
}
