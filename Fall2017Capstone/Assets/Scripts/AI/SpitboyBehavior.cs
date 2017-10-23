using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpitboyBehavior : MonoBehaviour {

	public Transform attackPoint;
	public float attackRange;
	public float attackAnimDuration;
	public float chargeAttackTime;
	public float attackIdleTime;

	public GameObject projectilePrefab;
	public float heightPredictionMultiplierX;
	public float heightPredictionMultiplierY;
	public float speedPredictionMultiplierX;
	public float speedPredictionMultiplierY;

	private GameObject player;
	private Animator animator;
	private bool attacking;
	private bool attackLock;

	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		animator = GetComponent<Animator>();
		attacking = false;
		attackLock = false;
	}

	void Update () {
		float distance = Vector2.Distance(transform.position, player.transform.position);
		if(distance < attackRange) {
			if(!attacking) { // First attack loop
				attacking = true;
				gameObject.SendMessage("StopWalking");
				StartCoroutine(Attack());
			}
			gameObject.SendMessage("OverrideFaceDirection" + (player.transform.position.x > transform.position.x ? "R" : "L"));
		} else {
			if(attacking) { // First no-longer-attacking loop
				attacking = false;
				gameObject.SendMessage("StartWalking");
				gameObject.SendMessage("ResetFaceDirection");
			}
		}
	}

	IEnumerator Attack() {
		if(attackLock) // In case two coroutines end up existing for some reason, break out of the new one
			yield break;

		attackLock = true;
		while(attacking) {
			animator.SetBool("attacking", true);
			yield return new WaitForSeconds(chargeAttackTime);
			if(!attacking) { // Variable may change after the charging time
				animator.SetBool("attacking", false);
				break;
			}
			
			GameObject projectileObj = Instantiate(projectilePrefab);
			projectileObj.transform.position = attackPoint.position;

			Vector2 offset = player.transform.position - attackPoint.position;
			Vector3 target = player.transform.position + Vector3.up * (Mathf.Sqrt(Mathf.Abs(offset.x))*heightPredictionMultiplierX + (offset.y+0.8f)*heightPredictionMultiplierY);
			Vector2 vec = target - attackPoint.position;
			vec.Normalize();
			vec = vec * (Mathf.Sqrt(offset.magnitude)*speedPredictionMultiplierX + Mathf.Abs(offset.y)*speedPredictionMultiplierY);
			projectileObj.GetComponent<ProjectileBehavior>().SetVelocity(vec);

			yield return new WaitForSeconds(attackAnimDuration - chargeAttackTime);
			animator.SetBool("attacking", false);
			yield return new WaitForSeconds(attackIdleTime);
		}
		attackLock = false;
	}
}
