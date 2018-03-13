using System;
using UnityEngine;

public class SpearGoblin : Goblin
{
    public float moveSpeed = 5;
    float accelerationTimeGrounded = .1f;
    Vector3 velocity;
    Movement movement;
    float velocityXSmoothing;
    bool isIddle;
    private bool facingRight;
    private bool _isBreaking;
    private float breakingSpeedInterpolation = 0.0f;

    internal override void StartOverride()
    {
        isIddle = true;
        movement = GetComponent<Movement>();
    }

    internal override void UpdateOverride()
    {
    }

    internal override void PlayerIsInRange(GameObject playerObject)
    {
        if (_isBreaking)
        {
            if (velocity.x <= 0.5 && facingRight)
                Flip();
            if (velocity.x >= -0.5 && !facingRight)
                Flip();
            else
                SlowlyFlipTowardPlayer(playerObject);
        }
        else if (isIddle)
        {
            Animator.SetBool("PlayerIsInRange", true);
            MoveTowardPlayer(playerObject);
        }

    }

    private void SlowlyFlipTowardPlayer(GameObject playerObject)
    {
        float playerDirection = Mathf.Sign(playerObject.transform.position.x - transform.position.x);

        if (playerDirection == 1 && !facingRight)
            Break();
        else if (playerDirection == -1 && facingRight)
            Break();

        velocity.x = Mathf.Lerp(velocity.x, 0, breakingSpeedInterpolation); //Mathf.SmoothDamp(velocity.x, 0, ref velocityXSmoothing, 0.1f);
        breakingSpeedInterpolation += 0.5f * Time.deltaTime;
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
        _isBreaking = true;
        Animator.SetBool("IsBreaking",true);
    }

    private void Flip()
    {
        Vector2 localScale = gameObject.transform.localScale;
        Animator.SetBool("IsBreaking", false);
        localScale.x *= -1;
        transform.localScale = localScale;
        _isBreaking = false;
        facingRight = !facingRight;
        breakingSpeedInterpolation = 0.0f;
    }
}
