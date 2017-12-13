using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Shell3BossBehavior : MonoBehaviour {

	public enum Action {IDLE, TELEPORT, ATTACK1, ATTACK2, ATTACK3, STUN, STUN_DISAPPEAR, DYING, SPIKE_ATTACK, _LENGTH} // _LENGTH is used to easily access the number of enum values

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
	public float spikeIntervalTime;
	public float spikeStartBufferTime;
	public float spikeEndBufferTime;

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
	public GameObject spikePrefab;
	public GameObject cameraShaker;

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
	private int attack1ProjectileCount;
	private bool[] attack1Spawned, attack1Shot;
	private GameObject[] attack1Projectile;
	private bool attack3Spawned, attack3Shot;
	private GameObject attack3Projectile;
	private bool shouldFacePlayer;
	//private CapsuleCollider2D collider;
	private Vector2 defaultColliderSize;
	private Action[] possibleRandomActions;
	private Action[] stopFacingPlayer;
	private GameObject spikeSpawnPositions;
	private SpriteRenderer spriteRenderer;

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
		attack1ProjectileCount = 3;
		attack1Spawned = new bool[attack1ProjectileCount];
		attack1Shot = new bool[attack1ProjectileCount];
		attack1Projectile = new GameObject[attack1ProjectileCount];
		attack3Spawned = attack3Shot = false;
		attack3Projectile = null;
		shouldFacePlayer = true;
		//collider = GetComponent<CapsuleCollider2D>();
		//defaultColliderSize = collider.size;
		spikeSpawnPositions = GameObject.Find("Spike Spawn Positions");
		spriteRenderer = GetComponent<SpriteRenderer>();

		float teleportTime = disappearDuration + reappearDuration;
		float spikeAttackTime = teleportTime + spikeStartBufferTime + spikeEndBufferTime + spikeIntervalTime * 6;
		actionDurations = new float[] {
			idleTime, teleportTime, attack1Time, attack2Time, attack3Time, stunTime, stunDisappearTime,
			-1, spikeAttackTime
		};
		possibleRandomActions = new Action[] {
			//Action.IDLE, Action.ATTACK1
			Action.IDLE, Action.TELEPORT, Action.ATTACK1, Action.ATTACK2, Action.ATTACK3,
			Action.SPIKE_ATTACK
		};
		stopFacingPlayer = new Action[] {
			Action.ATTACK1, Action.ATTACK2, Action.ATTACK3, Action.STUN, Action.STUN_DISAPPEAR,
			Action.DYING
		};
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
		int index = random.Next(possibleRandomActions.Length);
		return possibleRandomActions[index];
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
			for(int i = 0; i < attack1ProjectileCount; i++) {
				attack1Spawned[i] = false;
				attack1Shot[i] = false;
				attack1Projectile[i] = null;
			}
		} else if(currentAction == Action.ATTACK2) {
			StartCoroutine(DoAttack2());
		} else if(currentAction == Action.ATTACK3) {
			attack3Spawned = false;
			attack3Shot = false;
			attack3Projectile = null;
		} else if(currentAction == Action.DYING) {
			//collider.isTrigger = true;
			//gameObject.tag = "Untagged";

			teleported = false; // Borrow teleport boolean from TELEPORT action
		} else if(currentAction == Action.SPIKE_ATTACK) {
			teleported = false; // Borrow teleport boolean from TELEPORT action
			StartCoroutine(SpikeAttack());
		}

		if(stopFacingPlayer.Contains(currentAction)) {
			shouldFacePlayer = false;
		}
	}

	private void EndAction() {
		if(stopFacingPlayer.Contains(currentAction)) {
			shouldFacePlayer = false;
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
			for(int i = 0; i < attack1ProjectileCount; i++) {
				float bufferTime = 0.2f * i;
				if(!attack1Spawned[i] && Time.time > startActionTime + attack1SpawnTime + bufferTime) {
					attack1Spawned[i] = true;

					// Create projectile
					attack1Projectile[i] = Instantiate(projectile1Prefab);
					attack1Projectile[i].transform.position = projectile1Spawn.position;
				} else if(Time.time > startActionTime + attack1SpawnTime && Time.time < attack1ShootTime + bufferTime) {
					// Freeze projectile
					attack1Projectile[i].GetComponent<Rigidbody2D>().velocity = Vector2.zero;
				} else if(!attack1Shot[i] && Time.time > startActionTime + attack1ShootTime + bufferTime) {
					attack1Shot[i] = true;

					// Shoot projectile
					Vector2 forward = (player.transform.position - attack1Projectile[i].transform.position).normalized;
					attack1Projectile[i].GetComponent<Rigidbody2D>().velocity = forward * projectile1Speed;
				}
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
				if(hitPlayer)
					deathPosition = GameObject.Find("Boss Death Position Backup");
				transform.position = deathPosition.transform.position;

				FacePlayer();
				Destroy(cameraShaker);
			}
		} else if(currentAction == Action.SPIKE_ATTACK) {
			if(!teleported && Time.time > startActionTime + disappearDuration) {
				teleported = true;
				spriteRenderer.enabled = false;
			} else if(Time.time > startActionTime + disappearDuration + spikeStartBufferTime + spikeEndBufferTime + spikeIntervalTime * 6) {
				spriteRenderer.enabled = true;
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
			if(currentAction != Action.ATTACK2) {
				break;
			}
		}
	}

	// Called by StunScript
	void StunByPlayer() {
		stunned = true;
		stunTeleportCount = 2 + random.Next(2);

		for(int i = 0; i < attack1ProjectileCount; i++) {
			if(attack1Projectile[i])
				Destroy(attack1Projectile[i]);
		}
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
		//collider.size = defaultColliderSize * 0.6f;

		yield return new WaitForSeconds(seconds);

		//collider.size = defaultColliderSize;
	}

	IEnumerator WaitAndLoadCore() {
		yield return new WaitForSeconds(2);

		LoadingScreen.loadSceneWithScreen(nextLevel);
	}

	IEnumerator SpikeAttack() {
		yield return new WaitForSeconds(disappearDuration + spikeStartBufferTime);
		if(currentAction != Action.SPIKE_ATTACK)
			yield break;

		for(int i = 0; i < 2; i++) {
			SpawnSpike(spikeSpawnPositions.transform.GetChild(0));
			SpawnSpike(spikeSpawnPositions.transform.GetChild(6));
			yield return new WaitForSeconds(spikeIntervalTime);
			SpawnSpike(spikeSpawnPositions.transform.GetChild(1));
			SpawnSpike(spikeSpawnPositions.transform.GetChild(5));
			yield return new WaitForSeconds(spikeIntervalTime);
			SpawnSpike(spikeSpawnPositions.transform.GetChild(2));
			SpawnSpike(spikeSpawnPositions.transform.GetChild(4));
			yield return new WaitForSeconds(spikeIntervalTime);
			SpawnSpike(spikeSpawnPositions.transform.GetChild(3));
			if(i != 1)
				yield return new WaitForSeconds(spikeIntervalTime);
		}
	}

	private void SpawnSpike(Transform transform) {
		GameObject spike = Instantiate(spikePrefab);
		spike.transform.position = transform.position;

		SpikeTrigger spikeTrigger = spike.GetComponent<SpikeTrigger>();
		spikeTrigger.destroyAfterTriggered = true;
		spikeTrigger.Attack();
	}
}
