using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * Note: this will only be created no matter how many times the same scene is loaded. It only gets
 *       replaced when a new different scene is loaded.
 */
public class CheckpointManagerShell1 : CheckpointManager {

	public CheckpointBehavior startingCheckpoint;
	public GinaBehavior gina;
	public MemoryScript memory1, memory2, memory3;
	public PowerUpScript powerup;

	void Start () {
		InitCheckpointManager();
		//currentCheckpointName = startingCheckpoint.name;
		PlayerPrefs.SetString("Current Checkpoint", startingCheckpoint.name);
	}
}
