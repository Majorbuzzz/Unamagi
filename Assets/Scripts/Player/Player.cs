using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public BoxCollider2D BodyCollider { get; private set; }
    public float jumpHeight = 4;
    public float timeToJumpApex = .4f;

    private Animator animator;
    private PlayerHealth Health { get; set; }
    Movement movement;
    float accelerationTimeAirborne = .2f;
    float accelerationTimeGrounded = .1f;
    float moveSpeed = 6;
    float gravity;
    float jumpVelocity;
    Vector3 velocity;
    float velocityXSmoothing;
    private bool facingRight;

    private SpriteRenderer mySpriteRenderer;
    private readonly Immunity immunity;

    public Player()
    {
        immunity = new Immunity();
    }

    internal Sword Weapon { get; private set; }

    void Awake()
    {
        animator = GetComponent<Animator>();
        Health = GetComponent<PlayerHealth>();
        BodyCollider = GetComponent<BoxCollider2D>();
        mySpriteRenderer = GetComponent<SpriteRenderer>();
        immunity.SpriteRenderer = mySpriteRenderer;
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

    private void Update()
    {
        immunity.DoYourThing();

        if (movement.collisions.above || movement.collisions.below)
            velocity.y = 0;

        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Jump();
        Move(input);
        Flip(input);    
        Attack();
    }

    private void Attack()
    {
        StartCoroutine("MeleeHandler");
    }

    private IEnumerator MeleeHandler()
    {
        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            animator.SetTrigger("Attack");
            GetComponentInChildren<CircleCollider2D>().enabled = true;
            yield return new WaitForSeconds(0.5f); 
            GetComponentInChildren<CircleCollider2D>().enabled = false;
        }
        yield return null;
    }

    private void Flip(Vector2 input)
    {
        if (input.x < 0.0f && facingRight == false)
            FlipPlayer();
        else if (input.x > 0.0f && facingRight)
            FlipPlayer();
    }

    private void Move(Vector2 input)
    {
        float targetVelocityX = input.x * moveSpeed;
        velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, (movement.collisions.below) ? accelerationTimeGrounded : accelerationTimeAirborne);
        velocity.y += gravity * Time.deltaTime;
        movement.Move(velocity * Time.deltaTime);

        bool isWalking = input.x != 0.0f;
        animator.SetBool("IsWalking", isWalking);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && movement.collisions.below)
        {
            velocity.y = jumpVelocity;
            animator.SetBool("IsJumping", true);
        }
        if (movement.collisions.below && velocity.y == 0)
            animator.SetBool("IsJumping", false);
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

        if (other.gameObject.CompareTag("Enemy") && immunity.NotImmune)
        {
            Health.ChangeHealth(-.25f);
            animator.SetTrigger("Hurt");
            var direction = facingRight ? 1 : -1;
            velocity.x = direction * 8f;
            movement.Move(velocity * Time.deltaTime);
            immunity.Start(2);

            Weapon weapon = other.GetComponent<Weapon>();
            if (weapon != null)
                weapon.TriggerWeaponHit();
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
