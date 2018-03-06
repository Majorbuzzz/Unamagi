using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goblin : MonoBehaviour
{

    private CircleCollider2D _perceptionCollider;
    private Animator _animator;
    public Rigidbody2D _slingRockPrefab;
    private bool _isSlingRecharged;
    private float timer = 1f;

    // Use this for initialization
    void Start()
    {

        _perceptionCollider = GetComponent<CircleCollider2D>();
        _animator = GetComponent<Animator>();
        _isSlingRecharged = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isSlingRecharged)
            timer -= Time.deltaTime;
        if (timer <= 0)
        {
            _isSlingRecharged = true;
            timer = 5F;
        }


    }

    private void OnTriggerStay2D(Collider2D other)
    {
        var playerIsInRange = other.gameObject.CompareTag("Player");
        _animator.SetBool("PlayerIsInRange", playerIsInRange);

        if (playerIsInRange && _isSlingRecharged)
        {
            var slingLocation = new Vector3(transform.localPosition.x-0.22f, transform.localPosition.y - 0.22f, 0);
            Rigidbody2D slingRock = Instantiate(_slingRockPrefab, slingLocation, Quaternion.identity);
            slingRock.velocity = new Vector2(-6f, 7);
            _isSlingRecharged = false;
        }


    }
}
