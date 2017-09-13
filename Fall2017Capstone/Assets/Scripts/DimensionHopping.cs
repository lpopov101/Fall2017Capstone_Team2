using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DimensionHopping : MonoBehaviour {

    public Vector3 DimensionOffset;

    private Rigidbody2D rigidBody;
    private bool DimensionMode;

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        DimensionMode = true;
    }

    // Update is called once per frame
    void Update () {
		if(Input.GetButtonDown("DimensionShift"))
        {
            if(DimensionMode)
            {
                transform.Translate(DimensionOffset);
            }
            else
            {
                transform.Translate(-1*DimensionOffset);
            }
            DimensionMode = !DimensionMode;
        }
	}
}
