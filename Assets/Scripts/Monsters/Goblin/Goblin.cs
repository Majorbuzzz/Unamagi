using System;
using UnityEngine;

public abstract class Goblin : MonoBehaviour
{    
    private CircleCollider2D _perceptionCollider;
    internal Animator Animator;

    // Use this for initialization
    void Start()
    {
        _perceptionCollider = GetComponent<CircleCollider2D>();
        Animator = GetComponent<Animator>();
        StartOverride();
    }

    internal abstract void StartOverride();
    internal abstract void UpdateOverride();
    internal abstract void PlayerIsInRange(GameObject playerObject);

    // Update is called once per frame
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
