using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunScript : MonoBehaviour {

	public bool hasStunAbility;
	public float cooldownTime;
	public float stunRadius;
	public float stunTime;
	public float stunAnimTime;
	public LayerMask stunnableLayer;
	public Animator stunAnimator;

	private bool gotStun;
	private float startCooldownTime;
	private bool stunning;
	private float startStunTime;
	private Collider2D[] stunnedColliders;

	void Start () {
		gotStun = true;
		startCooldownTime = 0;
		stunning = false;
		startStunTime = 0;
		stunnedColliders = null;
	}

	void Update () {
		// Check if the cooldown timer is up
		if(!gotStun && Time.time > startCooldownTime + cooldownTime) {
			gotStun = true;
		}

		// Check if the stun cooldown timer is up
		if(stunning && Time.time > startStunTime + stunTime) {
			stunning = false;
			UnStun();
		}
		if(stunning && Time.time > startStunTime + stunAnimTime) {
			stunAnimator.SetBool("stun", false);
		}

		if(hasStunAbility && gotStun && Input.GetButtonDown("Stun")) {
			Stun();

			gotStun = false;
			startCooldownTime = Time.time;
		}
	}

	void Stun() {
		stunning = true;
		startStunTime = Time.time;
		stunnedColliders = Physics2D.OverlapCircleAll(transform.position, stunRadius, stunnableLayer);

		foreach(Collider2D collider in stunnedColliders) {
			if(collider.gameObject) {
				collider.gameObject.SendMessage("StunByPlayer");
			}
		}

		stunAnimator.SetBool("stun", true);
	}

	private void UnStun() {
		if(stunnedColliders == null)
			return;

		foreach(Collider2D collider in stunnedColliders) {
			if(collider.gameObject) {
				collider.gameObject.SendMessage("UnStunByPlayer");
			}
		}

		stunAnimator.SetBool("stun", false); // Not necessary but just in case
	}

//	void OnDrawGizmos() {
//		Gizmos.DrawSphere(transform.position, stunRadius);
//	}
}
