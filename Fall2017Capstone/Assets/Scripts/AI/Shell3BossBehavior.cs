using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shell3BossBehavior : MonoBehaviour {

	public enum Action {IDLE, TELEPORT, ATTACK1, ATTACK2, ATTACK3, STUN, STUN_DISAPPEAR, DYING, _LENGTH} // _LENGTH is used to easily access the number of enum values

	public int totalLives;
	public string nextLevel;
	public float randomMovementMultiplier;
	public float dyingMovementMultiplier;

	// Animation times
	public float idleTime;
	//public float teleportTime;
	public float attack1Time;
	public float attack2Time;
	public float attack3Time;
	public float stunTime;
	public float stunDisappearTime;

	// Fine-tuned animation times
	public float disappearDuration;
	public float reappearDuration;
	public float attack1SpawnTime;
	public float attack1ShootTime;
	public float attack2StartTime;
	public float attack2ShotOffsetTime;
	public float attack3SpawnTime;
	public float attack3ShootTime;
	public float dyingStunnedTime;

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
	private Animator animator;

	private bool teleported;
	private bool attack1Spawned, attack1Shot;
	private GameObject attack1Projectile;
	private bool attack3Spawned, attack3Shot;
	private GameObject attack3Projectile;
	private bool shouldFacePlayer;
	private CapsuleCollider2D collider;
	private Vector2 defaultColliderSize;

	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		currentAction = Action.IDLE;
		startActionTime = Time.time;
		random = new System.Random();
		facingRight = true;
		stunned = false;
		lives = totalLives;
		animator = GetComponent<Animator>();
		teleported = false;
		attack1Spawned = attack1Shot = false;
		attack1Projectile = null;
		attack3Spawned = attack3Shot = false;
		attack3Projectile = null;
		shouldFacePlayer = true;
		collider = GetComponent<CapsuleCollider2D>();
		defaultColliderSize = collider.size;

		float teleportTime = disappearDuration + reappearDuration;
		actionDurations = new float[] {
			idleTime, teleportTime, attack1Time, attack2Time, attack3Time, stunTime, stunDisappearTime, -1};
	}

	void Update () {
		if(shouldFacePlayer)
			FacePlayer();

		// Update current action state
		float actionDuration = GetActionDuration(currentAction);
		if(actionDuration != -1 && Time.time > startActionTime + actionDuration) {
			Action action = GetRandomAction();
			if(Mathf.Abs(player.transform.position.x - transform.position.x) > maxXDistanceFromPlayer)
				action = Action.TELEPORT;
			if(stunTeleportCount > 0) {
				action = Action.TELEPORT;
				stunTeleportCount--;
			}

			ChangeAction(action);
		} else {
			UpdateAction();
		}
	}

	void FixedUpdate() {
		float multiplier = randomMovementMultiplier;
		if(currentAction == Action.DYING && teleported) // Dying
			multiplier = dyingMovementMultiplier;
		Vector3 vector = Random.insideUnitCircle * multiplier;
		transform.position += vector;
	}

	private void FacePlayer() {
		// Change facing direction to always face player
		facingRight = player.transform.position.x > transform.position.x;
		float scaleX = Mathf.Abs(transform.localScale.x);
		scaleX *= facingRight ? 1 : -1;
		transform.localScale = new Vector3(scaleX, transform.localScale.y, 1);
	}

	private float GetActionDuration(Action action) {
		return actionDurations[(int)action];
	}

	private Action GetRandomAction() {
		//return Action.ATTACK2;
		return (Action)random.Next((int)Action._LENGTH-3); // Do -3 so stun and death animations are not included
	}

	public void ChangeAction(Action action) {
		// End previous action
		EndAction();

		FacePlayer(); // Face the player in case the player has moved

		// Start new action
		currentAction = action;
		startActionTime = Time.time;
		animator.SetInteger("state", (int)currentAction);
		StartAction();
	}

	private void StartAction() {
		//Debug.Log("Current action: " + currentAction);
		if(currentAction == Action.TELEPORT) {
			teleported = false;
		} else if(currentAction == Action.ATTACK1) {
			attack1Spawned = false;
			attack1Shot = false;
			attack1Projectile = null;
		} else if(currentAction == Action.ATTACK2) {
			StartCoroutine(DoAttack2());
		} else if(currentAction == Action.ATTACK3) {
			attack3Spawned = false;
			attack3Shot = false;
			attack3Projectile = null;
		} else if(currentAction == Action.DYING) {
			collider.isTrigger = true;
			gameObject.tag = "Untagged";

			teleported = false; // Borrow teleport boolean from TELEPORT
		}

		if(currentAction == Action.ATTACK1 || currentAction == Action.ATTACK2 || currentAction == Action.ATTACK3 ||
		   currentAction == Action.STUN || currentAction == Action.STUN_DISAPPEAR || currentAction == Action.DYING) {
			shouldFacePlayer = false;
		}
	}

	private void EndAction() {
		if(currentAction == Action.ATTACK1 || currentAction == Action.ATTACK2 || currentAction == Action.ATTACK3 ||
		   currentAction == Action.STUN || currentAction == Action.STUN_DISAPPEAR || currentAction == Action.DYING) {
			shouldFacePlayer = true;
		}
			
		if(currentAction == Action.DYING) {
			StartCoroutine(WaitAndLoadCore());
		}
	}

	private void UpdateAction() {
		if(currentAction == Action.TELEPORT) {
			if(!teleported && Time.time > startActionTime + disappearDuration) {
				teleported = true;

				// Teleport
				Vector2 position = player.transform.position;
				Vector2 offset = new Vector2();
				offset.x = teleportMinX + (float)random.NextDouble() * (teleportMaxX - teleportMinX);
				offset.x *= random.Next(2) == 0 ? 1 : -1;
				offset.y = teleportMinY + (float)random.NextDouble() * (teleportMaxY - teleportMinY);

				transform.position = position + offset;
			}
		} else if(currentAction == Action.ATTACK1) {
			if(!attack1Spawned && Time.time > startActionTime + attack1SpawnTime) {
				attack1Spawned = true;

				// Create projectile
				attack1Projectile = Instantiate(projectile1Prefab);
				attack1Projectile.transform.position = projectile1Spawn.position;
			}
			if(attack1Spawned && Time.time > startActionTime + attack1SpawnTime && Time.time < attack1ShootTime) {
				// Freeze projectile
				attack1Projectile.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			}
			if(!attack1Shot && attack1Spawned && Time.time > startActionTime + attack1ShootTime) {
				attack1Shot = true;

				// Shoot projectile
				Vector2 forward = (player.transform.position - attack1Projectile.transform.position).normalized;
				attack1Projectile.GetComponent<Rigidbody2D>().velocity = forward * projectile1Speed;
			}
		} else if(currentAction == Action.ATTACK3) {
			if(!attack3Spawned && Time.time > startActionTime + attack3SpawnTime) {
				attack3Spawned = true;

				attack3Projectile = Instantiate(projectile3Prefab);
				attack3Projectile.transform.position = projectile3Spawn.position;
			}
			if(attack3Spawned && Time.time > startActionTime + attack3SpawnTime && Time.time < attack3ShootTime) {
				attack3Projectile.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			}
			if(!attack3Shot && Time.time > startActionTime + attack3ShootTime) {
				attack3Shot = true;

				Vector2 forward = (player.transform.position - attack3Projectile.transform.position).normalized;
				attack3Projectile.GetComponent<Rigidbody2D>().velocity = forward * projectile3Speed;
			}
		} else if(currentAction == Action.DYING) {
			float targetTime = startActionTime;

			if(Time.time > targetTime + dyingStunnedTime) {
				animator.SetBool("dying_stunned", false);
			}
			targetTime += dyingStunnedTime;
			if(!teleported && Time.time > targetTime + disappearDuration) {
				teleported = true;

				// Teleport to the center, or to the right if the player's in the way
				GameObject deathPosition = GameObject.Find("Boss Death Position");
				bool hitPlayer = Vector2.Distance(deathPosition.transform.position, player.transform.position) <= 2;
				Debug.Log("Hit player: " + hitPlayer);
				if(hitPlayer)
					deathPosition = GameObject.Find("Boss Death Position Backup");
				transform.position = deathPosition.transform.position;

				FacePlayer();
			}
		}
	}

	IEnumerator DoAttack2() {
		yield return new WaitForSeconds(attack2StartTime);

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

			yield return new WaitForSeconds(attack2ShotOffsetTime);
			projectile2Anchor.Rotate(0, 0, 18f);
			if(stunned)
				break;
		}
	}

	// Called by StunScript
	void StunByPlayer() {
		stunned = true;
		stunTeleportCount = 2 + random.Next(2);

		if(attack1Projectile)
			Destroy(attack1Projectile);
		if(attack3Projectile)
			Destroy(attack3Projectile);

		// Decrement remaining lives and do action
		lives--;
		if(lives > 0) {
			ChangeAction(Action.STUN);
			StartCoroutine(ShrinkCollisionForSeconds(0.75f));
		} else {
			ChangeAction(Action.DYING);
		}
	}

	// Called by StunScript
	void UnStunByPlayer() {
		stunned = false;
	}

	IEnumerator ShrinkCollisionForSeconds(float seconds) {
		// Shrink temporarily to make it easier for player not to accidentally run into boss
		collider.size = defaultColliderSize * 0.6f;

		yield return new WaitForSeconds(seconds);

		collider.size = defaultColliderSize;
	}

	IEnumerator WaitAndLoadCore() {
		yield return new WaitForSeconds(2);

		LoadingScreen.loadSceneWithScreen(nextLevel);
	}
}
