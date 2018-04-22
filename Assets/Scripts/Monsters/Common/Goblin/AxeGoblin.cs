using UnityEngine;

public class AxeGoblin : Monster
{
    public Weapon Weapon;
    public float moveSpeed = 5;
    float accelerationTimeGrounded = .1f;
    Vector3 velocity;
    Movement movement;
    Collider2D spearCollider;
    float velocityXSmoothing;
    private bool facingRight;
    private State _state;

    internal enum State
    {
        Idle = 0,
        Charging = 1,
        MeleeAttack = 3,
        Die = 4
    }

    internal override void StartOverride()
    {
        movement = GetComponentInChildren<Movement>();
        spearCollider = GetComponentInChildren<Collider2D>();
    }

    internal override void YouGotHurt(GameObject playerObject)
    {
        _state = State.Idle;
        base.YouGotHurt(playerObject);
    }

    internal override void PlayerIsInRange(GameObject playerObject)
    {
        float distance = Vector2.Distance(playerObject.transform.position, transform.position);

        if (distance <= Weapon.Range)
        {
            if (_state != State.MeleeAttack)
            {
                _state = State.MeleeAttack;
                Animator.SetBool("PlayerIsInRange", false);
                Animator.SetBool("PlayerIsInMeleeRange", true);
            }
        }
        else
        {
            if (_state != State.Charging)
            {
                _state = State.Charging;
                Animator.SetBool("PlayerIsInMeleeRange", false);
                Animator.SetBool("PlayerIsInRange", true);
            }
        }

        if (_state == State.Charging)
            MoveTowardPlayer(playerObject);

    }

    private void MoveTowardPlayer(GameObject playerObject)
    {
        float playerDirection = Mathf.Sign(playerObject.transform.position.x - transform.position.x);

        if (playerDirection == 1 && !facingRight)
            Flip();
        else if (playerDirection == -1 && facingRight)
            Flip();

        float targetVelocityX = moveSpeed * playerDirection;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTimeGrounded);
        movement.Move(velocity * Time.deltaTime);
    }


    private void Flip()
    {
        Vector2 localScale = gameObject.transform.localScale;
        Animator.SetBool("IsBreaking", false);
        localScale.x *= -1;
        transform.localScale = localScale;
        _state = State.Charging;
        facingRight = !facingRight;
    }

    internal override void Die()
    {
        _state = State.Die;
        velocity.x = 0;
    }
}
