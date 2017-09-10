using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTrigger : MonoBehaviour {

	public GameObject interactTextPrefab;

	GameObject interactTextObj;
	GameObject interactNPC;

	void Start () {
		interactTextObj = Instantiate (interactTextPrefab);
		interactTextObj.SetActive (false);

		interactNPC = null;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject != null && coll.gameObject.CompareTag ("NPC")) {
			// Set the interactNPC field
			interactNPC = coll.gameObject;

			// Attach text to NPC
			interactTextObj.SetActive(true);
			interactTextObj.transform.SetParent (interactNPC.transform);
			interactTextObj.transform.localPosition = new Vector3 (0, 3.5f, -5);
		}
	}

	void OnTriggerExit2D(Collider2D coll) {
		if (coll.gameObject != null && coll.gameObject == interactNPC) {
			// Hide the text
			interactTextObj.SetActive(false);

			// Remove the interactNPC instance
			interactNPC = null;
		}
	}
}
