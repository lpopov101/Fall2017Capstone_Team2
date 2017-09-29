using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHandsBehavior : MonoBehaviour {

	public GameObject target;
	public GameObject handIndicatorPrefab;
	public GameObject handPrefab;
	public float attackRadius;
	public float idleTime;
	public float idleTimeRandomizer;

	public float handIndicatorTime;
	public float handGrowthSpeed;
	public float handShrinkSpeed;
	public float handGrowthAmount;
	public float handWaitTime;

	public float targetPredictRandomizerMin;
	public float targetPredictRandomizerMax;

	private int attackPhase; // 0: idle, 1: preparing, 2: attacking
	private Vector3 targetPos; // The position the creature lands on
	private GameObject handIndicatorObj;
	private float groundYValue;
	private bool idle;

	void Start () {
		attackPhase = 0;
		targetPos = Vector3.zero;
		handIndicatorObj = null;
		groundYValue = transform.position.y;
		idle = false;
	}

	void Update() {
		if(idle) {
			return;
		}

		if(attackPhase == 0 && Vector2.Distance(transform.position, target.transform.position) <= attackRadius) {
			attackPhase++;
			StartCoroutine(PrepareForAttack());
		}
		if(attackPhase == 2) {
			attackPhase = 0;
			Vector3 pos = targetPos;
			pos.y = groundYValue-0.5f;

			GameObject handObj = Instantiate(handPrefab);
			handObj.transform.position = pos;
			BossHandBehavior handBehavior = handObj.GetComponent<BossHandBehavior>();
			handBehavior.growthSpeed = handGrowthSpeed;
			handBehavior.shrinkSpeed = handShrinkSpeed;
			handBehavior.growthAmount = handGrowthAmount;
			handBehavior.waitTime = handWaitTime;
			handBehavior.SetBossBehavior(this);

			StartCoroutine(Idle());
		}
	}

	IEnumerator PrepareForAttack() {
		if(attackPhase != 1) {
			Debug.LogError ("Trying to run PrepareForAttack in the wrong attack phase");
			yield break;
		}

		targetPos = PredictTargetPosition();
		handIndicatorObj = Instantiate(handIndicatorPrefab);
		handIndicatorObj.transform.position = new Vector3(targetPos.x,groundYValue-0.0f, handIndicatorObj.transform.position.z);
		yield return new WaitForSeconds(handIndicatorTime);
		attackPhase++;
	}

	IEnumerator Idle() {
		idle = true;
		yield return new WaitForSeconds(idleTime + Random.Range(-idleTimeRandomizer, idleTimeRandomizer));
		idle = false;
	}

	Vector3 PredictTargetPosition() {
		Vector3 pos = target.transform.position;
		Rigidbody2D rb = target.GetComponent<Rigidbody2D>();

		pos.x += rb.velocity.x * Random.Range(targetPredictRandomizerMin, targetPredictRandomizerMax);
		return pos;
	}

	public GameObject GetHandIndicator() {
		return handIndicatorObj;
	}
}