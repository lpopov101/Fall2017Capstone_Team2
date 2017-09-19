using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DodgeScript : MonoBehaviour {


	public bool gotDodge;
	Rigidbody2D rigidBody;
	public float dodgeForce = 15.0f;

	// Use this for initialization
	void Start () {
		gotDodge = false;
		rigidBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if (gotDodge) {
			float horizontalAxis = Input.GetAxisRaw("Horizontal");
			if (Input.GetButtonDown("Left Control")) {
				
				Vector2 vector = new Vector2 (horizontalAxis * dodgeForce,0);
				Debug.Log ("Applying Dodge + "+ vector);
				rigidBody.AddForce(vector,ForceMode2D.Impulse);
				StartCoroutine (CoolDown());
			}
		}
	}

	IEnumerator CoolDown() {
		Debug.Log ("CoolDown");
		gotDodge = false;
		yield return new WaitForSeconds(1.5f);
		gotDodge = true;
		Debug.Log ("CoolDown complete");
	}
}
