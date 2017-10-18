using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryScript : MonoBehaviour {

	public string memoryName;
	//public MovieTexture movie;
	public AudioClip shutterClip;
	public AudioSource memorySound;
	public Light light;
	public ToastScript toast;

	AudioSource audioSource;
	SpriteRenderer sr;

	private float startMovieTime;

	void Start () {
		Destroy(null);
		sr = GetComponent<SpriteRenderer>();
		sr.enabled = true;
		audioSource = GetComponent<AudioSource>();
		//audioSource.clip = movie.audioClip;
		memorySound.mute = false;
		memorySound.spatialBlend = 1f;
		startMovieTime = -1f;
	}

	void Update() {
		/*if(startMovieTime != -1f && Time.realtimeSinceStartup > startMovieTime + movie.duration) {
			ShutterAfterMovie();
		}*/
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.CompareTag ("PlayerInteract") && sr.enabled) {
			PlayerPrefs.SetInt(memoryName, 1);
			PlayerPrefs.Save();
			UIManager.Instance.PauseWithoutOverlay();
			audioSource.Play();
			//movie.Play();
			startMovieTime = Time.realtimeSinceStartup;
			sr.enabled = false;
			light.enabled = false;
			memorySound.mute = true;
			//Destroy the hint
			Transform fragmentHint = transform.Find ("HintTrigger");
			if (fragmentHint != null) {
				Destroy (fragmentHint.gameObject);
			}
		}
	}

	void OnGUI() {
		/*if (movie != null && movie.isPlaying) {
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), movie, ScaleMode.ScaleToFit, false, 0);
		}*/
	}

	void OnTriggerExit2D(Collider2D coll) {
	}

	void ShutterAfterMovie() {
		startMovieTime = -1f;
		UIManager.Instance.UnpauseWithoutOverlay();
		//movie.Stop();
		audioSource.clip = shutterClip;
		audioSource.Play();
		toast.Toast ("Memory Fragment collected",4.0f);
	}
}