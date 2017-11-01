using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManagerShell2 : CheckpointManager {

	public CheckpointBehavior startingCheckpoint;
	public GameObject memory1, memory2, memory3;
	//public PowerUpScript powerup;

	void Start () {
		InitCheckpointManager();
	}

	protected override void LoadSceneFirstTime() {
		PlayerPrefs.SetString("Current Checkpoint", startingCheckpoint.name);
		PlayerPrefs.SetInt("Shell2_Mem1", 0);
		PlayerPrefs.SetInt("Shell2_Mem2", 0);
		PlayerPrefs.SetInt("Shell2_Mem3", 0);
		PlayerPrefs.SetInt("Powerup", 0);
	}
}
