using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class PlayerMovement : MonoBehaviour
{
    public int speed;
    public float jumpHeight;
    public float timeToJumpApex;

    float gravity ;
    private bool facingRight = false;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    private float jumpVelocity;
    private float velocityXSmoothing;
    PlayerCollision playerCollision;
    private Vector3 velocity;

    public bool IsJumping { get; set; }

    void Start()
    {
        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        playerCollision = GetComponent<PlayerCollision>();
    }

    public void Move(Vector2 input, Animator playerAnimator)
    {
        if (playerCollision.IsGrounded || playerCollision.IsTouchingTheCelling)
            velocity.y = 0;

        Jump(ref velocity, playerAnimator);

        float targetVelocityX = input.x * speed;
        var maxSpeed = playerCollision.IsGrounded ? accelerationTimeGrounded : accelerationTimeAirborne;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, maxSpeed);

        if (velocity.x < 0.0f && facingRight == false)
            FlipPlayer();
        else if (velocity.x > 0.0f && facingRight)
            FlipPlayer();

        bool isWalking = input.x != 0.0f;
        playerAnimator.SetBool("IsWalking", isWalking);

        velocity = playerCollision.AffectCollisionToVelocity(velocity * Time.deltaTime);

        transform.Translate(velocity);
    }

    private void Jump(ref Vector3 velocity, Animator playerAnimator)
    {
        IsJumping = Input.GetKeyDown(KeyCode.Space) && playerCollision.IsGrounded;

        if (IsJumping)
            playerAnimator.SetBool("IsJumping", true);
        if (playerCollision.IsGrounded && velocity.y == 0)
            playerAnimator.SetBool("IsJumping", false);

        if (IsJumping)
            velocity.y = jumpVelocity;

        velocity.y += gravity * Time.deltaTime;
    }

    private void FlipPlayer()
    {
        facingRight = !facingRight;
        Vector2 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }
}