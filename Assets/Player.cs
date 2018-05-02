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

    // Ability stuff
    [SerializeField] bool dashing = false;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashDistance;
    [SerializeField] float dashAnimationSpeed;

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

        setSpriteDirection();

        // Set animation speed to something specific if we're dashing.
        if (dashing) {
            animator.speed = dashAnimationSpeed;
        }
        else if(Mathf.Abs(rb.velocity.x) >= 10) { // Increase the animation speed when we are running.
            animator.speed = runAnimationSpeed;
        }
        else {
            animator.speed = 1;
        }
        // Increase the animation speed when we are running.
        animator.SetFloat("vSpeed", rb.velocity.y);
    }

    private void setSpriteDirection() {
        // Check if we are facing left or right, change the sprite direction accordingly.
        if (rb.velocity.x >= 0) {    // Moving Right
            sprite.flipX = false;   // Face Right
        }
        else {                      // Moving Left
            sprite.flipX = true;    // Face Left
        }
    }

    private void MovementControl() {
        float h = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown("g") && !dashing) { // We dash if we aren't dashing and hit the "g" key
            StartCoroutine(doDash(h, dashDistance));
        }
        if (!dashing) { // We're not dashing, so move normal.
            rb.velocity = new Vector2(h * moveSpeed, rb.velocity.y);
        }
    }

    IEnumerator doDash(float h, float waitTime) {
        dashing = true;
        isGrounded = true; // Set this to true so we can dash in the air.
        animator.SetBool("Dashing", dashing);
        rb.velocity = new Vector2(h * moveSpeed * dashSpeed, rb.velocity.y);
        yield return new WaitForSeconds(waitTime);
        dashing = false;
        animator.SetBool("Dashing", dashing);
    }

    private void JumpControl() {
        if (isGrounded && Input.GetButtonDown("Jump")) {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            isGrounded = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        // Did we hit an enemy's head?
        if(collision.transform.tag == "Enemy_Hit") {
            Destroy(collision.transform.parent.gameObject);
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed / 2);
        }
        else if (collision.transform.tag == "Enemy_Danger") {
            print("Uh oh! You just got hit by the enemy!");
        }
        else { // Probably just on a platform.
            isGrounded = true;
        }
    }
}
