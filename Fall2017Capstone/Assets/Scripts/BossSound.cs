using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSound : MonoBehaviour {

	public AudioClip crackAudio;

	void CrackSound() {
		AudioSource.PlayClipAtPoint(crackAudio, Camera.main.transform.position, 0.2f);
	}
}
