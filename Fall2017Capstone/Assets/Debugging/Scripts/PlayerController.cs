using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	Rigidbody2D rb;
	GameObject cam;
	bool isGrounded;
	public SpriteRenderer sr;
	float speed = 7.0F;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		//sr = GetComponent<SpriteRenderer>();
		cam = Camera.main.gameObject;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (isGrounded && Input.GetKeyDown(KeyCode.Space))
		{

			rb.AddForce(new Vector2(0,500),ForceMode2D.Force);
		}
	}

	void Update() {


		if (Input.GetKeyDown(KeyCode.A))
		{
			sr.flipX = true;
		} else if (Input.GetKeyDown(KeyCode.D))
		{
			sr.flipX = false;
		}
		Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
		transform.position += move * speed * Time.deltaTime;

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

	void OnCollisionEnter2D (Collision2D collision) {
		if (collision.gameObject.tag == "ground") {
			isGrounded = true;
		}
	}

	void OnCollisionExit2D (Collision2D collision) {
		if (collision.gameObject.tag == "ground") {
			isGrounded = false;
		}
	}

}
