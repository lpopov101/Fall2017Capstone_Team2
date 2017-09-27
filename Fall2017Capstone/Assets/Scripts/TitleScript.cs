using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleScript : MonoBehaviour {

	public void ButtonNextLevel(string scene) {
		SceneManager.LoadScene (scene,LoadSceneMode.Single);
	}

	public void ButtonQuit() {
		Application.Quit();
	}
}
