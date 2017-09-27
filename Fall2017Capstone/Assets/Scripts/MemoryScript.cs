using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryScript : MonoBehaviour {

	public Camera camera;
	//public float textShowTimeSeconds = 5.0f;
	public MovieTexture movie;

	AudioSource shutter;
	SpriteRenderer sr;
	//Text memoryText;

	void Start () {
		sr = GetComponent<SpriteRenderer>();
		shutter = GetComponent<AudioSource>();
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.CompareTag ("Player")) {
			sr.enabled = false;
			movie.Play();

		}
	}

	void OnGUI() {
		if (movie != null && movie.isPlaying) {
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), movie, ScaleMode.ScaleToFit, false, 0);
		}
	}


	void OnTriggerExit2D(Collider2D coll) {
	}
}
