using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell3BossBehavior : MonoBehaviour {

	public enum Action {IDLE, TELEPORT, ATTACK1, ATTACK2, ATTACK3, _LENGTH} // _LENGTH is used to easily access the number of enum values

	public int totalLives;
	public string nextLevel;
	public float randomMovementMultiplier;
	public float idleTime;
	public float teleportTime;
	public float attack1Time;
	public float attack2Time;
	public float attack3Time;
	public float maxXDistanceFromPlayer;
	public float teleportMinX;
	public float teleportMaxX;
	public float teleportMinY;
	public float teleportMaxY;
	public GameObject projectile1Prefab;
	public Transform projectile1Spawn;
	public float projectile1Speed;
	public GameObject projectile2Prefab;
	public Transform projectile2Anchor;
	public Transform projectile2Spawn;
	public float projectile2Speed;
	public GameObject projectile3Prefab;
	public Transform projectile3Spawn;
	public float projectile3Speed;

	private GameObject player;
	private Action currentAction;
	private float startActionTime;
	private float[] actionDurations;
	private System.Random random;
	private bool facingRight;
	private bool stunned;
	private int stunTeleportCount;
	private int lives;

	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		currentAction = Action.IDLE;
		startActionTime = Time.time;
		actionDurations = new float[] {idleTime, teleportTime, attack1Time, attack2Time, attack3Time};
		random = new System.Random();
		facingRight = true;
		stunned = false;
		lives = totalLives;
	}

	void Update () {
		//if(stunned)
		//	return;

		// Change facing direction to always face player
		facingRight = player.transform.position.x > transform.position.x;
		float scaleX = Mathf.Abs(transform.localScale.x);
		scaleX *= facingRight ? 1 : -1;
		transform.localScale = new Vector3(scaleX, transform.localScale.y, 1);

		// Update current action state
		if(Time.time > startActionTime + GetActionDuration(currentAction)) {
			currentAction = GetRandomAction();
			if(Mathf.Abs(player.transform.position.x - transform.position.x) > maxXDistanceFromPlayer)
				currentAction = Action.TELEPORT;
			if(stunTeleportCount > 0) {
				currentAction = Action.TELEPORT;
				stunTeleportCount--;
			}
			startActionTime = Time.time;
			StartAction();
		} else {
			UpdateAction();
		}
	}

	void FixedUpdate() {
		Vector3 vector = Random.insideUnitCircle * randomMovementMultiplier;
		transform.position += vector;
	}

	public float GetActionDuration(Action action) {
		return actionDurations[(int)action];
	}

	public Action GetRandomAction() {
		return (Action)random.Next((int)Action._LENGTH);
	}

	void StartAction() {
		Debug.Log("Current action: " + currentAction);
		if(currentAction == Action.TELEPORT) {
			Vector2 position = player.transform.position;
			Vector2 offset = new Vector2();
			offset.x = teleportMinX + (float)random.NextDouble() * (teleportMaxX - teleportMinX);
			offset.x *= random.Next(2) == 0 ? 1 : -1;
			offset.y = teleportMinY + (float)random.NextDouble() * (teleportMaxY - teleportMinY);

			transform.position = position + offset;
		} else if(currentAction == Action.ATTACK1) {
			GameObject projectile = Instantiate(projectile1Prefab);
			projectile.transform.position = projectile1Spawn.position;
			Vector2 forward = (player.transform.position - projectile.transform.position).normalized;
			projectile.GetComponent<Rigidbody2D>().velocity = forward * projectile1Speed;
		} else if(currentAction == Action.ATTACK2) {
			StartCoroutine(DoAttack2());
		} else if(currentAction == Action.ATTACK3) {
			GameObject projectile = Instantiate(projectile3Prefab);
			projectile.transform.position = projectile3Spawn.position;
			Vector2 forward = (player.transform.position - projectile.transform.position).normalized;
			projectile.GetComponent<Rigidbody2D>().velocity = forward * projectile3Speed;
		}
	}

	void UpdateAction() {
		
	}

	IEnumerator DoAttack2() {
		Vector2 forward = (player.transform.position - transform.position).normalized;
		float roll = Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg;
		if(facingRight)
			projectile2Anchor.rotation = Quaternion.Euler(0, 0, roll - 45);
		else
			projectile2Anchor.rotation = Quaternion.Euler(0, 0, roll - 90 - 45);

		for(int i = 0; i < 6; i++) {
			GameObject projectile = Instantiate(projectile2Prefab);
			projectile.transform.position = projectile2Spawn.position;
			Vector2 forwardP = (projectile2Spawn.position - projectile2Anchor.transform.position).normalized;
			float rollP = Mathf.Atan2(forwardP.y, forwardP.x) * Mathf.Rad2Deg;
			projectile.transform.rotation = Quaternion.Euler(0, 0, rollP - 90);
			projectile.GetComponent<Rigidbody2D>().velocity = forwardP * projectile2Speed;

			yield return new WaitForSeconds(0.2f);
			projectile2Anchor.Rotate(0, 0, 15f);
			if(stunned)
				break;
		}
	}

	// Called by StunScript
	void StunByPlayer() {
		Debug.Log("Stunned");
		stunned = true;
		lives--;
		if(lives <= 0)
			LoadingScreen.loadSceneWithScreen(nextLevel);
		stunTeleportCount = 4;
		currentAction = Action.TELEPORT;
		stunTeleportCount--;
		startActionTime = Time.time;
		StartAction();
	}

	// Called by StunScript
	void UnStunByPlayer() {
		stunned = false;
	}
}
