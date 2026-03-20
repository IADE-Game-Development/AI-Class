using System;
using Unity.VisualScripting;
using UnityEngine;

public class STATE_Chase : BaseState
{
    private readonly AIAgent _owner;
    private GameObject chasingObject;

    public STATE_Chase(AIAgent owner) : base(owner.gameObject)
    {
        _owner = owner;
    }

    // Runs every frame
    public override Type Tick()
    {
        _owner.NavMeshAgent.SetDestination(chasingObject.transform.position);
        return null;
    }

    // Runs when we enter this state
    public override void OnEnter(BaseState oldState)
    {
        chasingObject = GameObject.FindWithTag("Player");
        _owner.OnTriggerExitEvent += OnTriggerExit;
        _owner.AttackComponent.OnEnterAttackRange += OnEnterAttackRange;
    }

    // Runs when we exit this state
    public override void OnExit(BaseState newState)
    {
        _owner.OnTriggerExitEvent -= OnTriggerExit;
        _owner.AttackComponent.OnEnterAttackRange -= OnEnterAttackRange;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _owner.StateMachine.SwitchToNewState(typeof(STATE_Patrol));
    }

    private void OnEnterAttackRange(Collider collider)
    {
        _owner.StateMachine.SwitchToNewState(typeof(STATE_Attack));
    }
}