using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {

	public float parallaxSpeed;

	private float lastCameraX;
	private Transform cameraTransform;

	// Use this for initialization
	void Start () {
		cameraTransform = Camera.main.transform;
		lastCameraX = cameraTransform.position.x;
	}
	
	// Update is called once per frame
	void Update () {
		float deltaX = cameraTransform.position.x - lastCameraX;
		transform.position += Vector3.right * (deltaX * parallaxSpeed);
		lastCameraX = cameraTransform.position.x;
	}
}
