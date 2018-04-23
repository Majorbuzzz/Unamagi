using UnityEngine;

public abstract class MeleeGoblin : Monster
{
    public Weapon Weapon;
    public float moveSpeed = 5;
    protected Vector3 Velocity;
    protected Movement Movement;
    float accelerationTimeGrounded = .1f;
    float velocityXSmoothing;
    protected bool FacingRight;
    protected State CurrentState;

    protected enum State
    {
        Idle = 0,
        Charging = 1,
        Breaking = 2,
        MeleeAttack = 3,
        Die = 4
    }

    internal override void StartOverride()
    {
        Movement = GetComponentInChildren<Movement>();
    }

    internal override void YouGotHurt(GameObject playerObject)
    {
        CurrentState = State.Idle;
        base.YouGotHurt(playerObject);
    }

    internal override void PlayerIsInRange(GameObject playerObject)
    {
        float distance = Vector2.Distance(playerObject.transform.position, transform.position);

        if (distance <= Weapon.Range )
        {
            if (CurrentState != State.MeleeAttack && CurrentState != State.Breaking)
            {
                CurrentState = State.MeleeAttack;
                Animator.SetBool("PlayerIsInRange", false);
                Animator.SetBool("PlayerIsInMeleeRange", true);
            }
        }
        else
        {
            if (CurrentState != State.Charging && CurrentState != State.Breaking)
            {
                CurrentState = State.Charging;
                Animator.SetBool("PlayerIsInMeleeRange", false);
                Animator.SetBool("PlayerIsInRange", true);
            }
        }

        if (CurrentState == State.Charging)
            MoveTowardPlayer(playerObject);
    }

    private void MoveTowardPlayer(GameObject playerObject)
    {
        float playerDirection = Mathf.Sign(playerObject.transform.position.x - transform.position.x);

        if (playerDirection == 1 && !FacingRight)
            Flip();
        else if (playerDirection == -1 && FacingRight)
            Flip();

        float targetVelocityX = moveSpeed * playerDirection;
        Velocity.x = Mathf.SmoothDamp(Velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTimeGrounded);
        Movement.Move(Velocity * Time.deltaTime);
    }


    protected virtual void Flip()
    {
        Vector2 localScale = gameObject.transform.localScale;
        Animator.SetBool("IsBreaking", false);
        localScale.x *= -1;
        transform.localScale = localScale;
        CurrentState = State.Charging;
        FacingRight = !FacingRight;
    }

    internal override void Die()
    {
        CurrentState = State.Die;
        Velocity.x = 0;
    }
}
