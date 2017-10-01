using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToastScript : MonoBehaviour {

	Text Toaster;

	// Use this for initialization
	void Start () {
		Toaster = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Toast(string text) {
		StartCoroutine(DisplayText(text));
	}

	IEnumerator DisplayText(string text) {
		Toaster.text = text;
		yield return new WaitForSeconds (5.0f);
		Toaster.text = "";
	}
}
