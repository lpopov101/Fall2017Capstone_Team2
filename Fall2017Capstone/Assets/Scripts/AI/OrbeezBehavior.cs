using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbeezBehavior : MonoBehaviour {

	public GameObject eye;
	public GameObject projectilePrefab;
	public float projectileSpeed;
	public float attackRange;
	public int attackCount;
	public float attackOffsetTime;
	public float chargeTime;
	public float chargeScaleAmount;
	public float cooldownTime;

	private GameObject player;
	private bool attacking;
	private bool attackLock;
	private bool charging;
	private float initialEyeScale;
	private bool stunned;

	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		attacking = false;
		attackLock = false;
		initialEyeScale = eye.transform.localScale.x;
		stunned = false;
	}

	void Update () {
		if(stunned)
			return;

		float distance = Vector2.Distance(transform.position, player.transform.position);
		if(distance < attackRange) {
			if(!attacking) {
				attacking = true;
				StartCoroutine(Attack());
			}
		} else {
			if(attacking) {
				attacking = false;
			}
		}

		if(charging) {
			Vector3 scale = eye.transform.localScale;
			float value = chargeScaleAmount;
			scale.x -= value * Time.deltaTime;
			scale.y -= value * Time.deltaTime;
			eye.transform.localScale = scale;
		} else {
			eye.transform.localScale = new Vector3(initialEyeScale, initialEyeScale, 1);
		}
	}

	IEnumerator Attack() {
		if(attackLock) // In case two coroutines end up existing for some reason, break out of the new one
			yield break;

		attackLock = true;
		while(attacking) {
			charging = true;
			yield return new WaitForSeconds(chargeTime);
			charging = false;

			if(!attacking)
				break;

			for(int i = 0; i < attackCount; i++) {
				CreateProjectile();
				yield return new WaitForSeconds(attackOffsetTime);
			}

			yield return new WaitForSeconds(cooldownTime);
		}
		attackLock = false;
	}

	void CreateProjectile() {
		GameObject projectile = Instantiate(projectilePrefab);
		projectile.transform.position = transform.position;
		Vector2 forward = (player.transform.position - transform.position).normalized;
		float roll = Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg;
		projectile.transform.rotation = Quaternion.Euler(0, 0, roll - 90);
		projectile.GetComponent<Rigidbody2D>().velocity = forward * projectileSpeed;
	}

	// Called by StunScript
	void StunByPlayer() {
		stunned = true;
		attacking = false;
	}

	// Called by StunScript
	void UnStunByPlayer() {
		stunned = false;
	}
}
