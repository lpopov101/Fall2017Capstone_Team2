using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2AreaScript : MonoBehaviour {

	bool isAttack;
	public GameObject chemical;
	public float riseSpeed;

	// Use this for initialization
	void Start () {
		isAttack = false;
		riseSpeed = 0.5f;
	}



	// Update is called once per frame
	void Update () {
		if (isAttack) {
			float transY = (Time.deltaTime * riseSpeed); 
			chemical.transform.Translate (Vector3.up * transY); 
		}
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.CompareTag ("Player")) {
			Debug.Log ("Enter boss area. Start boss seq.");
			isAttack = true;
		}
	}
}
