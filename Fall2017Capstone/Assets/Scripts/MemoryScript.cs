using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryScript : MonoBehaviour {
	
	#if UNITY_ANDROID
		void OnTriggerEnter2D(Collider2D coll) {
			if (coll.gameObject.CompareTag ("PlayerInteract") && sr.enabled) {
				PlayerPrefs.SetInt(memoryName, 1);
				PlayerPrefs.Save();
				UIManager.Instance.PauseWithoutOverlay();
				startMovieTime = Time.realtimeSinceStartup;
				Handheld.PlayFullScreenMovie(memoryName,Color.black, FullScreenMovieControlMode.Hidden);
				count++;
				Transform fragmentHint = transform.Find ("HintTrigger");
				if (fragmentHint != null) {
					Destroy (fragmentHint.gameObject);
				}
			}
		}

		void ShutterAfterMovie() {
			startMovieTime = -1f;
			UIManager.Instance.UnpauseWithoutOverlay();
			audioSource.clip = shutterClip;
			audioSource.Play();
			toast.Toast (count+"/3 Memory Fragments collected",4.0f);
		}
	#else
		public MovieTexture movie;
		
		void OnTriggerEnter2D(Collider2D coll) {
			if (coll.gameObject.CompareTag ("PlayerInteract") && sr.enabled) {
				PlayerPrefs.SetInt(memoryName, 1);
				PlayerPrefs.Save();
				UIManager.Instance.PauseWithoutOverlay();
				audioSource.Play();
				movie.Play();
				startMovieTime = Time.realtimeSinceStartup;
				sr.enabled = false;
				light.enabled = false;
				memorySound.mute = true;
				//Destroy the hint
				count++;
				Transform fragmentHint = transform.Find ("HintTrigger");
				if (fragmentHint != null) {
					Destroy (fragmentHint.gameObject);
				}
			}
		}
		
		void ShutterAfterMovie() {
			startMovieTime = -1f;
			UIManager.Instance.UnpauseWithoutOverlay();
			movie.Stop();
			audioSource.clip = shutterClip;
			audioSource.Play();
			toast.Toast (count+"/3 Memory Fragments collected",4.0f);
			Gatorp.SendMessage ("checkCollected");
		}
	#endif

	public string memoryName;
	public AudioClip shutterClip;
	public AudioSource memorySound;
	public Light light;
	public ToastScript toast;
	AudioSource audioSource;
	SpriteRenderer sr;
	static int count = 0;
	public GameObject Gatorp;

	private float startMovieTime;

	void Start () {
		Destroy(null);
		sr = GetComponent<SpriteRenderer>();
		sr.enabled = true;
		audioSource = GetComponent<AudioSource>();
		audioSource.clip = movie.audioClip;
		memorySound.mute = false;
		memorySound.spatialBlend = 1f;
		startMovieTime = -1f;
	}

	void Update() {
		if(startMovieTime != -1f && Time.realtimeSinceStartup > startMovieTime + movie.duration) {
			ShutterAfterMovie();
		}
	}



	void OnGUI() {
		if (movie != null && movie.isPlaying) {
			GUI.DrawTexture (new Rect (0, 0, Screen.width, Screen.height), movie, ScaleMode.ScaleToFit, false, 0);
		}
	}

	void OnTriggerExit2D(Collider2D coll) {
	}

	public static void setCount(int mem1, int mem2, int mem3) {
		//Debug.Log (mem1 + " " + mem2 + " " + mem3);
		count = mem1+ mem2+ mem3;
	}

	public static int getCount() {
		return count;
	}


}