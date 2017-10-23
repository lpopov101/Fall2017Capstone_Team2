using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBehavior : MonoBehaviour {

	public string avoidTag;
	public bool destroyOnHit;
	public GameObject spawnAfterHit;
	public bool disappearAfterHit;
	public float disappearTime;

	private Rigidbody2D rigidBody;
	private Animator animator;
	private bool disappearing;

	void Awake () {
		rigidBody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		disappearing = false;
	}

	public void SetVelocity(Vector2 vec) {
		rigidBody.velocity = vec;
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if(coll.gameObject.CompareTag(avoidTag))
			return;

		if(spawnAfterHit) {
			Instantiate(spawnAfterHit, transform);
		}

		if(destroyOnHit)
			Destroy(gameObject);

		if(disappearAfterHit && animator) {
			disappearing = true;
			animator.SetBool("disappearing", true);
			Destroy(gameObject, disappearTime);
			rigidBody.bodyType = RigidbodyType2D.Static;
		}
	}
}
