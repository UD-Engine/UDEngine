using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBuiltinPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D collider) {
		collider.GetComponent<SpriteRenderer> ().color = Color.red;
	}

	void OnTriggerExit2D(Collider2D collider) {
		collider.GetComponent<SpriteRenderer> ().color = Color.white;
	}
}
