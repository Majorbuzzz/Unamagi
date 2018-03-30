using UnityEngine;
using System.Collections;
using System;

public abstract class MonsterBody : MonoBehaviour
{
    public MonsterHealth Health;
    internal Animator Animator;

    private void Start()
    {
        Animator = GetComponent<Animator>();
        StartOverride();
    }    

    internal virtual void YouGotHurt(GameObject playerObject)
    {
        Health.ChangeValue(-10);
        Animator.SetTrigger("Hurt");
    }

    internal abstract void StartOverride();
    internal abstract void UpdateOverride();
    internal abstract void PlayerIsInRange(GameObject gameObject);
    

    private void OnTriggerStay2D(Collider2D other)
    {
        var playerAttack = other.gameObject.CompareTag("PlayerAttack");
        if (playerAttack)
            YouGotHurt(other.gameObject);      
    }
}
