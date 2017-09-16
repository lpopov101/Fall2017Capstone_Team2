using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnekBehavior : MonoBehaviour {

	public float movementSpeed = 1.0f;
	public float distanceBeforeFollowing = 3.0f;

	private Rigidbody2D rigidBody;
	private Vector3 initialPosition;
	private GameObject player;
	private PlayerControllerImproved playerController;
	private bool playerOnRightInitial;
	private bool facingRight;

	void Start () {
		rigidBody = GetComponent<Rigidbody2D>();
		initialPosition = transform.position;
		player = GameObject.FindGameObjectWithTag("Player");
		playerController = player.GetComponent<PlayerControllerImproved>();

		playerOnRightInitial = player.transform.position.x > transform.position.x;
		facingRight = true;
	}

	void Update() {
		float scaleX = Mathf.Abs(transform.localScale.x);
		scaleX *= facingRight ? 1 : -1;

		transform.localScale = new Vector3(scaleX, transform.localScale.y, 1);
	}

	void FixedUpdate () {
		float dist = Vector2.Distance(player.transform.position, transform.position);
		bool playerOnRight = player.transform.position.x > transform.position.x;
		bool playerFacingRight = playerController.GetFacingRight();
		bool initialOnRight = transform.position.x >= initialPosition.x; 
		bool initialOnLeft  = transform.position.x <= initialPosition.x;
		bool followPlayer = dist > distanceBeforeFollowing;

		// Make sure the snek isn't going the wrong way past its initial position
		if(playerOnRightInitial && initialOnRight && playerOnRight)
			followPlayer = false;
		if(!playerOnRightInitial && initialOnLeft && !playerOnRight)
			followPlayer = false;
		
		// Make sure the player isn't looking at the snek
		if(playerFacingRight && !playerOnRight)
			followPlayer = false;
		if(!playerFacingRight && playerOnRight)
			followPlayer = false;

		if(followPlayer) {
			// Move
			float x = playerOnRight ? movementSpeed : -movementSpeed;
			float smoothX = Mathf.Lerp(rigidBody.velocity.x, x, 1.0f);

			rigidBody.velocity = new Vector2(smoothX, rigidBody.velocity.y);
		}

		facingRight = playerOnRight;
	}
}
