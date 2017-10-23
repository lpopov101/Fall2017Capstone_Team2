using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManagerShell2 : CheckpointManager {

	public CheckpointBehavior startingCheckpoint;

	void Start () {
		InitCheckpointManager();
	}

	protected override void LoadSceneFirstTime() {
		PlayerPrefs.SetString("Current Checkpoint", startingCheckpoint.name);
	}
}
