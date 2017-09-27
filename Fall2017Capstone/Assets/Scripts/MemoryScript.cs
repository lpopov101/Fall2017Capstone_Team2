using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryScript : MonoBehaviour {

	//public Camera camera;
	//public float textShowTimeSeconds = 5.0f;
	public MovieTexture movie;
	public AudioClip shutterClip;

	AudioSource audioSource;
	SpriteRenderer sr;
	//Text memoryText;

	void Start () {
		sr = GetComponent<SpriteRenderer>();
		audioSource = GetComponent<AudioSource>();
		audioSource.clip = movie.audioClip;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.CompareTag ("Player") && sr.enabled) {
			sr.enabled = false;

			audioSource.Play();
			movie.Play();

			StartCoroutine(ShutterAfterMovie(movie.duration));
		}
	}

	void OnGUI() {
		if (movie != null && movie.isPlaying) {
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), movie, ScaleMode.ScaleToFit, false, 0);
		}
	}

	void OnTriggerExit2D(Collider2D coll) {
	}

	IEnumerator ShutterAfterMovie(float duration) {
		yield return new WaitForSeconds(duration);

		audioSource.clip = shutterClip;
		audioSource.Play();
	}
}
