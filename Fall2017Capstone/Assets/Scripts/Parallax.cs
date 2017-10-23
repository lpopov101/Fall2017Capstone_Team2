using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {

	public ParallaxGroup[] parallaxGroups;

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

		foreach (ParallaxGroup group in parallaxGroups) {
			Transform[] transforms = group.parallaxTransforms;
			foreach (Transform t in transforms) {
				t.position += Vector3.right * (deltaX * group.parallaxSpeedMultiplier);
			}
		}
		lastCameraX = cameraTransform.position.x;
	}
}

[System.Serializable]
public class ParallaxGroup {
	public float parallaxSpeedMultiplier;
	public Transform[] parallaxTransforms;
}