using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryScript : MonoBehaviour {

	SpriteRenderer sr;

	void Start () {
		sr = GetComponent<SpriteRenderer>();
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.CompareTag ("Player")) {
			// Set the interactNPC field
			//Debug.Log("Toucuhed");
			sr.enabled = false;
		}
	}

	void OnTriggerExit2D(Collider2D coll) {
	}
}
