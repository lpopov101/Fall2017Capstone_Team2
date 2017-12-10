using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrigger : MonoBehaviour, ISpike {

	public GameObject handIndicatorPrefab;
	public GameObject handPrefab;

	public float handIndicatorTime;
	public float handGrowthSpeed;
	public float handShrinkSpeed;
	public float handGrowthAmount;
	public float handWaitTime;
	public float idleTime;
	public bool destroyAfterTriggered;

	private bool attacking;
	private GameObject handIndicatorObj;
	private GameObject handObj;
	private float startAttackTime;
	private bool didAttack;

	void Awake () {
		attacking = false;
		handIndicatorObj = null;
		didAttack = false;
	}

	void Update () {
		if(attacking) {
			if(!didAttack && Time.time > startAttackTime + handIndicatorTime) {
				DoAttack();
			} else if(didAttack && Time.time > startAttackTime + handIndicatorTime + idleTime) {
				attacking = false;
				if(destroyAfterTriggered)
					Destroy(gameObject);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if(coll.gameObject.CompareTag("Player") && !attacking) {
			Attack();
		}
	}

	public void Attack() {
		startAttackTime = Time.time;
		attacking = true;
		didAttack = false;

		handIndicatorObj = Instantiate(handIndicatorPrefab);
		handIndicatorObj.transform.position = new Vector3(transform.position.x, transform.position.y, 0);
		handIndicatorObj.transform.parent = transform;
	}

	void DoAttack() {
		didAttack = true;

		GameObject handObj = Instantiate(handPrefab);
		handObj.transform.position = transform.position;
		BossHandBehavior handBehavior = handObj.GetComponent<BossHandBehavior>();
		handBehavior.growthSpeed = handGrowthSpeed;
		handBehavior.shrinkSpeed = handShrinkSpeed;
		handBehavior.growthAmount = handGrowthAmount;
		handBehavior.waitTime = handWaitTime;
		handBehavior.SetSpikeBase(this);
		handBehavior.transform.parent = transform;
	}

	public GameObject GetHandIndicator() {
		return handIndicatorObj;
	}
}
