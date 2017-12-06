using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour {

	public GameObject mainMenuPanel;
	public GameObject levelsPanel;
	public AudioSource buttonSound;
    public GameObject mainInitButton;
    public GameObject levelsInitButton;

	public void LoadLevel(string scene) {
		buttonSound.Play ();
        LoadingScreen.loadSceneWithScreen(scene);
	}

	public void SwitchToPanel(string panel) {
		buttonSound.Play ();
		mainMenuPanel.SetActive(panel == "Main Menu");
		levelsPanel.SetActive(panel == "Levels");
        if(panel == "Main Menu")
        {
            EventSystem.current.SetSelectedGameObject(mainInitButton);
        }
        else if(panel == "Levels")
        {
            EventSystem.current.SetSelectedGameObject(levelsInitButton);
        }
	}

	public void QuitGame() {
		buttonSound.Play ();
		Application.Quit();
	}
}
