using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DormantShell3Boss : MonoBehaviour {

	public GameObject shell3BossPrefab;
	public Transform shell3BossSpawn;
	public GameObject background;
	public GameObject leftPlatform;
	public GameObject[] removedPlatforms;
	public float spawningDuration;
	public float platformSpeed;

	private GameObject player;
	private bool spawning;
	private float startSpawnTime;
	private Animator animator;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		spawning = false;
		startSpawnTime = -1;
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(spawning) {
			background.transform.Translate(Vector3.down * platformSpeed * Time.deltaTime);
			leftPlatform.transform.Translate(Vector3.left * platformSpeed * Time.deltaTime);

			if(Time.time > startSpawnTime + spawningDuration) {
				PlayerPrefs.SetString("Current Checkpoint", "Checkpoint Boss Fight");
				Debug.Log(PlayerPrefs.GetString("Current Checkpoint"));

				player.GetComponent<PlayerControllerImproved>().SetCutscenePlaying(false);

				SpawnBoss();

				foreach(GameObject obj in removedPlatforms) {
					Destroy(obj);
				}

				Destroy(gameObject);
			}
		}
	}

	void StunByPlayer() {
		spawning = true;
		startSpawnTime = Time.time;

		// Freeze player movement
		player.GetComponent<PlayerControllerImproved>().SetCutscenePlaying(true);

		animator.SetInteger("state", 1);
	}

	void UnStunByPlayer() {}

	void SpawnBoss() {
		GameObject boss = Instantiate(shell3BossPrefab);
		boss.transform.position = shell3BossSpawn.position;
	}
}
