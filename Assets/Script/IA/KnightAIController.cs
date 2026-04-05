using UnityEngine;
using UnityEngine.AI;

public enum StateType
{
    None,
    Patrol,
    Follow,
    Attack
}

public class KnightAIController : MonoBehaviour
{
    [SerializeField] private StateType state = StateType.Patrol;
    [SerializeField] private StateType nextState = StateType.None;
    [SerializeField] private GameObject target;
    [SerializeField] private GameObject navpoint;
    [SerializeField] private float attackDistance = 1.5f;

    [Header("Movement Speeds")]
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float chaseSpeed = 5f;

    [Header("Labyrinth Reference")]
    [SerializeField] private LabyrinthManager _labyrinthManager;

    private NavMeshAgent _agent;
    private Animator _animator;
    private SightPerception _sight;
    private bool _isCatching = false;

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
        _animator = GetComponent<Animator>();
        _sight = GetComponent<SightPerception>();
    }

    private void Update()
    {
        //Si j'ai une condition de changement d'�tat
        if (TestChangeState())
        {
            //alors je change d'�tat. 
            ChangeState();
        }
        Behaviour();
    }

    private bool TestChangeState()
    {
        float distanceToTarget = Vector3.Distance(target.transform.position, transform.position);

        switch (state)
        {
            case StateType.Attack:
                if (!_sight.IsDetected)
                {
                    nextState = StateType.Patrol;
                    return true;
                }
                if (distanceToTarget > attackDistance)
                {
                    nextState = StateType.Follow;
                    return true;
                }
                break;

            case StateType.Patrol:
                if (_sight.IsDetected)
                {
                    nextState = distanceToTarget <= attackDistance
                        ? StateType.Attack
                        : StateType.Follow;
                    return true;
                }
                break;

            case StateType.Follow:
                if (!_sight.IsDetected)
                {
                    nextState = StateType.Patrol;
                    return true;
                }
                if (distanceToTarget <= attackDistance)
                {
                    nextState = StateType.Attack;
                    return true;
                }
                break;
        }
        return false;
    }

    private void ChangeState()
    {
        EndState();
        state = nextState;
        StartState();
    }

    private void StartState() { }

    private void EndState()
    {
        switch (state)
        {
            case StateType.Follow:
            case StateType.Patrol:
                _agent.SetDestination(transform.position);
                break;
            case StateType.Attack:
                _isCatching = false; // ← reset du flag quand on quitte l'état Attack
                break;
        }
    }


    private void Behaviour()
    {
        switch (state)
        {
            case StateType.Patrol: PatrolBehavior(); break;
            case StateType.Follow: FollowBehavior(); break;
            case StateType.Attack: AttackBehavior(); break;
        }
    }

    private void PatrolBehavior()
    {
        _agent.speed = patrolSpeed;
        _agent.SetDestination(navpoint.transform.position);
        _animator.SetFloat("Speed", _agent.velocity.magnitude);
    }

    private void FollowBehavior()
    {
        _agent.speed = chaseSpeed;
        _agent.SetDestination(target.transform.position);
        _animator.SetFloat("Speed", _agent.velocity.magnitude);
    }

    private void AttackBehavior()
    {
        _animator.SetTrigger("Smash");

        float distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
        if (distanceToTarget <= attackDistance && !_isCatching)
        {
            _isCatching = true;
            _labyrinthManager?.OnPlayerCaught();
        }
    }

    /// <summary>Remet l'IA en état Patrol depuis le LabyrinthManager lors d'un reset.</summary>
    public void ResetToPatrol()
    {
        _agent.SetDestination(transform.position);
        state = StateType.Patrol;
        nextState = StateType.None;
        _isCatching = false;   // ← ajouter
    }
}

