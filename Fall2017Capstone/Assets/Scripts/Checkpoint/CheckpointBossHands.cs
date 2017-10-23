using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointBossHands : CheckpointBehavior {

	private GameObject player;
	//private DimensionHopping dimensionHop;
	private DodgeScript dodgeScript;

	void Awake() {
		player = GameObject.FindGameObjectWithTag("Player");
		//dimensionHop = player.GetComponent<DimensionHopping>();
		dodgeScript = player.GetComponent<DodgeScript>();
	}

	void LoadCheckpoint() {
		player.transform.position = transform.position;

		CheckpointManagerShell1 manager = CheckpointManager.GetManager<CheckpointManagerShell1>();
		DestroyIfExists(manager.gina);
		DestroyIfExists(manager.memory1);
		DestroyIfExists(manager.memory2);
		DestroyIfExists(manager.memory3);
		DestroyIfExists(manager.powerup);

		dodgeScript.hasDodgeAbility = true;
	}
}
