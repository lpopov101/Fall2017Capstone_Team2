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
			streetAudio.mute = true;
			clubAudio.mute = true;
			cameraScript.DimensionHopCamera(DimensionOffset);
			transform.Translate(DimensionOffset);
			RenderSettings.ambientLight = new Color (0.3f,0.6f,0.9f);
		}
		else
		{
			streetAudio.mute = false;
			clubAudio.mute = false;
			cameraScript.DimensionHopCamera(-1*DimensionOffset);
			transform.Translate(-1*DimensionOffset);
			RenderSettings.ambientLight = new Color (0.6f,0.6f,0.6f);
		}
		DimensionMode = !DimensionMode;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		//Debug.Log (coll.gameObject.tag);
		if (coll.gameObject.CompareTag ("ToastTrigger") && !HardToggleDimension) {
			gameObject.GetComponent<DeathBehavior>().currentCheckpoint = coll.gameObject;

			HardToggleDimension = true;
			if (DimensionMode) {
				toast.Toast ("Something is not right. I can't seem to switch back...",4.0f);
				ChangeDimension ();
			}
		}
	}

	public bool GetDimensionMode() {
		return DimensionMode;
	}

	public void SetHardToggleDimension(bool toggle) {
		HardToggleDimension = toggle;
	}

	/*
	 * Called when the player dies, and the dimension must be reset.
	 */
	public void ResetDimension(bool reality) {
		if(!reality)
		{
			streetAudio.mute = true;
			clubAudio.mute = true;
			RenderSettings.ambientLight = new Color (0.3f,0.6f,0.9f);
		}
		else
		{
			streetAudio.mute = false;
			clubAudio.mute = false;
			RenderSettings.ambientLight = new Color (0.6f,0.6f,0.6f);
		}
		DimensionMode = reality;
		// No need to fix the position, this is done in DeathBehavior
	}
}
