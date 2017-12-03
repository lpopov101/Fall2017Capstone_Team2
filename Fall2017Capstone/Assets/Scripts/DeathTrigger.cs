using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathTrigger : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.CompareTag("Player")) {
			DeathBehavior deathBehavior = coll.gameObject.GetComponent<DeathBehavior>();
			deathBehavior.SetDead();
		}
	}
}
