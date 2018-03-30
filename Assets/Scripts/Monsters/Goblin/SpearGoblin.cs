using System;
using UnityEngine;

public class SpearGoblin : Monster
{
    public Weapon Weapon;
    public float moveSpeed = 5;
    float accelerationTimeGrounded = .1f;
    Vector3 velocity;
    Movement movement;
    Collider2D spearCollider;
    float velocityXSmoothing;
    private bool facingRight;
    private float breakingSpeedInterpolation = 0.0f;
    private State _state;


    internal enum State
    {
        Idle = 0,
        Charging = 1,
        Breaking = 2,
        MeleeAttack = 3
    }

    internal override void StartOverride()
    {
        movement = GetComponentInChildren<Movement>();
        spearCollider = GetComponentInChildren<Collider2D>();
        Weapon.WeaponHit += () => SpearHit();
    }

    internal override void YouGotHurt(GameObject playerObject)
    {
        velocity.x = 0;
        base.YouGotHurt(playerObject);
    }

    private void SpearHit()
    {
        _state = State.Breaking;
    }

    internal override void UpdateOverride()
    { }

    internal override void PlayerIsInRange(GameObject playerObject)
    {
        if (_state == State.Breaking)
        {
            if (velocity.x <= 0.5 && facingRight)
                Flip();
            if (velocity.x >= -0.5 && !facingRight)
                Flip();
            else
                SlowlyFlipTowardPlayer(playerObject);
        }
        else if (_state == State.Idle)
        {
            Animator.SetBool("PlayerIsInRange", true);
            _state = State.Charging;
            spearCollider.enabled = true;
        }
        else if (_state == State.Charging)
            MoveTowardPlayer(playerObject);
        else if (_state == State.MeleeAttack)
            Animator.SetBool("PlayerIsInMeleeRange", true);
    }

    private void SlowlyFlipTowardPlayer(GameObject playerObject)
    {
        float playerDirection = Mathf.Sign(playerObject.transform.position.x - transform.position.x);

        if (playerDirection == 1 && !facingRight)
            Break();
        else if (playerDirection == -1 && facingRight)
            Break();

        velocity.x = Mathf.Lerp(velocity.x, 0, breakingSpeedInterpolation);
        breakingSpeedInterpolation += 0.1f * Time.deltaTime;
        movement.Move(velocity * Time.deltaTime);
    }

    private void MoveTowardPlayer(GameObject playerObject)
    {
        float playerDirection = Mathf.Sign(playerObject.transform.position.x - transform.position.x);

        if (playerDirection == 1 && !facingRight)
            Break();
        else if (playerDirection == -1 && facingRight)
            Break();

        float targetVelocityX = moveSpeed * playerDirection;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTimeGrounded);
        movement.Move(velocity * Time.deltaTime);
    }

    private void Break()
    {
        _state = State.Breaking;
        Animator.SetBool("IsBreaking", true);
    }

    private void Flip()
    {
        Vector2 localScale = gameObject.transform.localScale;
        Animator.SetBool("IsBreaking", false);
        localScale.x *= -1;
        transform.localScale = localScale;
        _state = State.Charging;
        facingRight = !facingRight;
        breakingSpeedInterpolation = 0.0f;
    }
}
