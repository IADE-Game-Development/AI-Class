using System;
using UnityEngine;

public class STATE_Patrol : BaseState
{
    private readonly AIAgent _owner;
    private int currentPathIndex = -1;

    public STATE_Patrol(AIAgent owner) : base(owner.gameObject)
    {
        _owner = owner;
    }

    // Runs every frame
    public override Type Tick()
    {
        if (_owner.PatrolPoints.Count == 0)
            return null;

        if (_owner.AgentReachedDestination)
        {
            currentPathIndex++;
            if (currentPathIndex >= _owner.PatrolPoints.Count)
                currentPathIndex = 0;

            _owner.NavMeshAgent.SetDestination(_owner.PatrolPoints[currentPathIndex]);
        }

        return null;
    }

    // Runs when we enter this state
    public override void OnEnter(BaseState oldState)
    {
        if (_owner.PatrolPoints.Count == 0)
            return;

        currentPathIndex = 0;
        _owner.NavMeshAgent.SetDestination(_owner.PatrolPoints[currentPathIndex]);
        _owner.OnTriggerEnterEvent += OnTriggerEnter;
        _owner.OnTriggerExitEvent += OnTriggerExit;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _owner.StateMachine.SwitchToNewState(typeof(STATE_Chase));
    }

    private void OnTriggerExit(Collider other)
    {
    }

    // Runs when we exit this state
    public override void OnExit(BaseState newState)
    {
        _owner.OnTriggerEnterEvent -= OnTriggerEnter;
        _owner.OnTriggerExitEvent -= OnTriggerExit;
    }

    private int GetNextPathIndex()
    {
        return (currentPathIndex + 1) % _owner.PatrolPoints.Count;
    }
}