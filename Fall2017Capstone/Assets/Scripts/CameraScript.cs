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
	private float maxCameraShake, cameraShake;
	private float randomShakeValue;
	private float changeShakeValueTimer;

	void Start() {
		cam = GetComponent<Camera>();

		cameraOffset.x = target.position.x + defaultCameraOffset.x;
		cameraOffset.y = target.position.y + defaultCameraOffset.y;

		initialCameraSize = targetCameraSize = cam.orthographicSize;
		initialCameraOffset = targetCameraOffset = defaultCameraOffset;

		maxCameraShake = cameraShake = 0;
		randomShakeValue = 0;
		changeShakeValueTimer = 0;
	}

	void Update() {
		// Apply camera shake when camera shakers are nearby
		float maxDistance = 5;
		int cameraShakerLayer = 1 << LayerMask.NameToLayer("Camera Shaker");
		Collider2D[] colliders = Physics2D.OverlapCircleAll(target.position, maxDistance, cameraShakerLayer);
		foreach(Collider2D collider in colliders) {
			float shakeNormalized = (maxDistance-Vector2.Distance(collider.transform.position, transform.position))/maxDistance;
			shakeNormalized = shakeNormalized * shakeNormalized * shakeNormalized; // Steepen the interpolation
			float shake = shakeNormalized * 25;
			shake = Mathf.Min(shake, 7);
			ApplyCameraShake(shake);
		}
	}

	void LateUpdate() {
		// Interpolate the camera size
		cam.orthographicSize = Mathf.SmoothDamp(cam.orthographicSize, targetCameraSize, ref cameraSizeVelocity, smoothTime);

		// Interpolate the camera offset
		float targetX = target.position.x + targetCameraOffset.x;
		float targetY = target.position.y + targetCameraOffset.y;
		cameraOffset.x = Mathf.SmoothDamp(cameraOffset.x, targetX, ref cameraOffsetVelocity.x, smoothTime);
		cameraOffset.y = Mathf.SmoothDamp(cameraOffset.y, targetY, ref cameraOffsetVelocity.y, smoothTime);
		transform.position = new Vector3 (cameraOffset.x, cameraOffset.y + GetCameraShakeMultiplier(), -10);
	}

	public void SetCameraSize(float size) {
		targetCameraSize = size;
	}

	public void SetCameraOffset(Vector2 offset) {
		targetCameraOffset = offset;
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

	public void ApplyCameraShake(float shake) {
		maxCameraShake = Mathf.Max(maxCameraShake, shake);
	}

	private float GetCameraShakeMultiplier() {
		float scale = 1;

		if(Time.time > changeShakeValueTimer) {
			changeShakeValueTimer = Time.time + 0.03f;
			randomShakeValue = Random.Range(-scale, scale) * maxCameraShake;
		}
		
		maxCameraShake = 0;
		cameraShake += randomShakeValue * Time.deltaTime;
		cameraShake *= 0.7f;
		return cameraShake;
	}
}
