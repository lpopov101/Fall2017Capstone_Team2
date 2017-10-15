using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour {

	public static string[] GameScenes = {"NewShell1", "Shell2", "Shell3"};

	private static CheckpointManager Instance = null;
	private static bool FirstInit = true;

	//protected string currentSceneName = null;
	//protected string currentCheckpointName = null;

	public static T GetManager<T>() {
		return (T)(object)Instance;
	}

	void Awake() {
		Debug.Log("Setting Checkpoint Manager instance.");
		Instance = this;
	}

	protected void InitCheckpointManager() {
		if(FirstInit) {
			FirstInit = false;
			PlayerPrefs.DeleteAll();
		}

		string oldScene = PlayerPrefs.GetString("Current Scene");
		string newScene = SceneManager.GetActiveScene().name;
		PlayerPrefs.SetString("Current Scene", newScene);
		OnSceneChangedBase(oldScene, newScene);
	}

	private void OnSceneChangedBase(string fromScene, string toScene) {
		//Debug.Log("Scene Change: " + fromScene + ", " + toScene);

		foreach(string sceneName in GameScenes) {
			if(sceneName != toScene)
				continue;
			
			if(fromScene == toScene) {
				ReloadScene();
			}
		}
	}

	protected void ReloadScene() {
		string checkpointName = PlayerPrefs.GetString("Current Checkpoint");
		GameObject.Find(checkpointName).SendMessage("LoadCheckpoint");
	}
}
