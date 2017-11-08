using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
        SceneManager.LoadScene(PlayerPrefs.GetString("nextScene"), LoadSceneMode.Single);
    }

    public static void loadSceneWithScreen(string nextScene)
    {
        PlayerPrefs.SetString("nextScene", nextScene);
        SceneManager.LoadScene("LoadingScreen", LoadSceneMode.Single);
    }
}
