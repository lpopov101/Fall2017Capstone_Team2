using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueAreaScript : MonoBehaviour {
    public CameraScript cameraScript;
    public float cameraSize;
    public Vector2 cameraOffset;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            cameraScript.SetCameraViewport(cameraSize, cameraOffset);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            cameraScript.ResetCameraViewport();
        }
    }
}
