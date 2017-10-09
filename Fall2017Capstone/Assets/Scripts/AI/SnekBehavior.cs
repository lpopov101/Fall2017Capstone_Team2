using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnekBehavior : MonoBehaviour {

	public float movementSpeed = 1.0f;
	public float minFollowDistance;
	public float maxFollowDistance;

	private Rigidbody2D rigidBody;
	private GameObject player;
	private PlayerControllerImproved playerController;
	private bool facingRight;

	void Start () {
		rigidBody = GetComponent<Rigidbody2D>();
		player = GameObject.FindGameObjectWithTag("Player");
		playerController = player.GetComponent<PlayerControllerImproved>();

		facingRight = true;
	}

	void Update() {
		// Set the direction the snek is facing
		float scaleX = Mathf.Abs(transform.localScale.x);
		scaleX *= facingRight ? 1 : -1;
		transform.localScale = new Vector3(scaleX, transform.localScale.y, 1);
	}

	void FixedUpdate () {
		float dist = Vector2.Distance(player.transform.position, transform.position);
		bool playerOnRight = player.transform.position.x > transform.position.x;
		bool playerFacingRight = playerController.GetFacingRight();
		bool followPlayer = dist > minFollowDistance && dist < maxFollowDistance;
		
		// Make sure the player isn't looking at the snek
		if(playerFacingRight && !playerOnRight)
			followPlayer = false;
		if(!playerFacingRight && playerOnRight)
			followPlayer = false;

		// Move
		if(followPlayer) {
			float x = playerOnRight ? movementSpeed : -movementSpeed;
			float smoothX = Mathf.Lerp(rigidBody.velocity.x, x, 1.0f);

			rigidBody.velocity = new Vector2(smoothX, rigidBody.velocity.y);
		}

		facingRight = playerOnRight;
	}
}
