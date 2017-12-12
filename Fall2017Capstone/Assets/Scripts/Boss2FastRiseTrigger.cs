using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2FastRiseTrigger : MonoBehaviour {

	public Boss2AreaScript bossScript;

	void OnTriggerEnter2D(Collider2D coll) {
		if(coll.gameObject.CompareTag("Player")) {
			bossScript.FastRiseChemical(transform.position.y);
		}
	}
}
