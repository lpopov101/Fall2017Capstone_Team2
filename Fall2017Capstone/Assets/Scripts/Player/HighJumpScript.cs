using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighJumpScript : MonoBehaviour {

	public bool hasHighJumpAbility;
	public float cooldownTime;

	private bool gotHighJump;
	private float startCooldownTime;

	void Start () {
		gotHighJump = false;
		startCooldownTime = 0;
	}

	void Update () {
		// Check if the cooldown timer is up
		if(!gotHighJump && Time.time > startCooldownTime + cooldownTime) {
			gotHighJump = true;
		}

		// Jump
		if(hasHighJumpAbility && gotHighJump && (Input.GetButtonDown("Jump") || TouchInput.Jump)) {
			gameObject.SendMessage("HighJump");

			gotHighJump = false;
			startCooldownTime = Time.time;
		}
	}
}
