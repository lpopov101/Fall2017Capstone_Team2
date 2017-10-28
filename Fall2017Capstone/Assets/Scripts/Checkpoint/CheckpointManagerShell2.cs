using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManagerShell2 : CheckpointManager {

	public CheckpointBehavior startingCheckpoint;
	public MemoryScript memory1, memory2, memory3;
	//public PowerUpScript powerup;

	void Start () {
		InitCheckpointManager();
	}

	protected override void LoadSceneFirstTime() {
		PlayerPrefs.SetString("Current Checkpoint", startingCheckpoint.name);
	}
}
