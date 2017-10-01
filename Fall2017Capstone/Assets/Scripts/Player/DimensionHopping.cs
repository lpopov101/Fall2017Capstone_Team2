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
	private bool DimensionMode;

	private bool HardToggleDimension;

    void Start()
    {
		dimHopAudio = GetComponent<AudioSource>();
		cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>();
        DimensionMode = true;
		HardToggleDimension = false;
    }

    void Update () {
		if(Input.GetButtonDown("DimensionShift") && !HardToggleDimension)
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
		}
		else
		{
			streetAudio.mute = false;
			clubAudio.mute = false;
			cameraScript.DimensionHopCamera(-1*DimensionOffset);
			transform.Translate(-1*DimensionOffset);
		}
		DimensionMode = !DimensionMode;
	}

	void OnTriggerEnter2D(Collider2D coll) {
		Debug.Log (coll.gameObject.tag);
		if (coll.gameObject.CompareTag ("ForceSwitchTrigger") && !HardToggleDimension) {
			HardToggleDimension = true;
			if (DimensionMode) {
				toast.Toast ("Something is not right. I can't seem to switch back...");
				ChangeDimension ();
			}
		}
	}

}
