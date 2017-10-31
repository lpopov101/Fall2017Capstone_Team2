using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DHMISBehavior : MonoBehaviour {

	public float attackRange;
	public float rotateTime;
	public float attackTime;
	public float preAttackTime; // Time before actual attack to sync attack with animation
	public float postAttackTime; // Time after actual attack to sync attack with animation
	public float cooldownTime;
	public float dashMultiplier;
	public float fakeFrictionMultiplier;
	public float returnSpeed;
	public float returnRotationMultiplier;

	private GameObject player;
	private Animator animator;
	private Rigidbody2D rigidBody;
	private Vector3 startingPosition;
	private bool attacking;
	private bool attackLock;
	private float startAttackTime;
	private Quaternion startingRotation;
	private bool rotating;
	private bool dashing;

	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		animator = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody2D>();
		startingPosition = transform.position;
		attacking = false;
		attackLock = false;
		startAttackTime = 0;
		startingRotation = Quaternion.identity;
		rotating = false;
		dashing = false;
	}

	void Update () {
		float distance = Vector2.Distance(player.transform.position, transform.position);

		if(!attacking) {
			if(distance < attackRange) {
				attacking = true;
				if(!attackLock) {
					StartCoroutine(Attack());
				}
			}
		}

		if(attacking) {
			if(distance > attackRange) {
				attacking = false;
			}
		}

		if(rotating) {
			float interpolation = (Time.time + Time.deltaTime - startAttackTime) / rotateTime;
			Vector3 forward = player.transform.position - transform.position;
			forward.Normalize();
			float roll = Mathf.Atan2(forward.y, forward.x) * Mathf.Rad2Deg;
			transform.rotation = Quaternion.Slerp(startingRotation, Quaternion.Euler(0, 0, roll-90), interpolation);
		}
	}

	void FixedUpdate() {
		if(dashing) {
			rigidBody.velocity = transform.rotation * Vector3.up * dashMultiplier;
		} else if(!attackLock && Vector2.Distance(transform.position, startingPosition) > 0.2f) {
			rigidBody.velocity = (startingPosition - transform.position) * returnSpeed;
			transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z * returnRotationMultiplier);
		} else {
			rigidBody.velocity *= fakeFrictionMultiplier;
		}
	}

	IEnumerator Attack() {
		if(attackLock)
			yield break;

		attackLock = true;
		while(attacking) {
			startAttackTime = Time.time;
			startingRotation = transform.rotation;

			// Rotate towards player
			rotating = true;
			yield return new WaitForSeconds(rotateTime);
			rotating = false;

			// Attack player
			animator.SetBool("attacking", true);
			yield return new WaitForSeconds(preAttackTime);

			dashing = true;
			yield return new WaitForSeconds(attackTime - preAttackTime - postAttackTime);
			dashing = false;
			rigidBody.velocity = Vector2.zero;

			yield return new WaitForSeconds(postAttackTime);
			animator.SetBool("attacking", false);

			// Cooldown
			yield return new WaitForSeconds(cooldownTime);
		}
		attackLock = false;
	}
}
