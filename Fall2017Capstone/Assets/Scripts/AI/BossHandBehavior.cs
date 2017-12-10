﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHandBehavior : MonoBehaviour {

	public float growthSpeed = 1.0f;
	public float shrinkSpeed = 1.0f;
	public float growthAmount = 1.0f;
	public float waitTime = 1.0f;

	private float initialY;
	private int attackPhase; // 0: grow, 1: stay up, 2: shrink
	private Animator anim;
	private ISpike spikeBase;

	void Start () {
		initialY = transform.position.y;
		attackPhase = 0;
		anim = transform.GetChild(0).GetComponent<Animator>();
	}

	void Update() {
		if(attackPhase == 0) {
			transform.Translate(Vector3.up * growthSpeed * Time.deltaTime);
			if(transform.position.y >= initialY + growthAmount) {
				attackPhase++;
				transform.position.Set(transform.position.x, initialY + growthAmount, 0);
				anim.SetBool("isAttack", false);
				StartCoroutine(Wait());
			}
		} else if(attackPhase == 2) {
			transform.Translate(Vector3.down * shrinkSpeed * Time.deltaTime);
			if(transform.position.y < initialY) {
				Destroy(gameObject);
			}
		}
	}

	IEnumerator Wait() {
		if(attackPhase != 1) {
			Debug.LogError ("Trying to run Wait in the wrong attack phase");
			yield break;
		}

		yield return new WaitForSeconds(waitTime);
		if(spikeBase != null) {
			GameObject handIndicator = spikeBase.GetHandIndicator();
			if(handIndicator)
				handIndicator.GetComponent<BossHandIndicatorBehavior>().StartFading();
		}
		attackPhase++;
	}

	public void SetSpikeBase(ISpike boss) {
		spikeBase = boss;
	}
}