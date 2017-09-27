using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class IntroScript : MonoBehaviour {
	
	public string nextLevel;
	//TextEditor text;
	public Text text;

	// Use this for initialization
	void Start () {
		//text = GetComponent<GUI> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("Interact")) {
			Debug.Log ("Loading");
			text.color = Color.red;
			text.text = "Loading...";
			SceneManager.LoadScene(nextLevel,LoadSceneMode.Single);
		}
			
	}
}
