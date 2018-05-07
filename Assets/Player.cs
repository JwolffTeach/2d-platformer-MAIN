using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour {

    // Player stats
    int hp = 3;

    // Movement Stuff
    Rigidbody2D rb;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;
    public bool isGrounded = false;
    public bool gotHit = false;
    public float recoveryTime;

    // Animator Stuff
    private Animator animator;
    private SpriteRenderer sprite;
    [SerializeField] float runAnimationSpeed = 4;

    // Ability stuff
    [SerializeField] bool dashing = false;
    [SerializeField] float dashSpeed;
    [SerializeField] float dashDistance;
    [SerializeField] float dashAnimationSpeed;

    // UI things
    [SerializeField] Text hpText;

    void Start () {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }
	
	void Update () {
        MovementControl();
        JumpControl();
        SetAnimations();
        SetUI();
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        string otherTag = collision.collider.tag;
        string thisTag = collision.otherCollider.tag;
        if(otherTag.Contains("Platform")){ // Did we hit a platform?
            if (otherTag.Contains("Top")) {
                isGrounded = true;
            }
        }
        else if (otherTag.Contains("Enemy")){ // Did we hit an enemy?
            if (otherTag.Contains("Hit")) { // Did we land on top of it?
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
                collision.collider.SendMessage("GotHit", this.gameObject);
            }
            else if (otherTag.Contains("Danger")) { // It probably hit us.
                BodyCollision();
            }
        }
        else {
            Debug.LogError("Unable to parse collider tags. Collision: " + collision.otherCollider.tag + " and " + collision.collider.tag);
        }
    }

    private void BodyCollision() {
        rb.AddRelativeForce(Vector2.left * 1000, ForceMode2D.Force);
        StartCoroutine("HitRecovery");
        print("Uh oh! You just got hit by the enemy!");
    }

    private void SetAnimations() {
        animator.SetBool("isGrounded", isGrounded);
        animator.SetFloat("Speed", Mathf.Abs(rb.velocity.x));

        SetSpriteDirection();

        // Set animation speed to something specific if we're dashing.
        if (dashing) {
            animator.speed = dashAnimationSpeed;
        }
        else if (Mathf.Abs(rb.velocity.x) >= moveSpeed) {
            animator.speed = runAnimationSpeed;
            if (isGrounded) {
                FindObjectOfType<SoundManager>().PlayLooping("PlayerRun");
            }
        }
        else {
            animator.speed = 1;
        }
    }

    private void SetSpriteDirection() {
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

        if (!gotHit) {
            if (Input.GetKeyDown("g") && !dashing) { // We dash if we aren't dashing and hit the "g" key
                StartCoroutine(DoDash(h, dashDistance));
            }
            if (!dashing) { // We're not dashing, so move normal.
                rb.velocity = new Vector2(h * moveSpeed, rb.velocity.y);
            }
        }
        else {
            MovementRecovery();
        }
    }

    private void MovementRecovery() {
        if (rb.velocity.x < 0) { // We're moving left from a bounce
            rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + 0.5f, -moveSpeed, 0), rb.velocity.y);
        }
        else if (rb.velocity.x > 0) {
            if (rb.velocity.x < 0) { // We're moving left from a bounce
                rb.velocity = new Vector2(Mathf.Clamp(rb.velocity.x + 0.5f, 0, moveSpeed), rb.velocity.y);
            }
        }
    }

    IEnumerator HitRecovery() {
        gotHit = true;
        hp -= 1;
        FindObjectOfType<SoundManager>().Play("PlayerDamage");
        yield return new WaitForSeconds(recoveryTime);
        gotHit = false;
    }

    IEnumerator DoDash(float h, float waitTime) {
        dashing = true;
        isGrounded = true; // Set this to true so we can dash in the air.
        rb.velocity = new Vector2(h * moveSpeed * dashSpeed, rb.velocity.y);
        animator.SetBool("Dashing", dashing);
        FindObjectOfType<SoundManager>().PlayLooping("PlayerDash");

        yield return new WaitForSeconds(waitTime);

        dashing = false;
        animator.SetBool("Dashing", dashing);
    }

    private void JumpControl() {
        if (isGrounded && Input.GetButtonDown("Jump")) {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            isGrounded = false;
            FindObjectOfType<SoundManager>().Play("PlayerJump");
        }
    }

    private void SetUI() {
        hpText.text = "Health: " + hp;
    }
}
