using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour {

	public GameObject mainMenuPanel;
	public GameObject levelsPanel;

	public void LoadLevel(string scene) {
		SceneManager.LoadScene (scene,LoadSceneMode.Single);
	}

	public void SwitchToPanel(string panel) {
		mainMenuPanel.SetActive(panel == "Main Menu");
		levelsPanel.SetActive(panel == "Levels");
	}

	public void QuitGame() {
		Application.Quit();
	}
}
