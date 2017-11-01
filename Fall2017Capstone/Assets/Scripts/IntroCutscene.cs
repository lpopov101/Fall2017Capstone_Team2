using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class IntroCutscene : MonoBehaviour {

	public VideoPlayer videoPlayer;
	AudioSource audioSource;

	void Start()
	{
		audioSource = GetComponent<AudioSource>();


		// Will attach a VideoPlayer to the main camera.
		GameObject camera = GameObject.Find("Main Camera");
		// By default, VideoPlayers added to a camera will use the far plane.
		// Let's target the near plane instead.
		videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.CameraNearPlane;

		videoPlayer.targetCameraAlpha = 1.0F;
		videoPlayer.source = VideoSource.Url;
		videoPlayer.url = "Assets/StreamingAssets/Intro CutScene.mp4";
		videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;
		videoPlayer.EnableAudioTrack (0, true);
		videoPlayer.SetTargetAudioSource (0, audioSource);

		// Each time we reach the end
		videoPlayer.loopPointReached += EndReached;
		//To better control the delays
		//videoPlayer.Prepare();
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
