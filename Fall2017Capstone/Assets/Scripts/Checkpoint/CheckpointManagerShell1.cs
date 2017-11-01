using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManagerShell1 : CheckpointManager {

	public CheckpointBehavior startingCheckpoint;
	public GinaBehavior gina;
	public GameObject memory1, memory2, memory3;
	public PowerUpScript powerup;

	void Start () {
		InitCheckpointManager();
	}

	protected override void LoadSceneFirstTime() {
		PlayerPrefs.SetString("Current Checkpoint", startingCheckpoint.name);
		PlayerPrefs.SetInt("Gina", 0);
		PlayerPrefs.SetInt("Shell1_Mem1", 0);
		PlayerPrefs.SetInt("Shell1_Mem2", 0);
		PlayerPrefs.SetInt("Shell1_Mem3", 0);
		PlayerPrefs.SetInt("Powerup", 0);
	}
}