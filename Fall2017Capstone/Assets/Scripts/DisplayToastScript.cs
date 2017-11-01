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
		//Debug.Log (coll.gameObject.tag);
		if (coll.gameObject.CompareTag ("DimensionHint")) {
			toast.ImageToast ("shift", 7.0f);
			GameObject DimensionHint = GameObject.FindGameObjectWithTag ("DimensionHint");
			if( DimensionHint != null)
				DimensionHint.SetActive (false);
		} else if (coll.gameObject.CompareTag ("FragmentHint")) {
			toast.Toast ("Memory Fragment nearby in dissociated dimension.", 4.0f);
		} else if (coll.gameObject.CompareTag ("MoveHint")) {
			toast.ImageToast ("move", 7.0f);
			GameObject MoveHint = GameObject.FindGameObjectWithTag ("MoveHint");
			if( MoveHint != null)
				MoveHint.SetActive (false);
		} else if (coll.gameObject.CompareTag ("JumpHint")) {
			toast.ImageToast ("jump", 7.0f);
			GameObject JumpHint = GameObject.FindGameObjectWithTag ("JumpHint");
			if( JumpHint != null)
				JumpHint.SetActive (false);
		} else if (coll.gameObject.CompareTag ("memorydoor")) {
			string text = CutSceneScript.getCount() + "/3 Memory Shards Collected. Collect all shards to advance";
			toast.Toast (text, 7.0f);
		}
	}

	void checkCollected() {
		if (CutSceneScript.getCount() >= 3 && gameObject.GetComponent<DodgeScript>().hasDodgeAbility) {
			GameObject[] door = GameObject.FindGameObjectsWithTag("memorydoor");
			foreach (GameObject obj in door) {
				Destroy (obj);
				toast.Toast ("Door is open", 7.0f);
			}
		}
	}
}