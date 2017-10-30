using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointStartShell2 : CheckpointBehavior {

	private GameObject player;
	private DodgeScript dodgeScript;
	private HighJumpScript highJumpScript;

	void Awake() {
		player = GameObject.FindGameObjectWithTag("Player");
		dodgeScript = player.GetComponent<DodgeScript>();
		highJumpScript = player.GetComponent<HighJumpScript>();
	}

	void LoadCheckpoint() {
		int memory1 = PlayerPrefs.GetInt("Memory_1");
		int memory2 = PlayerPrefs.GetInt("Memory_2");
		int memory3 = PlayerPrefs.GetInt("Memory_3");
		int powerup = PlayerPrefs.GetInt("Powerup");

		MemoryScript.setCount(memory1, memory2, memory3);

		CheckpointManagerShell2 manager = CheckpointManager.GetManager<CheckpointManagerShell2>();
		DestroyOnCondition(manager.memory1, memory1);
		DestroyOnCondition(manager.memory2, memory2);
		DestroyOnCondition(manager.memory3, memory3);
		//DestroyOnCondition(manager.powerup, powerup);

		dodgeScript.hasDodgeAbility = true;
		if(powerup != 0)
			highJumpScript.hasHighJumpAbility = true;
	}
}
