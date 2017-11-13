using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowdownBehavior : MonoBehaviour {

	public float slowdownMultiplier;

	private Rigidbody2D playerRb;

	void Start () {
		playerRb = null;
	}

	void Update () {
		if(playerRb) {
			Vector2 velocity = playerRb.velocity;
			velocity.x *= slowdownMultiplier;
			playerRb.velocity = velocity;
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if(coll.gameObject.CompareTag("Player")) {
			playerRb = coll.gameObject.GetComponent<Rigidbody2D>();
		}
	}

	void OnCollisionExit2D(Collision2D coll) {
		if(coll.gameObject.CompareTag("Player")) {
			playerRb = null;
		}
	}
}
