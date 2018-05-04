using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHit : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

    }

    void GotHit(GameObject collision) {
        FindObjectOfType<SoundManager>().Play("EnemyDeath");
        Destroy(transform.parent.gameObject);
    }
}
