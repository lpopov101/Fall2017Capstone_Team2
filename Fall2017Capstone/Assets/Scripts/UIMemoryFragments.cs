using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMemoryFragments : MonoBehaviour {

	public GameObject fragmentsImage;

	private Animator animator;

	void Start () {
		animator = fragmentsImage.GetComponent<Animator>();

		// Hide all fragments at first, even the gray ones
		// Let the gray ones appear when the player collects his/her first fragment
		HideUIFragments();
	}

	public void UpdateUIFragments(int memoryCount) {
		fragmentsImage.SetActive(true);
		animator.SetInteger("shardCount", memoryCount);
	}

	public void HideUIFragments() {
		fragmentsImage.SetActive(false);
	}
}
