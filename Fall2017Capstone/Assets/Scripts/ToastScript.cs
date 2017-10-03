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

	public void Toast(string text,float time) {
		StartCoroutine(DisplayText(text,time));
	}

	IEnumerator DisplayText(string text, float time) {
		Toaster.text = text;
		Toaster.CrossFadeAlpha(0.0f, time, false);
		yield return new WaitForSeconds(time);
		Toaster.text = "";
		Toaster.CrossFadeAlpha(1.0f, 0.1f, false);
	}

}
