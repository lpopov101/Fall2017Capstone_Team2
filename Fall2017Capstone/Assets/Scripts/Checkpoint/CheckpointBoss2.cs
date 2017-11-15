using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointBoss2 : CheckpointBehavior {

	private GameObject player;
	private DodgeScript dodgeScript;
	private HighJumpScript highScript;

	void Awake() {
		player = GameObject.FindGameObjectWithTag("Player");
		dodgeScript = player.GetComponent<DodgeScript>();
		highScript = player.GetComponent<HighJumpScript>();
	}

	void LoadCheckpoint() {
		player.transform.position = transform.position;

		CheckpointManagerShell2 manager = CheckpointManager.GetManager<CheckpointManagerShell2>();
		DestroyIfExists(manager.memory1);
		DestroyIfExists(manager.memory2);
		DestroyIfExists(manager.memory3);
		DestroyIfExists(manager.powerup);

		dodgeScript.hasDodgeAbility = true;
		highScript.hasHighJumpAbility = true;
	}
}
