using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditScript : MonoBehaviour {

	public Animator anim;

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.CompareTag ("Player")) {
			Debug.Log ("Start Credz");
			anim.SetBool ("scroll",true);
		}
	}
}
