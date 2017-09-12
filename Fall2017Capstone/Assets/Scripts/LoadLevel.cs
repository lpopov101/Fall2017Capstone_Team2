using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadLevel : MonoBehaviour {

	public string nextLevel;

	void Start () {
		
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if(coll.gameObject.CompareTag("Player"))
			SceneManager.LoadScene(nextLevel,LoadSceneMode.Single);
	}

}
