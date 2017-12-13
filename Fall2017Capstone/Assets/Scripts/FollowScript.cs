using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowScript : MonoBehaviour {

	public bool following;
	public float smoothTime;
	public Transform target;
	public Vector2 offset;

	private Vector2 smoothVelocity;

	void Start () {
		
	}

	void LateUpdate () {
		if(following) {
			Vector3 position = transform.position;
			float targetX = target.position.x + offset.x;
			float targetY = target.position.y + offset.y;
			position.x = Mathf.SmoothDamp(position.x, targetX, ref smoothVelocity.x, smoothTime);
			position.y = Mathf.SmoothDamp(position.y, targetY, ref smoothVelocity.y, smoothTime);
			transform.position = position;
		}
	}
}
