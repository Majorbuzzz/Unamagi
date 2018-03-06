using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    PlayerMovement movement;

    internal Sword Weapon { get; private set; }

    void Awake()
    {
        animator = GetComponent<Animator>();
        movement = GetComponent<PlayerMovement>();
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        movement.Move(input, animator);
        
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
    }

}
