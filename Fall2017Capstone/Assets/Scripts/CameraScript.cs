using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	public Transform target;
	public Vector2 cameraOffset;

	void Update() {
		float x = target.position.x + cameraOffset.x;
		float y = target.position.y + cameraOffset.y;
		transform.position = new Vector3 (x, y, -10);
	}
}
