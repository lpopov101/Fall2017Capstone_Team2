using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditScript : MonoBehaviour {

	public Animator anim;
	public float creditsDuration;
	public GameObject mainMenuButton;

	private bool playedCreditsAlready;
	private float startCreditsTime;
	private bool showedMainMenuButton;

	void Start() {
		playedCreditsAlready = false;
		showedMainMenuButton = false;
		mainMenuButton.SetActive(false);
	}

	void Update() {
		float bufferTime = 2;
		if(playedCreditsAlready && !showedMainMenuButton && Time.time > startCreditsTime + creditsDuration) {
			showedMainMenuButton = true;
			anim.SetBool("scroll", false);
			mainMenuButton.SetActive(true);
		}
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (!playedCreditsAlready && coll.gameObject.CompareTag ("Player")) {
			Debug.Log ("Start Credz");
			startCreditsTime = Time.time;
			playedCreditsAlready = true;
			anim.SetBool ("scroll",true);
		}
	}
}
