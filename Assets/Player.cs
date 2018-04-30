using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

    Rigidbody2D rb;
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;
    bool isGrounded = false;
    
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	void Update () {
        MovementControl();
        JumpControl();
    }

    private void MovementControl() {
        float h = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(h * moveSpeed, rb.velocity.y);
    }

    private void JumpControl() {
        if (isGrounded && Input.GetButtonDown("Jump")) {
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
            isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision) {
            isGrounded = false;
        
    }
}
