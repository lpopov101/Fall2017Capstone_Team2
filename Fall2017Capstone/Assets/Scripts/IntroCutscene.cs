using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroCutscene : MonoBehaviour {

	//public MovieTexture movie;
	public string nextScene;

	private AudioSource audioSource;

	void Start () {
		audioSource = GetComponent<AudioSource>();
		//audioSource.clip = movie.audioClip;

		//movie.Play();
		audioSource.Play();
	}

	void Update () {
		//if(!movie.isPlaying)
			SceneManager.LoadScene(nextScene, LoadSceneMode.Single);
	}

	void OnGUI() {
		/*if(movie && movie.isPlaying)
			GUI.DrawTexture(new Rect (0, 0, Screen.width, Screen.height), movie, ScaleMode.ScaleToFit, false, 0);*/
	}
}
