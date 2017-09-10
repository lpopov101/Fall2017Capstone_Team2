using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	Rigidbody2D rb;
	GameObject cam;
	Animator anim;
	bool isGrounded;
	public SpriteRenderer sr;
	float speed = 3.5F;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		cam = Camera.main.gameObject;
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		anim.SetFloat ("speed",Mathf.Abs(Input.GetAxis("Horizontal")));

		Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, 0);
		float speedWithRun = Input.GetKey (KeyCode.LeftControl) ? speed*1.75f : speed;
		transform.position += (move * speedWithRun * Time.deltaTime);

		if(Input.GetKey (KeyCode.LeftControl)) {
			anim.SetBool ("isRunning",true);
		} else if (!Input.GetKey(KeyCode.LeftControl)) {
			anim.SetBool ("isRunning",false);
		}

		if (isGrounded && Input.GetKey(KeyCode.Space))
		{
			anim.SetFloat ("jump",1);
			anim.SetFloat ("landing",0);
			rb.AddForce(new Vector2(0,300),ForceMode2D.Force);
		}


			
	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
		{
			sr.flipX = true;
		} else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
		{
			sr.flipX = false;
		}

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
			anim.SetFloat("jump",0);
			//anim.SetFloat("landing",1);
		}
	}

	void OnCollisionExit2D (Collision2D collision) {
		if (collision.gameObject.tag == "ground") {
			isGrounded = false;
		}
	}

}
