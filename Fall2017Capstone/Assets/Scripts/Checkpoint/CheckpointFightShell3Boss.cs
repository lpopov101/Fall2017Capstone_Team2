using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointFightShell3Boss : CheckpointBehavior {

	public GameObject shell3BossPlaceholder;
	public GameObject background;
	public GameObject[] removedPlatforms;

	private GameObject player;
	private DodgeScript dodgeScript;
	private HighJumpScript highJumpScript;
	private StunScript stunScript;

	void Awake () {
		player = GameObject.FindGameObjectWithTag("Player");
		dodgeScript = player.GetComponent<DodgeScript>();
		highJumpScript = player.GetComponent<HighJumpScript>();
		stunScript = player.GetComponent<StunScript>();
	}

	void LoadCheckpoint () {
		dodgeScript.hasDodgeAbility = true;
		highJumpScript.hasHighJumpAbility = true;
		stunScript.hasStunAbility = true;

		player.transform.position = transform.position;

		shell3BossPlaceholder.SendMessage("SpawnBoss");
		Destroy(shell3BossPlaceholder);

		foreach(GameObject obj in removedPlatforms) {
			Destroy(obj);
		}
	}
}
