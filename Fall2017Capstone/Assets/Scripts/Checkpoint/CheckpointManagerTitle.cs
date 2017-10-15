using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManagerTitle : CheckpointManager {

	void Start () {
		InitCheckpointManager();
		PlayerPrefs.DeleteAll();
	}
}
