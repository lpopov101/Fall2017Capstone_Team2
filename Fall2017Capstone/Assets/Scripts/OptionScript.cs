using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionScript : MonoBehaviour {

	public GameObject Gatorp;
	public AudioSource bgm;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void MuteAll () {
		Debug.Log ("here");
		AudioListener audioListener = Gatorp.GetComponent<AudioListener>();
		audioListener.enabled = !audioListener.enabled;
	}

	public void MasterVolume (float vol) {
		//AudioListener audioListener = Gatorp.GetComponent<AudioListener>();
		AudioListener.volume = vol;
	}

	public void PitchVolume (float vol) {
		bgm.pitch = vol;

	}
}
