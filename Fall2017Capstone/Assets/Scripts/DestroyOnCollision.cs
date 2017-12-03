using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnCollision : MonoBehaviour {

	public bool hasDestroyAnimation = false;
	public float destroyAnimDuration;
	public bool freezeMotion = false;

	private Animator animator;
	private Rigidbody2D rigidBody;

	void Start() {
		animator = hasDestroyAnimation ? GetComponent<Animator>() : null;
		rigidBody = GetComponent<Rigidbody2D>();
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if(freezeMotion && rigidBody) {
			rigidBody.bodyType = RigidbodyType2D.Static;
		}

		if(!hasDestroyAnimation) {
			Destroy(gameObject);
		} else {
			animator.SetBool("disappearing", true);
			Destroy(gameObject, destroyAnimDuration);
		}
	}
}
