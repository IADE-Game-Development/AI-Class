using System;
using UnityEngine;

public class STATE_Attack : BaseState
{
    private readonly AIAgent _owner;
    private float cooldown;

    public STATE_Attack(AIAgent owner) : base(owner.gameObject)
    {
        _owner = owner;
    }

    // Runs every frame
    public override Type Tick()
    {
        if (cooldown <= 0f)
        {
            Debug.Log("Attacking");
            // LeanTween scale up and down
            _owner.gameObject.transform.LeanScale(Vector3.one * 1.5f, 0.2f).setEasePunch();
            cooldown = 2f;
        }
        else
        {
            cooldown -= Time.deltaTime;
        }

        return null;
    }

    // Runs when we enter this state
    public override void OnEnter(BaseState oldState)
    {
        cooldown = 0f;
        _owner.AttackComponent.OnExitAttackRange += OnExitRange;
    }

    // Runs when we exit this state
    public override void OnExit(BaseState newState)
    {
        _owner.AttackComponent.OnExitAttackRange -= OnExitRange;
    }

    private void OnExitRange(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        _owner.StateMachine.SwitchToNewState(typeof(STATE_Chase));
    }
}