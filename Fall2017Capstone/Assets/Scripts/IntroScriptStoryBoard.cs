using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IntroScriptStoryBoard : MonoBehaviour {

	public Text text;
	public Text pressE;

	// Use this for initialization
	void Start () {
		text.enabled = false;
		//pressE.enabled = false;
		StartCoroutine (WaitForAudio());
	}

	IEnumerator WaitForAudio() {
		yield return new WaitForSeconds (17.0f);
		text.enabled = true;
		//pressE.enabled = true;
	}


	// Update is called once per frame
	void Update () {
		
	}
}
