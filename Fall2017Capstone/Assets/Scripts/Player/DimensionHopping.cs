using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public interface IDimensionEventListener {
	void OnDimensionChange(bool reality);
}

public class DimensionHopping : MonoBehaviour {

    public Vector3 DimensionOffset;
	public Scene scene ;
	public AudioSource streetAudio;
	public AudioSource clubAudio;
	public ToastScript toast;
	public MonoBehaviour[] eventListeners;
    public int MaxStackSize;
    public float DistanceToStackUpdate;
    public Transform PositionCheckPoint;
    public float PositionCheckRadius;

	private AudioSource dimHopAudio;
	private CameraScript cameraScript;
	private PlayerControllerImproved playerController;
	private bool realityMode;
	private bool HardToggleDimension;
	public List<IDimensionEventListener> _eventListeners;
    private Stack<Vector3> positionStack;
    private bool shiftNextPhysicsUpdate;

	/* Deprecated */
	[System.Obsolete("Renamed to realityMode to be a bit more descriptive")]
	private bool DimensionMode;

    void Start()
    {
		scene = SceneManager.GetActiveScene();
		dimHopAudio = GetComponent<AudioSource>();
		cameraScript = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraScript>();
		playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControllerImproved>();
		realityMode = true;
		if (scene.name.Equals ("NewShell1"))
			HardToggleDimension = true;
		else
			HardToggleDimension = false;
		_eventListeners = new List<IDimensionEventListener>();
		foreach(MonoBehaviour mono in eventListeners) {
			_eventListeners.Add((IDimensionEventListener)mono);
		}
        positionStack = new Stack<Vector3>();
        positionStack.Push(transform.position);
        shiftNextPhysicsUpdate = false;
    }

    void Update () {
		if((Input.GetButtonDown("DimensionShift") || TouchInput.Shift) && !HardToggleDimension && !playerController.FreezeMovement())
        {
            shiftNextPhysicsUpdate = true;
        }
        if (Vector3.Distance(transform.position, positionStack.Peek()) >= DistanceToStackUpdate)
        {
            positionStack.Push(transform.position);
            if (positionStack.Count > MaxStackSize)
            {
                positionStack.Pop();
            }
        }
    }

	void ChangeDimension() {
		dimHopAudio.Play();
        Vector3 mostRecentPosition = transform.position;
		if(realityMode) // Switching from reality to dissociative
		{
			if(streetAudio != null)
				streetAudio.mute = true;
			if(clubAudio != null)
				clubAudio.mute = true;
			cameraScript.DimensionHopCamera(DimensionOffset);
			transform.Translate(DimensionOffset);
			//RenderSettings.ambientLight = new Color (0.3f,0.6f,0.9f);
            gameObject.SendMessage("DimensionShift");
        }
		else // Switching from dissociative to reality
		{
			if(streetAudio != null)
				streetAudio.mute = false;
			if(clubAudio != null)
				clubAudio.mute = false;
			cameraScript.DimensionHopCamera(-1*DimensionOffset);
			transform.Translate(-1*DimensionOffset);
			//RenderSettings.ambientLight = new Color (0.6f,0.6f,0.6f);
            gameObject.SendMessage("DimensionShift");
        }
		realityMode = !realityMode;
		foreach(IDimensionEventListener listener in _eventListeners) {
			listener.OnDimensionChange(realityMode);
		}
        bool notColliding = false;
        while(!notColliding && positionStack.Count > 0)
        {
            if (Physics2D.OverlapCircle(PositionCheckPoint.position, PositionCheckRadius, LayerMask.GetMask("Ground")))
            {
                Debug.Log("ayyyyy");
                Vector3 queueHead = positionStack.Pop();
                Vector3 distance = queueHead - mostRecentPosition;
                transform.Translate(distance);
                Debug.Log("translating by " + distance);
                mostRecentPosition = queueHead;
            }
            else
            {
                notColliding = true;
            }
        }
        positionStack.Clear();
        positionStack.Push(transform.position);
	}

    private void FixedUpdate()
    {
        if(shiftNextPhysicsUpdate)
        {
            ChangeDimension();
            shiftNextPhysicsUpdate = false;
        }
    }

    void OnTriggerEnter2D(Collider2D coll) {
		//Debug.Log (coll.gameObject.tag);

		if (coll.gameObject.CompareTag ("DimensionHint")) {
			HardToggleDimension = false;
		}

		if (coll.gameObject.CompareTag ("ToastTrigger") && !HardToggleDimension) {
			Debug.Log("HELLO TOAST TRIGGER");
			HardToggleDimension = true;
			if (realityMode) {
				//toast.Toast ("Something is not right. I can't seem to switch back...",4.0f);
				ChangeDimension ();
				if(coll.gameObject.name != PlayerPrefs.GetString("Current Checkpoint")) {
					PlayerPrefs.SetString("Current Checkpoint", coll.gameObject.name);
					PlayerPrefs.Save();
				}
			}
		}
	}

	void SetHardDimension(bool bHardDimension) {
		HardToggleDimension = bHardDimension;
	}
}
