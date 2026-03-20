using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AIAgent : MonoBehaviour
{
    private StateMachine _stateMachine;
    public StateMachine StateMachine => _stateMachine;


    [SerializeField] private NavMeshAgent agent;
    public NavMeshAgent NavMeshAgent => agent;

    public bool AgentReachedDestination =>
        agent.isOnNavMesh &&
        !agent.pathPending &&
        agent.remainingDistance != Mathf.Infinity &&
        agent.remainingDistance <= agent.stoppingDistance &&
        Vector3.Distance(transform.position, agent.destination) <= agent.stoppingDistance &&
        agent.velocity.sqrMagnitude == 0f;

    [SerializeField] private GameObject chaseObject;

    public event Action<Collider> OnTriggerExitEvent;
    public event Action<Collider> OnTriggerEnterEvent;
    public List<Vector3> PatrolPoints = new List<Vector3>();
    public NPCAttack AttackComponent;

    private void Awake()
    {
        InitializeStateMachine();
    }

    void InitializeStateMachine()
    {
        var states = new Dictionary<Type, BaseState>()
        {
            { typeof(STATE_Patrol), new STATE_Patrol(this) },
            { typeof(STATE_Chase), new STATE_Chase(this) },
            { typeof(STATE_Attack), new STATE_Attack(this) },
        };

        _stateMachine = GetComponent<StateMachine>();
        if (_stateMachine == null)
            _stateMachine = gameObject.AddComponent<StateMachine>();
        _stateMachine.SetStates(states);
    }

    private void Update()
    {
    }

    private void OnTriggerExit(Collider other)
    {
        OnTriggerExitEvent?.Invoke(other);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterEvent?.Invoke(other);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        foreach (var point in PatrolPoints)
        {
            Gizmos.DrawSphere(point, 0.5f);
        }
    }
}

public enum AIState
{
    PATROL,
    CHASE,
    ATTACK
}