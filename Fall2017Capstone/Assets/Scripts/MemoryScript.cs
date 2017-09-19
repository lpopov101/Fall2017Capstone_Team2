using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryScript : MonoBehaviour {

	SpriteRenderer sr;
	public Text memoryText;
	public float textShowTimeSeconds = 5.0f;

	void Start () {
		sr = GetComponent<SpriteRenderer>();
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.CompareTag ("Player")) {
			// Set the interactNPC field
			//Debug.Log("Toucuhed");
			sr.enabled = false;
			//memoryText.text = "Hey, sweetheart. It’s time to wake up, okay?";
			StartCoroutine(DisplayText());
			//memoryText.text = "";
		}
	}

	IEnumerator DisplayText () {
		memoryText.text = "Hey, sweetheart. It’s time to wake up, okay?";
		yield return new WaitForSeconds (textShowTimeSeconds);
		memoryText.text = "";
	}

	void OnTriggerExit2D(Collider2D coll) {
	}
}
