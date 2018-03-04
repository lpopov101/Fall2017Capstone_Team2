using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipBlurScript : MonoBehaviour {

	private GameObject monster;

	void Start () {
		monster = transform.parent.parent.gameObject;
	}

	void Update () {
		bool positiveScale = transform.localScale.x >= 0;
		bool positiveParentScale = monster.transform.localScale.x >= 0;

		if(positiveScale != positiveParentScale) {
			float scaleX = Mathf.Abs(transform.localScale.x);
			scaleX *= positiveParentScale ? 1 : -1;
			transform.localScale = new Vector3(scaleX, transform.localScale.y, 1);
		}
	}
}
