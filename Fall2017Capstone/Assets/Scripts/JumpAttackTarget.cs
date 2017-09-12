using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpAttackTarget : MonoBehaviour {

	public GameObject target;
	public GameObject landIndicatorPrefab;
	public float attackRadius;
	public float idleTime;
	public float jumpForce;
	public float landForce;
	public float jumpHoverTime;
	public float landHoverTime;

	Rigidbody2D rb;
	int attackPhase; // 0: idle, 1: jumping, 2: hovering, 3: landing
	Vector3 prevPos; // The creature's position before jumping
	Vector3 targetPos; // The position the creature lands on
	GameObject landIndicatorObj;
	bool idle;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		attackPhase = 0;
		prevPos = targetPos = Vector3.zero;
		landIndicatorObj = null;
		idle = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if(idle) {
			return;
		}

		if(attackPhase == 0 && Vector2.Distance(transform.position, target.transform.position) <= attackRadius) {
			attackPhase++;
			prevPos = transform.position;
			rb.AddForce(Vector2.up * jumpForce);
		}
		if(attackPhase == 1 && transform.position.y - prevPos.y > 20.0f) {
			attackPhase++;
			StartCoroutine(Hover());
		}
		if(attackPhase == 2) {
			rb.velocity = Vector2.zero;
		}
	}

	IEnumerator Hover() {
		if(attackPhase != 2)
			yield break;

		yield return new WaitForSeconds(jumpHoverTime);	
		targetPos = target.transform.position;
		landIndicatorObj = Instantiate(landIndicatorPrefab);
		landIndicatorObj.transform.position = targetPos;
		yield return new WaitForSeconds(landHoverTime);

		attackPhase++;
		transform.position = new Vector3(targetPos.x, transform.position.y);
		rb.velocity = Vector2.zero;
		rb.AddForce(Vector2.down * landForce);
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if(attackPhase == 3) {
			attackPhase = 0;
			prevPos = targetPos = Vector3.zero;
			Destroy(landIndicatorObj);
			landIndicatorObj = null;
			StartCoroutine(Idle());
		}
	}

	IEnumerator Idle() {
		idle = true;
		yield return new WaitForSeconds(idleTime);
		idle = false;
	}
}
