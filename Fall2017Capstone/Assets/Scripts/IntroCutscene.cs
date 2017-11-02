using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IntroCutscene : MonoBehaviour {

	public VideoPlayer videoPlayer;
	public VideoClip cutScene;
	AudioSource audioSource;

	void Start()
	{
		audioSource = gameObject.AddComponent<AudioSource>();
		// Will attach a VideoPlayer to the main camera.
		GameObject camera = GameObject.Find("Main Camera");
		// By default, VideoPlayers added to a camera will use the far plane.
		// Let's target the near plane instead.
		videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.CameraNearPlane;

		videoPlayer.targetCameraAlpha = 1.0F;
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
		SceneManager.LoadScene (nextScene, LoadSceneMode.Single);
	}

	void Update () {
		if (Input.GetButtonDown ("Interact")) {
			SceneManager.LoadScene (nextScene, LoadSceneMode.Single);
		}
	}

	public string nextScene;


}
