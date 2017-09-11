using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

	Rigidbody2D rb;
	GameObject cam;
	Animator anim;
	BoxCollider2D bc;

	public SpriteRenderer sr;
	public bool isGrounded;
	public bool isJump;
	public float speed = 3.5F;
	public float jumpSpeed = 550.0f;
	public Vector3 moveDirection = Vector3.zero;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D>();
		cam = Camera.main.gameObject;
		anim = GetComponent<Animator>();
		bc = GetComponent<BoxCollider2D>();
	}

	void Movement() {
		
		//start animation if input is horizontal
		anim.SetFloat ("speed",Mathf.Abs(Input.GetAxisRaw("Horizontal")));
		//apply run speed when players is pressing ctrl
		float speedWithRun = Input.GetKey (KeyCode.LeftControl) ? speed*1.75f : speed;
		//apply movement
		moveDirection = new Vector3 (Input.GetAxisRaw("Horizontal"),moveDirection.y, 0);
		if (isJump) 
		{
			this.transform.Translate ((moveDirection * speed) * Time.deltaTime);
		} 
		else 
		{
			this.transform.Translate ((moveDirection * speedWithRun) * Time.deltaTime);
		}

		//determine when to use running animation
		if(Input.GetKey (KeyCode.LeftControl) && Mathf.Abs(Input.GetAxis("Horizontal")) > 0) 
		{
			anim.SetBool ("isRunning",true);
		} 
		else if (!Input.GetKey(KeyCode.LeftControl)) 
		{
			anim.SetBool ("isRunning",false);
		}

		//jumping mechanic
		if (isGrounded && Input.GetButton("Jump"))
		{
			isJump = true;
			isGrounded = false;
			anim.SetFloat ("jump",1);
			anim.SetFloat ("landing",0);
			rb.AddForce ((transform.up) * jumpSpeed);
			//rb.AddForce(new Vector2(0,300),ForceMode2D.Force);
		}
	}

	// Update is called once per frame
	void FixedUpdate () {
		Movement();

	}

	void Update() {
		if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
		{
			sr.flipX = true;
		}
		else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
		{
			sr.flipX = false;
		}

		if(Input.GetKeyDown(KeyCode.LeftShift)) 
		{
			Vector3 pos = transform.position, camPos = cam.transform.position;
			if(pos.y < -500) 
			{
				pos.y += 1000;
				camPos.y += 1000;
			} 
			else 
			{
				pos.y -= 1000;
				camPos.y -= 1000;
			}
			transform.position = pos;
			cam.transform.position = camPos;
		}
	}

	void OnCollisionEnter2D (Collision2D collision) {
		//if (collision.gameObject.tag == "ground") {
			isGrounded = true;
			isJump = false;
			anim.SetFloat("jump",0);

		//}
	}

	void OnCollisionExit2D (Collision2D collision) {
		isGrounded = false;
	}

}
