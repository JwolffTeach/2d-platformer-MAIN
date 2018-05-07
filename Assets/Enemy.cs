using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

    enum enemyTypes {Winger, Spikey, Copter, Bouncy};
    [SerializeField] enemyTypes enemyType;
    [SerializeField] [Range(0f, 10f)] float amplitude;
    [SerializeField] [Range(0f, 10f)] float frequency;

    // Copter Properties
    bool triggerJump = false;
    [SerializeField]
    bool grounded;
    [SerializeField][Range(0f, 10f)] float jumpTime;
    [SerializeField][Range(0f, 20f)] float jumpStrength;


    // Use this for initialization
    void Start () {
        if (enemyType == enemyTypes.Copter) {
            GetComponent<Rigidbody2D>().AddForce(transform.up * 10, ForceMode2D.Impulse);
        }
    }

    // Update is called once per frame
    void Update() {
        if (enemyType == enemyTypes.Winger) {
            // Do Winger movement
            WingerMovement();
        }
        if(enemyType == enemyTypes.Copter) {
            GetComponent<Animator>().SetBool("Grounded", grounded);
            GetComponent<Animator>().SetFloat("ySpeed", GetComponent<Rigidbody2D>().velocity.y);
        }
    }

    void WingerMovement() {
        // The Winger moves up and down in a sine wave pattern.
        Vector3 newPos = transform.position;
        newPos.y += amplitude * Mathf.Sin(frequency * Time.time) * Time.deltaTime;
        print(newPos);
        transform.position = newPos;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if(enemyType == enemyTypes.Copter) {
            grounded = true;
            if (!triggerJump) {
                StartCoroutine(delayJump());
            }
        }
    }

    IEnumerator delayJump() {
        yield return new WaitForSeconds(jumpTime);
        GetComponent<Rigidbody2D>().AddForce(transform.up * jumpStrength, ForceMode2D.Impulse);
        triggerJump = false;
        grounded = false;
    }
}
