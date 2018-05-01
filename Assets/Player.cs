using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    Rigidbody2D rb;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;
    public bool isGrounded = false;

    private Animator animator;
    private SpriteRenderer sprite;
    [SerializeField] float runAnimationSpeed = 4;
    
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }
	
	void Update () {
        MovementControl();
        JumpControl();
        setAnimations();
    }

    private void setAnimations() {
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

        // Check if we are facing left or right, change the sprite direction accordingly.
        if(rb.velocity.x >= 0) {    // Moving Right
            sprite.flipX = false;   // Face Right
        }
        else {                      // Moving Left
            sprite.flipX = true;    // Face Left
        }

        // Increase the animation speed when we are running.
        if (Mathf.Abs(rb.velocity.x) >= 10) {
            animator.speed = runAnimationSpeed;
        }
        else {
            animator.speed = 1;
        }
        animator.SetFloat("vSpeed", rb.velocity.y);
    }

    private void MovementControl() {
        float h = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(h * moveSpeed, rb.velocity.y);
        print(rb.velocity.x);
    }

    private void JumpControl() {
        if (isGrounded && Input.GetButtonDown("Jump")) {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
            isGrounded = true;
    }
}
