using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerCameraScript : MonoBehaviour, IDimensionEventListener {

	public static bool ignoreExitTrigger = false;

	public Vector2 offset;
	private CameraScript camScript;

	void Start () {
		camScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if(other.gameObject.CompareTag("Player")) {
			camScript.SetCameraOffset(offset);
		}
	}

	void OnTriggerExit2D(Collider2D other) {
		if(other.gameObject.CompareTag("Player")) {
			if(!ignoreExitTrigger)
				camScript.ResetCameraViewport();
			else
				ignoreExitTrigger = false;
		}
	}

	public void OnDimensionChange(bool reality) {
		ignoreExitTrigger = true;
	}
}
