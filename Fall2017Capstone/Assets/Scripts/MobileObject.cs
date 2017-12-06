using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MobileObject : MonoBehaviour {

	// Use this for initialization
	void Start () {
#if UNITY_ANDROID
#else
        gameObject.SetActive(false);
#endif
    }
}
