using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAreaScript : MonoBehaviour {

	public AudioSource audioReality;
	public AudioSource streetAudio;
	public AudioSource bgm;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.CompareTag ("Player")) {
			audioReality.mute = true;
			streetAudio.mute = true;
			bgm.mute = true;
		}
	}

	void fadeOut() {
	}

	void fadeIn() {
	}
}
