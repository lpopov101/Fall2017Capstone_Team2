using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisplayToastScript : MonoBehaviour {

	public ToastScript toast;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D coll) {
		Debug.Log (coll.gameObject.tag);
		if (coll.gameObject.CompareTag ("DimensionHint")) {
			toast.Toast ("Press Shift to Dimension Hop", 7.0f);
			GameObject DimensionHint = GameObject.FindGameObjectWithTag("DimensionHint");
			DimensionHint.SetActive (false);
		} else if (coll.gameObject.CompareTag ("FragmentHint")) {
			toast.Toast ("Memory Fragment nearby in dissociated dimension.", 4.0f);
			GameObject fragmentHint = GameObject.FindGameObjectWithTag("FragmentHint");
			fragmentHint.SetActive (false);
		}
	}
}
