using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour {

	public GameObject mainMenuPanel;
	public GameObject levelsPanel;
	public GameObject controlsPanel;
	public GameObject creditsPanel;
	public AudioSource buttonSound;
    public GameObject mainInitButton;
    public GameObject levelsInitButton;
	public GameObject controlsInitButton;
	public GameObject creditsInitButton;

	public void LoadLevel(string scene) {
		buttonSound.Play ();
        LoadingScreen.loadSceneWithScreen(scene);
	}

	public void SwitchToPanel(string panel) {
		buttonSound.Play ();
		mainMenuPanel.SetActive(panel == "Main Menu");
		levelsPanel.SetActive(panel == "Levels");
		controlsPanel.SetActive(panel == "Controls");
		creditsPanel.SetActive(panel == "Credits");
//		if(panel == "Main Menu") {
//			EventSystem.current.SetSelectedGameObject(mainInitButton);
//		} else if(panel == "Levels") {
//			EventSystem.current.SetSelectedGameObject(levelsInitButton);
//		} else if(panel == "Controls") {
//			EventSystem.current.SetSelectedGameObject(controlsInitButton);
//		} else if(panel == "Credits") {
//			EventSystem.current.SetSelectedGameObject(creditsInitButton);
//		}
	}

	public void QuitGame() {
		buttonSound.Play ();
		Application.Quit();
	}
}
