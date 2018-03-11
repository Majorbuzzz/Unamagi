using UnityEngine;

public class SpearGoblin : Goblin
{
    public float moveSpeed = 5;
    float accelerationTimeGrounded = .1f;
    private bool _isAttacking;
    Vector3 velocity;
    Movement movement;
    float velocityXSmoothing;

    internal override void StartOverride()
    {
        movement = GetComponent<Movement>();
    }

    internal override void UpdateOverride()
    {
        float targetVelocityX = moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTimeGrounded);
        movement.Move(velocity * Time.deltaTime);
    }

    internal override void PlayerIsInRange()
    {
        if (!_isAttacking)
        {
            Animator.SetBool("PlayerIsInRange", true);
            _isAttacking = true;
        }
    }
}
