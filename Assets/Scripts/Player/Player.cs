using System;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Animator animator;
    private Health Health { get; set; }
    Movement movement;

    public float jumpHeight = 4;
    public float timeToJumpApex = .4f;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    float moveSpeed = 6;

    float gravity;
    float jumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;
    private bool facingRight;

    internal Sword Weapon { get; private set; }

    void Awake()
    {
        animator = GetComponent<Animator>();
        Health = GetComponent<Health>();
        Health.CurrentValue = 3;
    }

    // Use this for initialization
    void Start()
    {
        movement = GetComponent<Movement>();

        gravity = -(2 * jumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        jumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        print("Gravity: " + gravity + "  Jump Velocity: " + jumpVelocity);
    }
    
    private void FixedUpdate()
    {
        if (movement.collisions.above || movement.collisions.below)
            velocity.y = 0;

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        if (Input.GetKeyDown(KeyCode.Space) && movement.collisions.below)
        {
            velocity.y = jumpVelocity;
            animator.SetBool("IsJumping", true);
        }
        if (movement.collisions.below && velocity.y == 0)
            animator.SetBool("IsJumping", false);

        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (movement.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        movement.Move(velocity * Time.deltaTime);

        if (velocity.x < 0.0f && facingRight == false)
            FlipPlayer();
        else if (velocity.x > 0.0f && facingRight)
            FlipPlayer();

        bool isWalking = input.x != 0.0f;
        animator.SetBool("IsWalking", isWalking);

        if (Input.GetKeyDown(KeyCode.LeftControl))
            animator.SetTrigger("Attack");
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("sword"))
        {
            other.gameObject.SetActive(false);
            animator.SetTrigger("GotSword");
            animator.runtimeAnimatorController = (RuntimeAnimatorController)Resources.Load("player_sword");
            Weapon = new Sword();
        }
        if (other.gameObject.CompareTag("EnemyAttack"))
        {
            this.Health.ChangeHealth(-.25f);
            Destroy(other.gameObject);
        }
    }

    private void FlipPlayer()
    {
        facingRight = !facingRight;
        Vector2 localScale = gameObject.transform.localScale;
        localScale.x *= -1;
        transform.localScale = localScale;
    }

}
