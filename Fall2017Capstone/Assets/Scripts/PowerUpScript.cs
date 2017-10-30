using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpScript : MonoBehaviour {

	// Level is going to be used to determine which bool to set in the powerup scripts.
	//public string level;
	public DodgeScript dodgeScript;
	SpriteRenderer sr;
	public Text text;
	public ToastScript toaster;
	public Light light;


	// Use this for initialization
	void Start () {
		sr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.CompareTag ("Player") && sr.enabled) {
			toaster.Toast ("You have picked up the ability evade. Press Left Control to use.", 5.0f);
			PlayerPrefs.SetInt("Powerup", 1);
			PlayerPrefs.Save();
			dodgeScript.hasDodgeAbility = true;
			sr.enabled = false;
			light.enabled = false;
		}
	}


	IEnumerator DisplayText() {
		text.text = "";
		yield return new WaitForSeconds (5.0f);
		text.text = "";
	}
}
