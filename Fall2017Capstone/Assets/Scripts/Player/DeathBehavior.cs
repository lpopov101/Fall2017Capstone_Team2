using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathBehavior : MonoBehaviour {

	public GameObject currentCheckpoint;
	public Texture blackTexture;
	public float idleTime;
	public float fadeTime;

	private DimensionHopping dimensionHopScript;
	private PlayerControllerImproved playerController;
	private CameraScript cameraScript;
	private bool isDead;
	private bool fading;
	private float startFadeTime;

	// Use this for initialization
	void Start () {
		dimensionHopScript = GetComponent<DimensionHopping>();
		playerController = GetComponent<PlayerControllerImproved>();
		cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>();

		isDead = false;
		fading = false;
		startFadeTime = 0;
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnGUI() {
		if(fading) {
			float alpha = (Time.time - startFadeTime) / fadeTime;
			GUI.color = new Color(0, 0, 0, alpha);
			GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), blackTexture);
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.CompareTag("Monster") && !isDead) {
			SetDead();
		}
	}

	public void SetDead() {
		// TODO: Insert death animation
		playerController.SetDead(true);
		isDead = true;
		StartCoroutine(FadeAndRespawn());
	}

	IEnumerator FadeAndRespawn() {
		yield return new WaitForSeconds(idleTime);
		fading = true;
		startFadeTime = Time.time;
		yield return new WaitForSeconds(fadeTime);

		gameObject.transform.position = currentCheckpoint.transform.position;
		currentCheckpoint.GetComponent<CheckpointBehavior>().RespawnOnCheckpoint();
		bool reality = currentCheckpoint.transform.position.y > -500;
		dimensionHopScript.ResetDimension(reality);
		cameraScript.ResetCameraViewportComplete();
		gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		playerController.SetDead(false);
		isDead = false;
		fading = false;
	}
}
