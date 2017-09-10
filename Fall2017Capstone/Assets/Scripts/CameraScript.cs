using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	public Transform target;
	public float distance;

	void Update(){
		
		transform.position = new Vector3 (target.position.x, target.position.y, -10);

	}
}
