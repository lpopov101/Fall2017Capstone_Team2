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

	public void DestroyOnCondition(MonoBehaviour mono, int i) {
		DestroyOnCondition(mono.gameObject, i);
	}

	public void DestroyOnCondition(GameObject obj, int i) {
		if(i != 0)
			DestroyIfExists(obj);
	}
}
