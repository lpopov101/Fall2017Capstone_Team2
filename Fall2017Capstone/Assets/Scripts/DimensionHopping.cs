using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionHopping : MonoBehaviour {

    public Vector3 DimensionOffset;

    private Rigidbody2D rigidBody;
    private bool DimensionMode;
	public AudioSource streetAudio;
	public AudioSource clubAudio;
	AudioSource dimHopAudio;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
		dimHopAudio = GetComponent<AudioSource>();
        DimensionMode = true;
    }

    // Update is called once per frame
    void Update () {
		if(Input.GetButtonDown("DimensionShift"))
        {
			dimHopAudio.Play();
            if(DimensionMode)
            {
				RenderSettings.ambientLight = Color.blue;
				streetAudio.mute = true;
				clubAudio.mute = true;
                transform.Translate(DimensionOffset);
            }
            else
            {
				RenderSettings.ambientLight = Color.white;
				streetAudio.mute = false;
				clubAudio.mute = false;
                transform.Translate(-1*DimensionOffset);
            }
            DimensionMode = !DimensionMode;
        }
	}
}
