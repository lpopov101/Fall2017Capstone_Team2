using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroCutscene : MonoBehaviour {

	public VideoPlayer videoPlayer;
	public VideoClip cutScene;
    public RawImage videoImage;
	AudioSource audioSource;

	void Start()
	{
		audioSource = gameObject.AddComponent<AudioSource>();
		videoPlayer.clip = cutScene;
		videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
		videoPlayer.EnableAudioTrack (0, true);
		videoPlayer.SetTargetAudioSource (0, audioSource);
		audioSource.volume = 1.0f;
		videoPlayer.playOnAwake = false;
		audioSource.playOnAwake = false;
		videoPlayer.controlledAudioTrackCount = 1;
		// Each time we reach the end
		videoPlayer.loopPointReached += EndReached;
		videoPlayer.prepareCompleted += VideoPlayer_prepareCompleted;
		//To better control the delays
		videoPlayer.Prepare();

		//videoPlayer.Play();
	}

	void VideoPlayer_prepareCompleted (VideoPlayer source)
	{
		Debug.Log ("prepare complete");
		audioSource.Play ();
		videoPlayer.Play ();
	}

	void EndReached(UnityEngine.Video.VideoPlayer vp)
	{
		vp.Stop ();
        LoadingScreen.loadSceneWithScreen(nextScene);
	}

	void Update () {
		if (Input.GetButtonDown ("Interact") || TouchInput.TapAnywhere)
        {
            LoadingScreen.loadSceneWithScreen(nextScene);
		}
	}

	public string nextScene;


}
