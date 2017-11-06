using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoreScript : MonoBehaviour {

	private CameraScript cameraScript;
	public float cameraSize;
	public Vector2 cameraOffset;

	// Use this for initialization
	void Start () {
		cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>();
		cameraScript.SetCameraViewport(cameraSize, cameraOffset);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
