using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void DestroyIfExists(MonoBehaviour mono) {
		DestroyIfExists(mono.gameObject);
	}

	public void DestroyIfExists(GameObject obj) {
		if(obj != null)
			Destroy(obj);
	}
}
