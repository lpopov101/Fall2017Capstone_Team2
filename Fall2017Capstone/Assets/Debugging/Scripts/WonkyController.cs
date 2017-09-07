using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WonkyController : MonoBehaviour {

	Rigidbody2D rb;
	GameObject cam;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		cam = Camera.main.gameObject;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		int moveX = (Input.GetKey(KeyCode.D) ? 1 : 0) - (Input.GetKey(KeyCode.A) ? 1 : 0);
		int moveY = Input.GetKey(KeyCode.Space) ? 1 : 0;

		Vector2 vel = rb.velocity;

		if(moveX != 0)
			vel.x = moveX * 3.0f;
		if(moveY != 0)
			vel.y = moveY * 6.0f;

		rb.velocity = vel;
	}

	void Update() {
		if(Input.GetKeyDown(KeyCode.LeftShift)) {
			Vector3 pos = transform.position, camPos = cam.transform.position;
			if(pos.y < -500) {
				pos.y += 1000;
				camPos.y += 1000;
			} else {
				pos.y -= 1000;
				camPos.y -= 1000;
			}
			transform.position = pos;
			cam.transform.position = camPos;
		}
	}
}
