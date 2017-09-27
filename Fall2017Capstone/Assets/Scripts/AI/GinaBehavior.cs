using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GinaBehavior : MonoBehaviour {

	public float waitTime;
	public float movementSpeed;
	public float distanceBeforeDestroying;

	private Animator animator;
	private bool walking;
	private Vector3 initialPos;

	void Start () {
		animator = GetComponent<Animator>();
		walking = false;
		initialPos = transform.position;

		StartCoroutine(WaitAndWalk());	
	}

	void Update () {
		if(walking) {
			transform.Translate(Vector3.right * movementSpeed * Time.deltaTime);
			if(transform.position.x > initialPos.x + distanceBeforeDestroying) {
				Destroy(gameObject);
			}
		}
	}

	IEnumerator WaitAndWalk() {
		yield return new WaitForSeconds(waitTime);

		animator.SetBool("walking", true);
		yield return new WaitForSeconds(0.15f); // Fixes weird animation delay
		walking = true;
	}
}
