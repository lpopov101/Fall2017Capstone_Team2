using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2AreaScript : MonoBehaviour {

	bool isAttack;
	public GameObject chemical;
	public float riseSpeed;
	public AudioSource bossMusic;
	private GameObject sound;
	// Use this for initialization
	void Start () {
		isAttack = false;
		riseSpeed = 0.5f;
		sound = GameObject.FindGameObjectWithTag ("Sound");
		if (sound != null) {
			sound.SetActive(true);
		}
	}



	// Update is called once per frame
	void Update () {
		if (isAttack) {
			float transY = (Time.deltaTime * riseSpeed); 
			chemical.transform.Translate (Vector3.up * transY); 
		}
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.CompareTag ("Player") && !bossMusic.isPlaying) {
			Debug.Log ("Enter boss area. Start boss seq.");
			sound.SetActive(false);
			if( bossMusic != null)
				bossMusic.Play ();
			isAttack = true;
		}
	}
}
