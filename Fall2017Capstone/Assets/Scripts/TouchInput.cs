using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour {
    public static float Horizontal;
    public static float Vertical;
    public static bool Jump;
    public static bool Dash;
    public static bool Shift;
    public static bool TapAnywhere;

    private static int leftTouch;
    private static int rightTouch;
    private static Vector2 leftTouchLastPos;
    private static Vector2 rightTouchLastPos; 
    private static bool shiftEntered;

    private static float accelerometerUpdateInterval = 1.0f / 60.0f;
    private static float lowPassKernelWidthInSeconds = 1.0f;
    private static float shakeDetectionThreshold = 0.9f;
    private static float lastShakeTime = 0F;

    float lowPassFilterFactor;
    Vector3 lowPassValue;
    // Use this for initialization
    void Start () {
        Horizontal = 0;
        Vertical = 0;
        Jump = false;
        Shift = false;
        Dash = false;
        
        TapAnywhere = false;
        leftTouch = -1;
        rightTouch = -1;
        leftTouchLastPos = Vector2.zero;
        rightTouchLastPos = Vector2.zero;
        shiftEntered = false;

        lowPassFilterFactor = accelerometerUpdateInterval / lowPassKernelWidthInSeconds;
        shakeDetectionThreshold *= shakeDetectionThreshold;
        lowPassValue = Input.acceleration;
    }
	
	// Update is called once per frame
	void Update () {
        TapAnywhere = false;
        Horizontal = 0;
        Vertical = 0;
        Jump = false;
        Dash = false;
        Shift = false;
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch t = Input.GetTouch(i);
            
            if(t.phase == TouchPhase.Began)
            {
                TapAnywhere = true;
                float hPos = t.position.x;
                if (hPos >= Screen.width / 2)
                {
                    if(rightTouch == -1)
                    {
                        rightTouch = t.fingerId;
                        rightTouchLastPos = t.position;
                    }
                }
                else if (hPos < Screen.width / 2)
                {
                    if(leftTouch == -1)
                    {
                        leftTouch = t.fingerId;
                        leftTouchLastPos = t.position;
                    }
                }
            }
            if(t.phase == TouchPhase.Stationary || t.phase == TouchPhase.Moved)
            {
                if(t.fingerId == rightTouch)
                {
                    Horizontal += 1;
                    if(t.phase == TouchPhase.Moved)
                    {
                        if(t.position.y - rightTouchLastPos.y > (Screen.height/20))
                        {
                            Jump = true;
                        }
                        if(t.position.x - rightTouchLastPos.x > (Screen.width/20))
                        {
                            Dash = true;
                        }
                        rightTouchLastPos = t.position;
                    }
                }
                if(t.fingerId == leftTouch)
                {
                    Horizontal -= 1;
                    if(t.phase == TouchPhase.Moved)
                    {
                        if (t.position.y - leftTouchLastPos.y > (Screen.height/20))
                        {
                            Jump = true;
                        }
                        if (leftTouchLastPos.x - t.position.x > (Screen.width / 20))
                        {
                            Dash = true;
                        }
                        leftTouchLastPos = t.position;
                    }
                }
            }
            if(t.phase == TouchPhase.Ended)
            {
                if (t.fingerId == rightTouch)
                {
                    rightTouch = -1;
                }
                if (t.fingerId == leftTouch)
                {
                    leftTouch = -1;
                }
            }
        }

        Vector3 acceleration = Input.acceleration;
        lowPassValue = Vector3.Lerp(lowPassValue, acceleration, lowPassFilterFactor);
        Vector3 deltaAcceleration = acceleration - lowPassValue;

        if (deltaAcceleration.sqrMagnitude >= shakeDetectionThreshold  && Time.time - lastShakeTime > 0.5F)
        {
            if(!shiftEntered)
            {
                shiftEntered = true;
                lastShakeTime = Time.time;
                Shift = true;
            }
        }
        else
        {
            shiftEntered = false;
        }
    }
}
