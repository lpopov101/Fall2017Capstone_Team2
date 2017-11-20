﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayToastScript : MonoBehaviour {

	public ToastScript toast;
	public GameObject powerup;
	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D coll) {
		//Debug.Log (coll.gameObject.tag);
		if (coll.gameObject.CompareTag ("DimensionHint")) {
#if UNITY_ANDROID
            toast.AltImageToast("mobileshift", 7.0F);
#else
            toast.ImageToast ("shift", 7.0f);
#endif
			GameObject DimensionHint = GameObject.FindGameObjectWithTag ("DimensionHint");
			if( DimensionHint != null)
				DimensionHint.SetActive (false);
		} else if (coll.gameObject.CompareTag ("FragmentHint")) {
			//toast.Toast ("Memory Fragment nearby in dissociated dimension.", 4.0f);
		} else if (coll.gameObject.CompareTag ("MoveHint")) {
#if UNITY_ANDROID
            toast.AltImageToast("mobilemove", 7.0F);
#else
            toast.ImageToast ("move", 7.0f);
#endif
			GameObject MoveHint = GameObject.FindGameObjectWithTag ("MoveHint");
			if( MoveHint != null)
				MoveHint.SetActive (false);
		} else if (coll.gameObject.CompareTag ("JumpHint")) {
#if UNITY_ANDROID
            toast.AltImageToast("mobilejump", 7.0F);
#else
            toast.ImageToast ("jump", 7.0f);
#endif
            GameObject JumpHint = GameObject.FindGameObjectWithTag ("JumpHint");
			if( JumpHint != null)
				JumpHint.SetActive (false);
		} else if (coll.gameObject.CompareTag ("memorydoor")) {
			string text = CutSceneScript.getCount() + "/3 Memory Shards Collected. Collect all shards to advance";
			toast.Toast (text, 7.0f);
		}
	}

	void checkCollected() {
		
		PowerUpScript pws = powerup.GetComponent<PowerUpScript>();
		Debug.Log ("mem collected: "+ CutSceneScript.getCount() + " powerup: " + pws.getSpriteRendererStatus());
		if (CutSceneScript.getCount() >= 3 && (pws == null || !pws.getSpriteRendererStatus())) {
			GameObject[] door = GameObject.FindGameObjectsWithTag("memorydoor");
			foreach (GameObject obj in door) {
				Destroy (obj);
				toast.Toast ("Door is open", 7.0f);
			}
		}
	}
}