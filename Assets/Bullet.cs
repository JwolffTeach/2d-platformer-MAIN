using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {

    // Bullet Stats
    [SerializeField] float speed;
    float carrotOffset;
    Rigidbody2D rb;


	// Use this for initialization
	void Start () {
        carrotOffset = transform.rotation.z;
        transform.localRotation = Quaternion.Euler(transform.rotation.x, transform.rotation.y, 45f);
        rb = GetComponent<Rigidbody2D>();
        rb.AddRelativeForce(Vector3.right  * speed, ForceMode2D.Impulse);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
