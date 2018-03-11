using UnityEngine;

public class SpearGoblin : Goblin
{
    public float moveSpeed = 5;
    private bool _isAttacking;


    // Update is called once per frame
    internal override void UpdateOverride()
    {
        float targetVelocityX = moveSpeed;
        //velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (movement.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
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
