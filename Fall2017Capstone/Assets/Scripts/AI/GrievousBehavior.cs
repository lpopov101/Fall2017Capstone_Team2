using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrievousBehavior : MonoBehaviour {

	public Transform groundPoint;
	public LayerMask groundLayer;
	public float attackRadius;
	public float jumpYMultiplier;
	public float jumpXMultiplier;
	public float preJumpTime;
	public float maxJumpTime;
	public float cooldownTime;

	private GameObject player;
	private Animator animator;
	private Rigidbody2D rigidBody;
	private bool attacking, prevAttacking;
	private bool jumping;
	private float startJumpTime;
	private float targetPointX;
	private float startCooldownTime;
	private bool stunned;
	private bool collidingWithGround;

	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		animator = GetComponent<Animator>();
		rigidBody = GetComponent<Rigidbody2D>();

		attacking = prevAttacking = false;
		jumping = false;
		startJumpTime = 0;
		startCooldownTime = 0;
		stunned = false;
		collidingWithGround = false;
	}

	void Update () {
		if(stunned)
			return;

		bool grounded = IsGrounded(); // Store local variable so only raycasts once

		prevAttacking = attacking;
		attacking = Vector2.Distance(player.transform.position, transform.position) < attackRadius;

		if(attacking && !prevAttacking) { // Just started attacking
			SendMessage("StopWalking");
			startCooldownTime = Time.time - cooldownTime; // Start time at preJumpTime
		} else if(!attacking && prevAttacking) { // Just stopped attacking
			SendMessage("StartWalking");
			SendMessage("ResetFaceDirection");
			animator.SetBool("jumping", false);
		}

		if(attacking) {
			gameObject.SendMessage("OverrideFaceDirection" + (player.transform.position.x > transform.position.x ? "R" : "L"));
			if(!jumping) {
				if(Time.time > startCooldownTime + cooldownTime) {
					animator.SetBool("jumping", true);
				}
				if(Time.time > startCooldownTime + cooldownTime + preJumpTime) {
					jumping = true;
					startJumpTime = Time.time;

					//animator.SetBool("jumping", true);

					Vector2 velocity = rigidBody.velocity;
					velocity.x = (player.transform.position.x - transform.position.x) * jumpXMultiplier;
					velocity.y = jumpYMultiplier;
					rigidBody.velocity = velocity;
				}
			}
		}

		if(jumping) {
			if(grounded && Time.time > startJumpTime + 0.5f || Time.time > startJumpTime + maxJumpTime) {
				jumping = false;
				startCooldownTime = Time.time;
				animator.SetBool("jumping", false);
			} else {
//				Vector2 velocity = rigidBody.velocity;
//				float target = (targetPointX + player.transform.position.x) / 2f;
//				velocity.x += (target - transform.position.x) * jumpXMultiplier;
//				rigidBody.velocity = velocity;
			}
		}
	}
	
	bool IsGrounded() {
		return collidingWithGround || Physics2D.Raycast(groundPoint.position, -Vector2.up, 0.1f, groundLayer);
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if(coll.gameObject.layer == LayerMask.NameToLayer("Ground")) {
			collidingWithGround = true;
		}
	}

	void OnCollisionExit2D(Collision2D coll) {
		if(coll.gameObject.layer == LayerMask.NameToLayer("Ground")) {
			collidingWithGround = false;
		}
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
