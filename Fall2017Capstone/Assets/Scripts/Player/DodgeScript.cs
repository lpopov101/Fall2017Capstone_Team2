using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DodgeScript : MonoBehaviour {

	public bool hasDodgeAbility;
	public float cooldownTime;

	private bool gotDodge;

    void Start () {
		hasDodgeAbility = false;
		gotDodge = true;
	}

	void Update () {
		if(hasDodgeAbility && gotDodge && Input.GetButtonDown("Dodge"))
        {
            gameObject.SendMessage("Dodge");
            StartCoroutine(CoolDown());
        }
	}

	IEnumerator CoolDown() {
		//Debug.Log ("Dodge CoolDown");
		gotDodge = false;
		yield return new WaitForSeconds(cooldownTime);
		gotDodge = true;
		//Debug.Log ("Dodge CoolDown complete");
	}
		
}
