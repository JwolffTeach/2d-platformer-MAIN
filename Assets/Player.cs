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
    
	void Start () {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
	}
	
	void Update () {
        MovementControl();
        JumpControl();
        setAnimations();
    }

    private void setAnimations() {
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("Speed", rb.velocity.x);
        animator.SetFloat("vSpeed", rb.velocity.y);
    }

    private void MovementControl() {
        float h = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(h * moveSpeed, rb.velocity.y);
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
