using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	public float smoothTime; // The smaller, the faster
	public Transform target;
	public Vector2 defaultCameraOffset;

	private Camera cam;
	private Vector2 cameraOffset;

	private float initialCameraSize;
	private Vector2 initialCameraOffset;
	private float targetCameraSize;
	private Vector2 targetCameraOffset;
	private float cameraSizeVelocity;
	private Vector2 cameraOffsetVelocity;

	void Start() {
		cam = GetComponent<Camera>();

		cameraOffset.x = target.position.x + defaultCameraOffset.x;
		cameraOffset.y = target.position.y + defaultCameraOffset.y;

		initialCameraSize = targetCameraSize = cam.orthographicSize;
		initialCameraOffset = targetCameraOffset = defaultCameraOffset;
	}

	void LateUpdate() {
		// Interpolate the camera size
		cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, targetCameraSize, ref cameraSizeVelocity, smoothTime);

		// Interpolate the camera offset
		float targetX = target.position.x + targetCameraOffset.x;
		float targetY = target.position.y + targetCameraOffset.y;
		cameraOffset.x = Mathf.SmoothDamp(cameraOffset.x, targetX, ref cameraOffsetVelocity.x, smoothTime);
		cameraOffset.y = Mathf.SmoothDamp(cameraOffset.y, targetY, ref cameraOffsetVelocity.y, smoothTime);
		transform.position = new Vector3 (cameraOffset.x, cameraOffset.y, -10);

		// Old code
		//float x = target.position.x + defaultCameraOffset.x;
		//float y = target.position.y + defaultCameraOffset.y;
		//transform.position = new Vector3 (x, y, -10);
	}

	public void SetCameraViewport(float size, Vector2 offset) {
		targetCameraSize = size;
		targetCameraOffset = offset;
	}

	public void ResetCameraViewport() {
		targetCameraSize = initialCameraSize;
		targetCameraOffset = initialCameraOffset;
	}

	public void ResetCameraViewportComplete() {
		cam.orthographicSize = targetCameraSize = initialCameraSize;
		targetCameraOffset = initialCameraOffset;

		cameraOffset.x = target.position.x + initialCameraOffset.x;
		cameraOffset.y = target.position.y + initialCameraOffset.y;
	}

	// Called by DimensionHoppping to fix smooth camera issue when dimension hopping
	public void DimensionHopCamera(Vector3 dimensionOffset) {
		cameraOffset.x += dimensionOffset.x;
		cameraOffset.y += dimensionOffset.y;
	}
}
