using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	public float lerpSpeed;
	public Transform target;
	public Vector2 defaultCameraOffset;

	private Camera cam;
	private Vector2 cameraOffset;

	private float initialCameraSize;
	private Vector2 initialCameraOffset;
	private float targetCameraSize;
	private Vector2 targetCameraOffset;

	void Start() {
		cam = GetComponent<Camera>();

		initialCameraSize = targetCameraSize = cam.orthographicSize;
		initialCameraOffset = targetCameraOffset = cameraOffset = defaultCameraOffset;
	}

	void LateUpdate() {
		// Interpolate the camera size
		cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetCameraSize, lerpSpeed*Time.deltaTime);

		// Interpolate the camera offset
		float targetX = target.position.x + targetCameraOffset.x;
		float targetY = target.position.y + targetCameraOffset.y;
		cameraOffset.x = Mathf.Lerp(cameraOffset.x, targetX, lerpSpeed*Time.deltaTime);
		cameraOffset.y = Mathf.Lerp(cameraOffset.y, targetY, lerpSpeed*Time.deltaTime);
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

	// Called by DimensionHoppping to fix smooth camera issue when dimension hopping
	public void DimensionHopCamera(Vector3 dimensionOffset) {
		cameraOffset.x += dimensionOffset.x;
		cameraOffset.y += dimensionOffset.y;
	}
}
