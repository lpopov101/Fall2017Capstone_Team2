using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonBehavior : MonoBehaviour {

	public float extensionLength;
	public float movementTime;
	public float idleTime;
	public bool startMovingUp = true;
	public GameObject bottomPart;

	private float startingHeight;
	private float topHeight;
	private bool movingUp;
	private bool idle;
	private float startMovingTime;
	private float idleStartTime;
	private bool hidingBottom;

	void Start () {
		startingHeight = transform.position.y;
		topHeight = startingHeight + extensionLength;
		movingUp = true;
		idle = false;
		startMovingTime = Time.time;
		idleStartTime = 0;
		hidingBottom = true;

		if(!startMovingUp) {
			movingUp = false;
			hidingBottom = false;
			bottomPart.SetActive(true);
			Vector3 vec = transform.position;
			vec.y = topHeight;
			transform.position = vec;
		}
	}

	void Update () {
		float time = Time.time;

		if(idle) {
			if(time > idleStartTime + idleTime) {
				idle = false;
				startMovingTime = Time.time;
			} else {
				return;
			}
		}

		if(movingUp) {
			if(transform.position.y >= topHeight) {
				movingUp = false;
				SetIdle();
			} else {
				float value = (time - startMovingTime) / movementTime;

				// Smooth interpolate the movement
				Vector3 vec = transform.position;
				vec.y = Mathf.SmoothStep(startingHeight, topHeight, value);
				transform.position = vec;

				// Show the bottom if moved past half the distance
				if(hidingBottom && value > 0.5f) {
					hidingBottom = false;
					bottomPart.SetActive(true);
				}
			}
		} else {
			if(transform.position.y <= startingHeight) {
				movingUp = true;
				SetIdle();
			} else {
				float value = (time - startMovingTime) / movementTime;

				// Smooth interpolate the movement
				Vector3 vec = transform.position;
				vec.y = Mathf.SmoothStep(topHeight, startingHeight, value);
				transform.position = vec;

				// Hide the bottom if moved past half the distance
				if(!hidingBottom && value > 0.5f) {
					hidingBottom = true;
					bottomPart.SetActive(false);
				}
			}
		}
	}

	private void SetIdle() {
		idle = true;
		idleStartTime = Time.time;
	}
}
