using System;
using UnityEngine;
using UnityEngine.Events;

public class NPCAttack : MonoBehaviour
{
    public event Action<Collider> OnEnterAttackRange;
    public event Action<Collider> OnExitAttackRange;

    private void OnTriggerEnter(Collider other)
    {
        OnEnterAttackRange?.Invoke(other);
    }

    private void OnTriggerExit(Collider other)
    {
        OnExitAttackRange?.Invoke(other);
    }
}