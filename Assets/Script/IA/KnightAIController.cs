using UnityEngine;
using UnityEngine.AI;

public enum StateType
{
    None,
    Patrol,
    LookAround,
    Follow,
    Attack
}

public class KnightAIController : MonoBehaviour
{
    [SerializeField] private StateType state = StateType.Patrol;
    [SerializeField] private StateType nextState = StateType.None;
    [SerializeField] private GameObject target;
    [SerializeField] private float attackDistance = 1.5f;

    [Header("Patrol Waypoints")]
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private float waypointReachedDistance = 0.5f;

    [Header("Look Around")]
    [SerializeField] [Range(0f, 1f)] private float lookAroundChance = 0.5f;
    [SerializeField] private float lookAroundDuration = 3f;

    [Header("Movement Speeds")]
    [SerializeField] private float patrolSpeed = 2f;
    [SerializeField] private float chaseSpeed = 5f;

    [Header("Labyrinth Reference")]
    [SerializeField] private LabyrinthManager _labyrinthManager;

    private NavMeshAgent _agent;
    private Animator _animator;
    private SightPerception _sight;
    private bool _isCatching = false;
    private int _currentWaypointIndex = 0;
    private float _lookAroundTimer = 0f;

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

            case StateType.LookAround:
                if (_sight.IsDetected)
                {
                    nextState = distanceToTarget <= attackDistance
                        ? StateType.Attack
                        : StateType.Follow;
                    return true;
                }
                if (_lookAroundTimer <= 0f)
                {
                    nextState = StateType.Patrol;
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

    private void StartState()
    {
        if (state == StateType.LookAround)
        {
            _lookAroundTimer = lookAroundDuration;
            _agent.SetDestination(transform.position);
            _animator.SetBool("LookAround", true);
            _animator.SetFloat("Speed", 0f);
        }
    }

    private void EndState()
    {
        switch (state)
        {
            case StateType.LookAround:
                _animator.SetBool("LookAround", false);
                break;
            case StateType.Follow:
            case StateType.Patrol:
                _agent.SetDestination(transform.position);
                break;
            case StateType.Attack:
                _isCatching = false;
                break;
        }
    }


    private void Behaviour()
    {
        switch (state)
        {
            case StateType.Patrol:     PatrolBehavior();     break;
            case StateType.LookAround: LookAroundBehavior(); break;
            case StateType.Follow:     FollowBehavior();     break;
            case StateType.Attack:     AttackBehavior();     break;
        }
    }

    private void PatrolBehavior()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        _agent.speed = patrolSpeed;

        Transform currentWaypoint = waypoints[_currentWaypointIndex];
        _agent.SetDestination(currentWaypoint.position);

        bool waypointReached = !_agent.pathPending && _agent.remainingDistance <= waypointReachedDistance;
        if (waypointReached)
        {
            _currentWaypointIndex = (_currentWaypointIndex + 1) % waypoints.Length;

            if (Random.value <= lookAroundChance)
            {
                nextState = StateType.LookAround;
                ChangeState();
                return;
            }
        }

        _animator.SetFloat("Speed", _agent.velocity.magnitude);
    }

    private void LookAroundBehavior()
    {
        _lookAroundTimer -= Time.deltaTime;
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
    }

    /// <summary>Appelé par un Animation Event sur l'animation Smash au moment de l'impact.</summary>
    public void OnSmashHit()
    {
        if (_isCatching) return;

        float distanceToTarget = Vector3.Distance(target.transform.position, transform.position);
        if (distanceToTarget <= attackDistance)
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
        _isCatching = false;
        _currentWaypointIndex = 0;
    }
}

