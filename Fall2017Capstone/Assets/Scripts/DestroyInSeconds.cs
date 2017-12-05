using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyInSeconds : MonoBehaviour {

	public float duration;

	void Start () {
		Destroy(gameObject, duration);
	}
}
