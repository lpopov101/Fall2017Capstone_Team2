using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DodgeScript : MonoBehaviour {

	public bool hasDodgeAbility;
	public float dodgeForce = 200.0f;
	public float dodgeTime = 0.25f;
	public float cooldownTime = 1f;

	private Rigidbody2D rigidBody;
	private PlayerControllerImproved playerController;
	private bool gotDodge;
	private Vector2 preDodgeVelocity;

	void Start () {
		rigidBody = GetComponent<Rigidbody2D>();
		playerController = GetComponent<PlayerControllerImproved>();
		gotDodge = true;
		preDodgeVelocity = Vector2.zero;
	}

	void Update () {
		if (hasDodgeAbility && gotDodge && (Input.GetButtonDown("Dodge") || TouchInput.Dash)) {
			ApplyDodgeForce();

			StartCoroutine (DodgeAndCoolDown());
		}
	}

	private void ApplyDodgeForce() {
		preDodgeVelocity = rigidBody.velocity;

		float horizontalAxis = Input.GetAxisRaw("Horizontal") + TouchInput.Horizontal;
		if(horizontalAxis > 0.001f)
			horizontalAxis = 1;
		else if(horizontalAxis < -0.001f)
			horizontalAxis = -1;
		else
			horizontalAxis = playerController.GetFacingRight() ? 1 : -1;
		
		Vector2 vector = new Vector2 (horizontalAxis * dodgeForce, 0);
		rigidBody.AddForce(vector, ForceMode2D.Force);
	}

	IEnumerator DodgeAndCoolDown() {
		gotDodge = false;

		yield return new WaitForSeconds(dodgeTime);
		Vector3 vel = preDodgeVelocity;
		float sign = rigidBody.velocity.x > 0 ? 1 : -1;
		vel.x = Mathf.Abs(preDodgeVelocity.x) * sign;
		vel.y = Mathf.Min(vel.y, 0);
		rigidBody.velocity = vel;

		yield return new WaitForSeconds(cooldownTime);

		gotDodge = true;
	}

	public void SetHasDodge(bool has) {
		hasDodgeAbility = has;
	}
}