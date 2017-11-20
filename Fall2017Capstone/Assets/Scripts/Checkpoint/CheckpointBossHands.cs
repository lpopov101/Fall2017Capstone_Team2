using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointBossHands : CheckpointBehavior {

	private GameObject player;
	private DodgeScript dodgeScript;

	void Awake() {
		player = GameObject.FindGameObjectWithTag("Player");
		dodgeScript = player.GetComponent<DodgeScript>();
	}

	void LoadCheckpoint() {
		player.transform.position = transform.position;

		CheckpointManagerShell1 manager = CheckpointManager.GetManager<CheckpointManagerShell1>();
		//DestroyIfExists(manager.gina);
		DestroyIfExists(manager.memory1);
		DestroyIfExists(manager.memory2);
		DestroyIfExists(manager.memory3);
		DestroyIfExists(manager.powerup);

		player.SendMessage ("SetHardDimension",false);
		dodgeScript.hasDodgeAbility = true;
	}
}
