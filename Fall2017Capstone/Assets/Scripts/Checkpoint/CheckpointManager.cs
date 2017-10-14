using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckpointManager : MonoBehaviour {

	public static string[] GameScenes = {"NewShell1", "Shell2", "Shell3"};

	private static CheckpointManager Instance = null;

	protected string currentSceneName = null;
	protected string currentCheckpointName = null;

	public static T GetManager<T>() {
		return (T)(object)Instance;
	}

	void Awake() {
		/*string newSceneName = SceneManager.GetActiveScene().name;

		if(Instance == null) {
			Debug.Log("Creating Checkpoint Manager.");
			Instance = gameObject.GetComponent<CheckpointManager>();
			DontDestroyOnLoad(gameObject);
		} else {
			Debug.Log("Instance: " + Instance.name);
			Debug.Log("Current Scene: " + currentSceneName);
			Debug.Log("New Scene: " + newSceneName);
			if(currentSceneName == newSceneName) { // Same scene, just respawning
				// Don't need two checkpoint managers, delete the new one
				Instance.ReloadScene();
				Destroy(gameObject);
			} else { // New scene
				// New checkpoint manager has different properties, delete the old one
				Debug.Log("Replacing Checkpoint Manager with new one.");
				Destroy(Instance.gameObject);
				Instance = gameObject.GetComponent<CheckpointManager>();
				DontDestroyOnLoad(gameObject);
				OnSceneChangedBase(currentSceneName, newSceneName);
			}
		}

		currentSceneName = newSceneName;
		Debug.Log("Setting current scene: " + currentSceneName);*/

		Debug.Log("Setting Checkpoint Manager instance.");
		Instance = this;
	}

	protected void InitCheckpointManager() {
		string oldScene = PlayerPrefs.GetString("Current Scene");
		string newScene = SceneManager.GetActiveScene().name;
		PlayerPrefs.SetString("Current Scene", newScene);
		OnSceneChangedBase(oldScene, newScene);
	}

	private void OnSceneChangedBase(string fromScene, string toScene) {
		//Debug.Log("Scene Change: " + fromScene + ", " + toScene);

		if(toScene == "TitleScreen") {
			// Going to title screen means throwing away current game, so reset properties
			Reset();
		}

		foreach(string sceneName in GameScenes) {
			if(sceneName != toScene)
				continue;
			
			if(fromScene == toScene) {
				ReloadScene();
			}
		}
	}

	protected void Reset() {
		Instance = null;
		PlayerPrefs.DeleteAll();
		//Destroy(gameObject);
	}

	protected void ReloadScene() {
		string checkpointName = PlayerPrefs.GetString("Current Checkpoint");
		GameObject.Find(checkpointName).SendMessage("LoadCheckpoint");
	}
}
