using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroScript : MonoBehaviour {
	
	public string nextLevel;
	Text text;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Interact") || TouchInput.TapAnywhere == true) {
			Debug.Log ("Loading");
			text.color = Color.red;
			text.text = "Loading...";
            LoadingScreen.loadSceneWithScreen(nextLevel);
		}
			
	}
}
