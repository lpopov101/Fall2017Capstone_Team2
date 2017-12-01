using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{

    public static UIManager Instance = null; // Singleton that currently refreshes when a new scene is loaded

    public GameObject pausedOverlay;
    public Button pauseButton;
    public Animator MemoryHint;
	public GameObject pausePanel;
	public GameObject optionPanel;
	public AudioSource buttonClick;
	public GameObject Gatorp;
    //public GameObject dialogueText;

    bool paused, dialoguePaused;
    //Text dialogueTextScript;

    void Awake()
    {
        Debug.Log("Setting UI Manager instance.");
        Instance = this;
    }

    void Start()
    {
#if UNITY_ANDROID
        pauseButton.onClick.AddListener(() =>
        {
            Pause();
        });
#else
        pauseButton.gameObject.SetActive(false);
#endif
        pausedOverlay.SetActive(false);
        //dialogueText.SetActive(false);

        paused = dialoguePaused = false;
        //dialogueTextScript = dialogueText.GetComponent<Text>();
    }

    void Update()
    {
        // Pause or unpause
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (paused)
            {
                Unpause();
            }
            else
            {
                Pause();
            }
        }

        // End a dialogue
        if (!paused && dialoguePaused && Input.GetKeyDown(KeyCode.Return))
        {
            DialogueUnpause();
        }
    }


    public bool GetPaused()
    {
        return paused;
    }

    public void Pause()
    {
		buttonClick.Play ();
        paused = true;
        UpdateTimeScale();
        AudioListener.pause = true;
        pausedOverlay.SetActive(true);
		pausePanel.SetActive(true);
		optionPanel.SetActive(false);
    }

    public void PauseWithoutOverlay()
    {
        paused = true;
        UpdateTimeScale();
    }

    public void Unpause()
    {
		buttonClick.Play ();
        paused = false;
        UpdateTimeScale();
        AudioListener.pause = false;
        pausedOverlay.SetActive(false);
    }

    public void UnpauseWithoutOverlay()
    {
        paused = false;
        UpdateTimeScale();
    }


    public bool GetDialoguePaused()
    {
        return dialoguePaused;
    }

    public void DialoguePause()
    {
        dialoguePaused = true;
        UpdateTimeScale();
        //dialogueText.SetActive(true);
    }

    public void DialogueUnpause()
    {
        dialoguePaused = false;
        UpdateTimeScale();
        //dialogueText.SetActive(false);
    }

    /*
	 * Displays the specified text as an ingame dialogue.
	 */
    public void DisplayDialogue(string s)
    {
        if (!dialoguePaused)
            DialoguePause();
        //dialogueTextScript.text = s;
    }


    /*
	 * Returns true of any of the 'pause booleans' are true. In other words, the gameplay has been
	 * paused.
	 */
    public bool IsGameplayPaused()
    {
        return paused || dialoguePaused;
    }

    /*
	 * Pauses the time scale if any of the 'pause' booleans are true. Otherwise, unpauses the time scale.
	 */
    public void UpdateTimeScale()
    {
        if (IsGameplayPaused())
            Time.timeScale = 0;
        else
            Time.timeScale = 1;
    }

    public void TitleScreen()
    {
		buttonClick.Play ();
        Unpause();
        LoadingScreen.loadSceneWithScreen("TitleScreen");
    }

	public void SwitchToPanel(string panel) {
		buttonClick.Play ();
		if (panel == "option") {
			optionPanel.SetActive (panel == "option");
			pausePanel.SetActive (panel == "pause");
		} else {
			optionPanel.SetActive (panel == "option");
			pausePanel.SetActive (panel == "pause");
		}


	}

	public void MuteAll (bool isMute) {
		
	}

    void OnTriggerEnter2D(Collider2D coll)
    {
        if (MemoryHint && coll.gameObject.CompareTag("FragmentHint"))
        {
            MemoryHint.SetBool("enterHintArea", true);
        }
    }

	void OnValueChanged(bool ev) {
		Debug.Log (ev);
		AudioListener audioListener = Gatorp.GetComponent<AudioListener>();
		audioListener.enabled = ev;
	}

    void OnTriggerExit2D(Collider2D coll)
    {
        if (MemoryHint && coll.gameObject.CompareTag("FragmentHint"))
        {
            MemoryHint.SetBool("enterHintArea", false);
        }
    }
}
