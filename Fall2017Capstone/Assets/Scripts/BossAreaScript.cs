using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAreaScript : MonoBehaviour {

	public AudioSource audioReality;
	public AudioSource streetAudio;
	public AudioSource bgm;
	public AudioSource enterArea;
	public float cameraSize;
	public Vector2 cameraOffset;

	private Camera cam;
	private CameraScript cameraScript;
	private float initialCameraSize;
	private Vector2 initialCameraOffset;

	void Start () {
		GameObject camObj = GameObject.FindGameObjectWithTag("MainCamera");
		cam = camObj.GetComponent<Camera>();
		cameraScript = camObj.GetComponent<CameraScript>();

		initialCameraSize = cam.orthographicSize;
		initialCameraOffset = cameraScript.cameraOffset;
	}

	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.CompareTag ("Player")) {
			audioReality.mute = true;
			streetAudio.mute = true;
			bgm.mute = true;
			enterArea.Play ();

			cam.orthographicSize = cameraSize;
			cameraScript.cameraOffset = cameraOffset;
		}
	}

	void OnTriggerExit2D(Collider2D coll) {
		if(coll.gameObject.CompareTag("Player") && coll.gameObject.transform.position.y < -500) {
			cam.orthographicSize = initialCameraSize;
			cameraScript.cameraOffset = initialCameraOffset;
		}
	}

	void fadeOut() {
	}

	void fadeIn() {
	}
}
