using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicScriptBoss3 : MonoBehaviour {

	public AudioSource main;
	public AudioSource layer1;
	public AudioSource layer2;
	public AudioSource layer3;
	public AudioSource beginning;
	int count = 0;
	public float incrementSpeed;
	public float raiseVolMax;
	// Use this for initialization
	void Start () {
		raiseVolMax = 0.35f;

		main.Pause();
		layer1.Pause();
		layer2.Pause();
		layer3.Pause();

		main.volume = 0f;
		layer1.volume = 0f;
		layer2.volume = 0f;
		layer3.volume = 0f;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void startMain () {
		main.volume = 0.5f;
		main.Play ();
		layer1.Play ();
		layer2.Play ();
		layer3.Play ();
	}

	IEnumerator increaseLayerVolume (AudioSource layer) {
		float initVolume = layer.volume;
		while (layer.volume < raiseVolMax) {
			layer.volume += incrementSpeed * Time.deltaTime;
			Debug.Log (layer.volume);
			if (layer.volume > raiseVolMax)
				layer.volume = raiseVolMax;
			yield return null;
		}

	}

	void updateMusic(int lives) {
		if (lives == 0) {
		
		}
		if (lives != -1) {
			count++;
			if (count <= 3) {
				Debug.Log ("adding layer" + count);
				string layernum = "musiclayer" + count;
				AudioSource layer = GameObject.Find (layernum).GetComponent<AudioSource> ();
				StartCoroutine (increaseLayerVolume(layer));
				//layer.volume = Mathf.Lerp (0f, 0.35f, 1f);
			}
		}
	}
}
