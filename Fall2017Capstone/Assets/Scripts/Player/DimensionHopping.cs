using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionHopping : MonoBehaviour {

    public Vector3 DimensionOffset;
	
	public AudioSource streetAudio;
	public AudioSource clubAudio;

	private AudioSource dimHopAudio;
	private CameraScript cameraScript;
	private bool DimensionMode;

    void Start()
    {
		dimHopAudio = GetComponent<AudioSource>();
		cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>();
        DimensionMode = true;
    }

    void Update () {
		if(Input.GetButtonDown("DimensionShift"))
        {
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
	}
}
