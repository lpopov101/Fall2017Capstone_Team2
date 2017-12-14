using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointStartShell3 : CheckpointBehavior {

	private GameObject player;
	private DodgeScript dodgeScript;
	private HighJumpScript highJumpScript;
	private StunScript stunScript;

	void Awake() {
		player = GameObject.FindGameObjectWithTag("Player");
		dodgeScript = player.GetComponent<DodgeScript>();
		highJumpScript = player.GetComponent<HighJumpScript>();
		stunScript = player.GetComponent<StunScript>();
	}

	void LoadCheckpoint() {
		int memory1 = PlayerPrefs.GetInt("Shell3_Mem1");
		int memory2 = PlayerPrefs.GetInt("Shell3_Mem2");
		int memory3 = PlayerPrefs.GetInt("Shell3_Mem3");
		int powerup = PlayerPrefs.GetInt("Powerup");

		CutSceneScript.setCount(memory1, memory2, memory3);

		CheckpointManagerShell3 manager = CheckpointManager.GetManager<CheckpointManagerShell3>();
		DestroyOnCondition(manager.memory1, memory1);
		DestroyOnCondition(manager.memory2, memory2);
		DestroyOnCondition(manager.memory3, memory3);
		//DestroyOnCondition(manager.powerup, powerup);

		dodgeScript.hasDodgeAbility = true;
		highJumpScript.hasHighJumpAbility = true;
		if(powerup != 0) {
			stunScript.hasStunAbility = true;
			manager.powerup.setSpriteEnabled(false);
		}
		player.SendMessage("checkCollected");
	}
}
