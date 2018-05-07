using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelEnd : MonoBehaviour {

    Rigidbody2D rb;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(transform.forward, 1f);
	}

    private void OnTriggerEnter2D(Collider2D collision) {
        print("Congratulations!");
        FindObjectOfType<SoundManager>().Play("LevelEnd");
        Destroy(gameObject);
    }
}
