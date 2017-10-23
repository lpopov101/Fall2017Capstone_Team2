﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointStartShell1 : CheckpointBehavior {

	private GameObject player;
	private DimensionHopping dimensionHop;
	private DodgeScript dodgeScript;

	void Awake() {
		player = GameObject.FindGameObjectWithTag("Player");
		dimensionHop = player.GetComponent<DimensionHopping>();
		dodgeScript = player.GetComponent<DodgeScript>();
		int memory1 = PlayerPrefs.GetInt("Memory_1");
		int memory2 = PlayerPrefs.GetInt("Memory_2");
		int memory3 = PlayerPrefs.GetInt("Memory_3");
		MemoryScript.setCount(memory1, memory2, memory3);
	}

	void LoadCheckpoint() {
		int gina = PlayerPrefs.GetInt("Gina");
		int memory1 = PlayerPrefs.GetInt("Memory_1");
		int memory2 = PlayerPrefs.GetInt("Memory_2");
		int memory3 = PlayerPrefs.GetInt("Memory_3");
		int powerup = PlayerPrefs.GetInt("Powerup");
		MemoryScript.setCount(memory1, memory2, memory3);

		CheckpointManagerShell1 manager = CheckpointManager.GetManager<CheckpointManagerShell1>();
		DestroyOnCondition(manager.gina, gina);
		DestroyOnCondition(manager.memory1, memory1);
		DestroyOnCondition(manager.memory2, memory2);
		DestroyOnCondition(manager.memory3, memory3);
		DestroyOnCondition(manager.powerup, powerup);

		if(powerup != 0)
			dodgeScript.hasDodgeAbility = true;
	}
}
