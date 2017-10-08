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

	private bool attacking;
	private GameObject handIndicatorObj;

	// Use this for initialization
	void Start () {
		attacking = false;
		handIndicatorObj = null;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if(coll.gameObject.CompareTag("Player") && !attacking) {
			StartCoroutine(Attack());
		}
	}

	IEnumerator Attack() {
		attacking = true;

		yield return StartCoroutine(PrepareForAttack());
		yield return StartCoroutine(DoAttack());
		yield return StartCoroutine(Idle());

		attacking = false;
	}

	IEnumerator PrepareForAttack() {
		handIndicatorObj = Instantiate(handIndicatorPrefab);
		handIndicatorObj.transform.position = new Vector3(transform.position.x,transform.position.y, handIndicatorObj.transform.position.z);
		yield return new WaitForSeconds(handIndicatorTime);
	}

	IEnumerator DoAttack() {
		GameObject handObj = Instantiate(handPrefab);
		handObj.transform.position = transform.position;
		BossHandBehavior handBehavior = handObj.GetComponent<BossHandBehavior>();
		handBehavior.growthSpeed = handGrowthSpeed;
		handBehavior.shrinkSpeed = handShrinkSpeed;
		handBehavior.growthAmount = handGrowthAmount;
		handBehavior.waitTime = handWaitTime;
		handBehavior.SetSpikeBase(this);
		yield return null;
	}

	IEnumerator Idle() {
		yield return new WaitForSeconds(idleTime);
	}

	public GameObject GetHandIndicator() {
		return handIndicatorObj;
	}
}
