using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour {

	public static UIManager Instance; // Singleton that currently refreshes when a new scene is loaded

	public GameObject pausedOverlay;
	//public GameObject dialogueText;

	bool paused, dialoguePaused;
	Text dialogueTextScript;

	void Start () {
		Instance = this;

		pausedOverlay.SetActive(false);
		//dialogueText.SetActive(false);

		paused = dialoguePaused = false;
		//dialogueTextScript = dialogueText.GetComponent<Text>();
	}

	void Update () {
		// Pause or unpause
		if(Input.GetKeyDown(KeyCode.Escape)) {
			if(paused) {
				Unpause();
			} else {
				Pause();
			}
		}

		// End a dialogue
		if(!paused && dialoguePaused && Input.GetKeyDown(KeyCode.Return)) {
			DialogueUnpause();
		}
	}


	public bool GetPaused() {
		return paused;
	}

	public void Pause() {
		paused = true;
		UpdateTimeScale();
		AudioListener.pause = true;
		pausedOverlay.SetActive(true);
	}

	public void Unpause() {
		paused = false;
		UpdateTimeScale();
		AudioListener.pause = false;
		pausedOverlay.SetActive(false);
	}


	public bool GetDialoguePaused() {
		return dialoguePaused;
	}

	public void DialoguePause() {
		dialoguePaused = true;
		UpdateTimeScale();
		//dialogueText.SetActive(true);
	}

	public void DialogueUnpause() {
		dialoguePaused = false;
		UpdateTimeScale();
		//dialogueText.SetActive(false);
	}

	/*
	 * Displays the specified text as an ingame dialogue.
	 */
	public void DisplayDialogue(string s) {
		if(!dialoguePaused)
			DialoguePause();
		dialogueTextScript.text = s;
	}


	/*
	 * Returns true of any of the 'pause booleans' are true. In other words, the gameplay has been
	 * paused.
	 */
	public bool IsGameplayPaused() {
		return paused || dialoguePaused;
	}

	/*
	 * Pauses the time scale if any of the 'pause' booleans are true. Otherwise, unpauses the time scale.
	 */
	public void UpdateTimeScale() {
		if(IsGameplayPaused())
			Time.timeScale = 0;
		else
			Time.timeScale = 1;
	}

	public void TitleScreen() {
		Unpause();
		SceneManager.LoadScene ("TitleScreen",LoadSceneMode.Single);
	}
}
