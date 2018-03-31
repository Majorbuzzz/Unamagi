using System.Collections;
using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    private MonsterHealth Health;
    internal Animator Animator;
    internal readonly Immunity Immunity;

    protected Monster()
    {
        Immunity = new Immunity();
    }

    void Start()
    {
        Animator = GetComponent<Animator>();
        Health = GetComponentInChildren<MonsterHealth>();
        StartOverride();
    }

    internal virtual void StartOverride() { }
    internal abstract void UpdateOverride();
    internal abstract void Die();
    internal abstract void PlayerIsInRange(GameObject playerObject);

    internal virtual void YouGotHurt(GameObject playerObject)
    {
        if (Immunity.NotImmune)
        {
            Immunity.Start(1);
            Health.ChangeValue(-3);
            Animator.SetTrigger("Hurt");
        }
    }

    void Update()
    {
        UpdateOverride();
        if (Health.IsDead)
            StartCoroutine("Dying");          
    }
    

    private IEnumerator Dying()
    {
        Animator.SetTrigger("Dying");
        Die();
        yield return new WaitForSeconds(1f);
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        Destroy(gameObject);
    }
}
