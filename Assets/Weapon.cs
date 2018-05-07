using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

    [SerializeField] GameObject bullet;

    // Weapon Stats
    [SerializeField] float cooldown;
    [SerializeField] float cooldownTimer;

	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        if (cooldownTimer <= 0) {
            if (Input.GetButtonDown("Fire1")) {
                GameObject newBullet = Instantiate(bullet, transform.position, bullet.transform.rotation);
                cooldownTimer = cooldown;
            }
        }
        else {
            cooldownTimer -= Time.deltaTime;
        }
	}
}
