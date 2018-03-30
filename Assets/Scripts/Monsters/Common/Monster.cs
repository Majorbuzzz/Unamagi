using UnityEngine;

public abstract class Monster : MonoBehaviour
{
    private MonsterHealth Health;
    internal Animator Animator;

    void Start()
    {
        Animator = GetComponent<Animator>();
        Health = GetComponentInChildren<MonsterHealth>();
        StartOverride();
    }

    internal virtual void StartOverride() { }
    internal abstract void UpdateOverride();
    internal abstract void PlayerIsInRange(GameObject playerObject);

    internal virtual void YouGotHurt(GameObject playerObject)
    {
        Health.ChangeValue(-10);
        Animator.SetTrigger("Hurt");
    }
    
    void Update()
    {
        UpdateOverride();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        var playerIsInRange = other.gameObject.CompareTag("Player");

        if (playerIsInRange)
            PlayerIsInRange(other.gameObject);


    }
}
