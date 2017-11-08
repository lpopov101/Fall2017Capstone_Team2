using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

	void OnCollisionEnter2D(Collision2D coll) {
		if(coll.gameObject.CompareTag("Player")) {
			coll.gameObject.transform.parent = transform;
		}
	}

	void OnCollisionExit2D(Collision2D coll) {
		if(coll.gameObject.CompareTag("Player")) {
			coll.gameObject.transform.parent = null;
		}
	}
}
