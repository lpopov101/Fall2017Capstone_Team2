using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHandsBehavior : MonoBehaviour {

	public GameObject target;
	public GameObject handIndicatorPrefab;
	public GameObject handPrefab;
	public float attackRadius = 1.0f;
	public float idleTime = 1.0f;
	public float handIndicatorTime;

	private int attackPhase; // 0: idle, 1: preparing, 2: attacking
	private Vector3 targetPos; // The position the creature lands on
	private GameObject handIndicatorObj;
	private float groundYValue;
	private bool idle;

	void Start () {
		attackPhase = 0;
		targetPos = Vector3.zero;
		handIndicatorObj = null;
		groundYValue = target.transform.position.y;
		idle = false;
	}

	void Update() {
		if (idle) {
			return;
		}

		if(attackPhase == 0 && Vector2.Distance(transform.position, target.transform.position) <= attackRadius) {
			attackPhase++;
		}
		if(attackPhase == 1) {
			attackPhase++;
			StartCoroutine(PrepareForAttack());
		}
		if(attackPhase == 2) {
			Instantiate(handPrefab);
			Vector3 pos = targetPos;
			pos.y = groundYValue-0.5f;
			handPrefab.transform.position = pos;
		}
	}

	IEnumerator PrepareForAttack() {
		if (attackPhase != 1) {
			Debug.LogError ("Trying to run PrepareForAttack in the wrong attack phase");
			yield break;
		}

		targetPos = target.transform.position;
		handIndicatorObj = Instantiate(handIndicatorPrefab);
		handIndicatorObj.transform.position = targetPos;
		yield return new WaitForSeconds(handIndicatorTime);

		attackPhase++;
	}

	IEnumerator Idle() {
		idle = true;
		yield return new WaitForSeconds(idleTime);
		idle = false;
	}
}
