using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointStartShell3Boss : CheckpointBehavior {

	private GameObject player;
	private DodgeScript dodgeScript;
	private HighJumpScript highJumpScript;
	private StunScript stunScript;

	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		dodgeScript = player.GetComponent<DodgeScript>();
		highJumpScript = player.GetComponent<HighJumpScript>();
		stunScript = player.GetComponent<StunScript>();
	}

	void LoadCheckpoint() {
		dodgeScript.hasDodgeAbility = true;
		highJumpScript.hasHighJumpAbility = true;
		stunScript.hasStunAbility = true;
	}
}
