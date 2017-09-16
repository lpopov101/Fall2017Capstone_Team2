using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkPath : MonoBehaviour {

	public GameObject position1; // Target position 1
	public GameObject position2; // Target position 2
	public float movementSpeed = 1.0f; // How fast the creature moves
	public float bufferDistance = 0.5f; // How close the creature has to be to the target position to trigger idle
	public float idleTime; // The number of seconds the creature should be idle for
	public float idleMovementReducer = 0.8f; // The multiplier applied to the x velocity when in the idle position

	private Rigidbody2D rigidBody;
	private Animator animator;
	private bool pathInverted;
	private bool idle;

	void Start() {
		rigidBody = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		pathInverted = false;
		idle = false;
	}

	void FixedUpdate() {
		if(idle) {
			// Remove inertia from existing velocity if idle, but smoothly
			// Only noticeable at higher speeds
			rigidBody.velocity = new Vector2(rigidBody.velocity.x * idleMovementReducer, rigidBody.velocity.y);
			return;
		}

		// Pick which position to target according to the value of pathInverted
		Vector3 targetPos = pathInverted ? position1.transform.position : position2.transform.position;
		Vector3 offset = targetPos - transform.position, offsetX = new Vector3(offset.x, 0, 0);

		if(offset.sqrMagnitude > bufferDistance*bufferDistance) {
			// If too far away from the target position, move towards it
			Vector3 dir = offsetX.normalized;

			// Interpolate the velocity so it is smoother in case it is hit with opposing forces
			float smoothX = Mathf.Lerp(rigidBody.velocity.x, dir.x * movementSpeed, 1.0f);

			rigidBody.velocity = new Vector2(smoothX, rigidBody.velocity.y);
		} else {
			// If close enough to the target position, start the idle coroutine
			StartCoroutine(Idle());
		}
	}

	IEnumerator Idle() {
		idle = true;
		yield return new WaitForSeconds(idleTime);
		pathInverted = !pathInverted;
		if(animator) {
			// Update animator direction boolean if it exists
			animator.SetBool("walkingRight", !pathInverted);
		}
		idle = false;
	}
}
