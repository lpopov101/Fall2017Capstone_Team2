using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathBehavior : MonoBehaviour {

	public Texture blackTexture;
	public float idleTime;
	public float fadeTime;

	private PlayerControllerImproved playerController;
	private bool isDead;
	private bool fading;
	private float startFadeTime;

	void Start () {
		playerController = GetComponent<PlayerControllerImproved>();

		isDead = false;
		fading = false;
		startFadeTime = 0;
	}

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
        playerController.gameObject.SendMessage("KillPlayer");
		isDead = true;
		StartCoroutine(FadeAndRespawn());
	}

	IEnumerator FadeAndRespawn() {
		yield return new WaitForSeconds(idleTime);
		fading = true;
		startFadeTime = Time.time;
		yield return new WaitForSeconds(fadeTime);

		SceneManager.LoadScene("NewShell1", LoadSceneMode.Single);

		/*gameObject.transform.position = currentCheckpoint.transform.position;
		currentCheckpoint.GetComponent<CheckpointBehavior>().RespawnOnCheckpoint();
		bool reality = currentCheckpoint.transform.position.y > -500;
		dimensionHopScript.ResetDimension(reality);
		cameraScript.ResetCameraViewportComplete();
		gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		playerController.SetDead(false);
		isDead = false;
		fading = false;*/
	}
}
