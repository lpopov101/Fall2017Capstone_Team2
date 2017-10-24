using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DimensionHopping : MonoBehaviour {

    public Vector3 DimensionOffset;
	
	public AudioSource streetAudio;
	public AudioSource clubAudio;
	public ToastScript toast;

	private AudioSource dimHopAudio;
	private CameraScript cameraScript;
	private PlayerControllerImproved playerController;
	private bool DimensionMode;

	private bool HardToggleDimension;

    void Start()
    {
		dimHopAudio = GetComponent<AudioSource>();
		cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>();
		playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerImproved>();
        DimensionMode = true;
		HardToggleDimension = false;
    }

    void Update () {
		if(Input.GetButtonDown("DimensionShift") && !HardToggleDimension && !playerController.FreezeMovement())
        {
			ChangeDimension();
        }
	}

	void ChangeDimension() {
		dimHopAudio.Play();
		if(DimensionMode)
		{
			if(streetAudio != null)
				streetAudio.mute = true;
			if(clubAudio != null)
				clubAudio.mute = true;
			cameraScript.DimensionHopCamera(DimensionOffset);
			transform.Translate(DimensionOffset);
			RenderSettings.ambientLight = new Color (0.3f,0.6f,0.9f);
            gameObject.SendMessage("DimensionShift");
        }
		else
		{
			if(streetAudio != null)
				streetAudio.mute = false;
			if(clubAudio != null)
				clubAudio.mute = false;
			cameraScript.DimensionHopCamera(-1*DimensionOffset);
			transform.Translate(-1*DimensionOffset);
			RenderSettings.ambientLight = new Color (0.6f,0.6f,0.6f);
            gameObject.SendMessage("DimensionShift");
        }
		DimensionMode = !DimensionMode;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		//Debug.Log (coll.gameObject.tag);
		if (coll.gameObject.CompareTag ("ToastTrigger") && !HardToggleDimension) {
			HardToggleDimension = true;
			if (DimensionMode) {
				ChangeDimension ();
				if(coll.gameObject.name != PlayerPrefs.GetString("Current Checkpoint")) {
					PlayerPrefs.SetString("Current Checkpoint", coll.gameObject.name);
					PlayerPrefs.Save();
				}
			}
		}
	}

}
