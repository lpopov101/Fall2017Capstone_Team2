using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2AreaScript : MonoBehaviour {

	public GameObject chemical;
	public float riseSpeed = 0.5f;
	public AudioSource bossMusic;
	public float fastRiseDuration = 1;
	public float fastRiseOffsetY = -5f;
	public float minCameraBoundY;
	public GameObject moveObjUp;
	public FollowScript bossBg;

	private GameObject sound;
	private bool isAttack; // Changed to 'private' because it acts as a private variable
	private bool fastRising;
	private float fastRisingStartY;
	private float fastRisingTargetY;
	private float fastRisingStartTime;
	private GameObject camera;

	void Start () {
		isAttack = false;
		sound = GameObject.FindGameObjectWithTag ("Sound");
		if (sound != null) {
			sound.SetActive(true);
		}
		fastRising = false;
		fastRisingStartY = fastRisingTargetY = 0f;
		fastRisingStartTime = 0f;
		camera = GameObject.FindGameObjectWithTag("MainCamera");
	}
		
	void Update () {
		if (isAttack && !fastRising) {
			float transY = (Time.deltaTime * riseSpeed); 
			chemical.transform.Translate (Vector3.up * transY);
		}

		if(fastRising) {
			Vector3 position = chemical.transform.position;
			if(position.y > fastRisingTargetY || Time.time > fastRisingStartTime + fastRiseDuration) {
				fastRising = false;
			}

			float timeStep = (Time.time - fastRisingStartTime) / fastRiseDuration;
			position.y = Mathf.SmoothStep(fastRisingStartY, fastRisingTargetY, timeStep);
			chemical.transform.position = position;
		}
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.CompareTag ("Player") && !bossMusic.isPlaying) {
			Debug.Log ("Enter boss area. Start boss seq.");
			sound.SetActive (false);
			if (bossMusic != null)
				bossMusic.Play ();
			isAttack = true;
			camera.GetComponent<CameraScript>().SetCameraOffset(Vector2.zero);
			bossBg.following = true;
			StartCoroutine(MoveObjUp());
		}
	}

	public void FastRiseChemical(float y) {
		float targetY = y + fastRiseOffsetY;
		float topY = chemical.GetComponent<BoxCollider2D>().bounds.max.y;
		if(topY >= targetY) {
			return;
		}

		float topYOffset = topY - chemical.transform.position.y;
		// Teleport chemical up just below the camera
		if(topY < camera.transform.position.y - minCameraBoundY) {
			Vector3 chemicalPosition = chemical.transform.position;
			chemicalPosition.y = camera.transform.position.y - topYOffset - minCameraBoundY;
			chemical.transform.position = chemicalPosition;
			// Reload topY value
			topY = chemical.GetComponent<BoxCollider2D>().bounds.max.y;
		}

		fastRising = true;
		fastRisingStartY = chemical.transform.position.y;
		fastRisingTargetY = targetY - topYOffset;
		fastRisingStartTime = Time.time;
	}

	// Small timer for shell 2 boss workaround; moves one platform higher for easier platforming
	IEnumerator MoveObjUp() {
		yield return new WaitForSeconds(1);
		moveObjUp.transform.position = moveObjUp.transform.position + Vector3.up * 1;
	}
}
