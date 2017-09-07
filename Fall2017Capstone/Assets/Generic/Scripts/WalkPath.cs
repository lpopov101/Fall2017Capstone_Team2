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

	Rigidbody2D rb;
	bool pathInverted;
	bool idle;

	void Start() {
		rb = GetComponent<Rigidbody2D>();
		pathInverted = false;
		idle = false;
	}

	void FixedUpdate() {
		if(idle) {
			// Remove inertia from existing velocity if idle, but smoothly
			// Only noticeable at higher speeds
			rb.velocity = new Vector2(rb.velocity.x * idleMovementReducer, rb.velocity.y);
			return;
		}

		// Pick which position to target according to the value of pathInverted
		Vector3 targetPos = pathInverted ? position1.transform.position : position2.transform.position;
		Vector3 offset = targetPos - transform.position, offsetX = new Vector3(offset.x, 0, 0);

		if(offset.sqrMagnitude > bufferDistance*bufferDistance) {
			// If too far away from the target position, move towards it
			Vector3 dir = offsetX.normalized;

			// Interpolate the velocity so it is smoother in case it is hit with opposing forces
			float smoothX = Mathf.Lerp(rb.velocity.x, dir.x * movementSpeed, 1.0f);

			rb.velocity = new Vector2(smoothX, rb.velocity.y);
		} else {
			// If close enough to the target position, start the idle coroutine
			StartCoroutine(Idle());
		}
	}

	IEnumerator Idle() {
		idle = true;
		yield return new WaitForSeconds(idleTime);
		pathInverted = !pathInverted;
		idle = false;
	}
}
