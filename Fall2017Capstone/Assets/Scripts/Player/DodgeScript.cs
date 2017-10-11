using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DodgeScript : MonoBehaviour {


	public bool gotDodge;
	public float dodgeForce = 200.0f;
    
    private Rigidbody2D rigidBody;

    // Use this for initialization
    void Start () {
		gotDodge = false;
		rigidBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        if(gotDodge && Input.GetButtonDown("Dodge"))
        {
            gameObject.SendMessage("Dodge");
            StartCoroutine(CoolDown());
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
