using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class CutSceneScript : MonoBehaviour {

	public VideoPlayer videoPlayer;
	public VideoClip cutScene;
	public string memoryName;
	SpriteRenderer sr;
	public GameObject shardLight;
	//public Light light;
	public AudioSource memorySound;
	static int count = 0;
	public GameObject gatorp;
	public ToastScript toast;
	public AudioClip shutterSound;
	AudioSource audioSource;

	void Start()
	{
		audioSource = GetComponent<AudioSource>();


		// Will attach a VideoPlayer to the main camera.
		GameObject camera = GameObject.Find("Main Camera");
		sr = GetComponent<SpriteRenderer>();
		// By default, VideoPlayers added to a camera will use the far plane.
		// Let's target the near plane instead.
		videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.CameraNearPlane;

		videoPlayer.targetCameraAlpha = 1.0F;
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
		UIManager.Instance.UnpauseWithoutOverlay();
		ShutterAfterMovie();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.CompareTag ("Player") && sr.enabled) {
			PlayerPrefs.SetInt(memoryName, 1);
			PlayerPrefs.Save();
			videoPlayer.clip = cutScene;
			UIManager.Instance.PauseWithoutOverlay();
			videoPlayer.Prepare ();
			videoPlayer.prepareCompleted += VideoPlayer_prepareCompleted;
			sr.enabled = false;
			shardLight.SetActive(false);
			//light.enabled = false;

			memorySound.mute = true;
			count++;
			Transform fragmentHint = transform.Find ("HintTrigger");
			if (fragmentHint != null) {
				Destroy (fragmentHint.gameObject);
			}
		}
	}

	void VideoPlayer_prepareCompleted (VideoPlayer source)
	{
		videoPlayer.Play();
		audioSource.Play ();
	}

	void ShutterAfterMovie() {
		audioSource.clip = shutterSound;
		audioSource.Play();
		toast.Toast (count+"/3 Memory Fragments collected",4.0f);
		gatorp.SendMessage ("checkCollected");
	}

	public static void setCount(int mem1, int mem2, int mem3) {
		//Debug.Log (mem1 + " " + mem2 + " " + mem3);
		count = mem1+ mem2+ mem3;
	}

	public static int getCount() {
		return count;

	}

}
