using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointBossHands : CheckpointBehavior {

	private DimensionHopping dimensionHop;

	void Start() {
		dimensionHop = GameObject.FindGameObjectWithTag("Player").GetComponent<DimensionHopping>();
	}

	public override void RespawnOnCheckpoint() {
		dimensionHop.SetHardToggleDimension(false);
	}
}
