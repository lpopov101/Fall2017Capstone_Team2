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

	private CameraScript cameraScript;
	private float initialCameraSize;
	private Vector2 initialCameraOffset;
	private bool isEntered;

	void Start () {
		cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>();
		isEntered = false;
	}

	void Update () {

	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.CompareTag ("Player")) {
			if (!isEntered) {
				isEntered = true;
				audioReality.mute = true;
				streetAudio.mute = true;
				bgm.mute = true;
				enterArea.Play ();
			}
			cameraScript.SetCameraViewport(cameraSize, cameraOffset);
		}
	}

	void OnTriggerExit2D(Collider2D coll) {
		if(coll.gameObject.CompareTag("Player")) {
			cameraScript.ResetCameraViewport();
		}
	}

	void fadeOut() {
	}

	void fadeIn() {
	}
}
