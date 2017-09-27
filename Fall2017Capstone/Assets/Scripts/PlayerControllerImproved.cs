using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerImproved : MonoBehaviour {
    
    public float acceleration = 3.5F;
    public float speedLimit = 2.0F;
    public float friction = 1.0F;
    public float slopeAngleLimit = 50.0F;
    public float gravity = -30F;
    public float sprintAnimationSpeedCutoff = 1.5F;
    public float skidAnimationSpeedCutoff = 6.0F;
    public float jumpStrength = 600.0F;
    public float airAcceleration = 10F;
    public float airControl = 1.0F;
    public float fallAnimationTriggerTime = 0.3F;
    public float ledgeGrabRange = 2.0F;
    public float ledgeGrabTimeMin = 0.1F;
    public float ledgeGrabTimeMax = 0.5F;
    public Transform ledgePoint;
    public Transform[] groundPoints;
    public LayerMask groundLayer;

    private Rigidbody2D rigidBody;
    private Animator animator;
    private bool isGrounded;
    private bool shouldJump;
    private bool jumped;
    private bool isSkidding;
    private bool ledgeMode;
    private float fallTime;
    private float jumpTime;
	private bool facingRight;
    
	void Start ()
    {
        Physics2D.queriesStartInColliders = false;
        Mathf.Clamp(acceleration, 0, Mathf.Infinity);
        Mathf.Clamp(speedLimit, 0, Mathf.Infinity);
        Mathf.Clamp(jumpStrength, 0, Mathf.Infinity);
        Mathf.Clamp(airAcceleration, 0, Mathf.Infinity);
        Mathf.Clamp(airControl, 0, Mathf.Infinity);
        Mathf.Clamp(fallAnimationTriggerTime, 0, Mathf.Infinity);
        Mathf.Clamp(ledgeGrabRange, 0, Mathf.Infinity);
        Mathf.Clamp(sprintAnimationSpeedCutoff, 0, speedLimit);
        Mathf.Clamp(skidAnimationSpeedCutoff, 0, speedLimit);

        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        isGrounded = false;
        shouldJump = false;
        jumped = false;
        isSkidding = false;
        ledgeMode = false;
        fallTime = -1;
        jumpTime = -1;
		facingRight = true;
	}

    void Update()
    {
        if (isGrounded)
        {
            if(fallTime != -1)
            {
                fallTime = -1;
                animator.SetBool("falling", false);

            }
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                shouldJump = true;
                jumped = false;
            }
        }
        else
        {
            if(fallTime == -1 && rigidBody.velocity.y < 0)
            {
                fallTime = Time.time;
            }
            else if(fallTime != -1)
            {
                float fallDuration = Time.time - fallTime;
                if(fallDuration >= fallAnimationTriggerTime)
                {
                    animator.SetBool("falling", true);
                }
            }
        }

        animator.SetBool("grounded", isGrounded);
        animator.SetBool("jump", shouldJump);
        animator.SetBool("isRunning", Mathf.Abs(rigidBody.velocity.x) >= sprintAnimationSpeedCutoff);
        animator.SetBool("skidding", isSkidding);
        animator.SetBool("moving", Input.GetAxisRaw("Horizontal") != 0);
        animator.SetBool("ledge", ledgeMode);
        animator.SetFloat("horizontalVelocity", rigidBody.velocity.x);
        animator.SetFloat("verticalVelocity", rigidBody.velocity.y);
        animator.SetFloat("jumpTime", Time.time - jumpTime);

		// Calculate facingRight boolean
		float x = Mathf.Abs(rigidBody.velocity.x);
		if(x > 0.01f) { // Make sure the player isn't standing virtually still
			if(facingRight && rigidBody.velocity.x < 0)
				facingRight = false;
			else if(!facingRight && rigidBody.velocity.x > 0)
				facingRight = true;
		}
    }

    void FixedUpdate () {
        float horizontalAxis = Input.GetAxisRaw("Horizontal");
        isGrounded = ledgeMode;
        float distanceToGround = float.MaxValue;
        float forceAngle = 0.0F;
        foreach (Transform t in groundPoints)
        {
            RaycastHit2D groundRay = Physics2D.Raycast(t.position, -Vector2.up, 10.0F, groundLayer);
            //Debug.DrawRay(t.position, -Vector2.up * 0.05F);
            if (groundRay.collider != null)
            {
                float angle = Vector2.Angle(Vector2.up, groundRay.normal);
                if(groundRay.distance < distanceToGround)
                {
                    distanceToGround = groundRay.distance;
                    forceAngle = (Vector2.Angle(Vector2.right, groundRay.normal) - 90) * Mathf.Deg2Rad;
                }
                if (angle <= slopeAngleLimit && groundRay.distance <= 0.05F)
                {
                    isGrounded = true;
                    jumpTime = -1;
                }
            }

        }

        //Debug.Log(distanceToGround + ", " + isGrounded + ", " + forceAngle);


        float moveForce = 0.0F;
        float jumpForce = 0.0F;
        bool isLanding = animator.GetCurrentAnimatorStateInfo(0).IsName("landing_r")
                || animator.GetCurrentAnimatorStateInfo(0).IsName("landing_l");
        if (isGrounded)
        {
            if(!ledgeMode)
            {
                if (shouldJump)
                {
                    jumpForce = jumpStrength;
                    jumpTime = Time.time;
                }
                if (Mathf.Abs(rigidBody.velocity.x) < speedLimit)
                {
                    moveForce = horizontalAxis * acceleration;
                }
                if (horizontalAxis == 0 || Mathf.Sign(horizontalAxis) != Mathf.Sign(rigidBody.velocity.x))
                {
                    float horizontalVelocity = rigidBody.velocity.x;
                    float verticalVelocity = rigidBody.velocity.y;
                    rigidBody.velocity = new Vector2(Mathf.Lerp(horizontalVelocity, 0, friction * Time.fixedDeltaTime), Mathf.Lerp(verticalVelocity, 0, friction * Time.fixedDeltaTime));
                }

            }
            else
            {
                if (Input.GetAxisRaw("Vertical") < 0 || Input.GetButton("DimensionShift"))
                {
                    isGrounded = false;
                    ledgeMode = false;
                    rigidBody.simulated = true;
                }
                if (shouldJump && !jumped)
                {
                    jumpForce = jumpStrength;
                    isGrounded = false;
                    ledgeMode = false;
                    rigidBody.simulated = true;
                    jumped = true;
                }
            }
        }
        else
        {
            jumpForce = gravity;
            if (Mathf.Abs(rigidBody.velocity.x) <= airControl)
            {
                moveForce = horizontalAxis * airAcceleration;
            }
            float timeJumped = Time.time - jumpTime;
            if (jumpTime != -1 && timeJumped >= ledgeGrabTimeMin && timeJumped <= ledgeGrabTimeMax)
            {
                bool hitLedge = false;
                int jumpMode = 0;
                if (animator.GetCurrentAnimatorStateInfo(0).IsName("jump_r"))
                {
                    jumpMode = 1;
                }
                else if (animator.GetCurrentAnimatorStateInfo(0).IsName("jump_l"))
                {
                    jumpMode = 2;
                    ledgePoint.localPosition = new Vector3(ledgePoint.localPosition.x * -1, ledgePoint.localPosition.y, ledgePoint.localPosition.z);
                }
                //Debug.DrawRay(ledgePoint.position, -Vector2.up * ledgeGrabRange, Color.green);
                RaycastHit2D ledgeRayCast = Physics2D.Raycast(ledgePoint.position, -Vector2.up, ledgeGrabRange, groundLayer);
                if (ledgeRayCast.collider != null && ledgeRayCast.distance >= 0.01)
                {
                    hitLedge = true;
                }
                if(jumpMode == 2)
                {
                    ledgePoint.localPosition = new Vector3(ledgePoint.localPosition.x * -1, ledgePoint.localPosition.y, ledgePoint.localPosition.z);
                }
                if (hitLedge)
                {
                    rigidBody.MovePosition(ledgeRayCast.point - rigidBody.position);
                    rigidBody.simulated = false;
                    rigidBody.velocity = Vector2.zero;
                    isGrounded = true;
                    ledgeMode = true;
                    jumpTime = -1;
                }
            }
            

        }
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("jump_r") || animator.GetCurrentAnimatorStateInfo(0).IsName("jump_l"))
        {
            shouldJump = false;
        }
        isSkidding = moveForce * rigidBody.velocity.x < 0 && Mathf.Abs(rigidBody.velocity.x) >= skidAnimationSpeedCutoff;
        Vector2 force = new Vector2((Mathf.Cos(forceAngle)*moveForce), (Mathf.Sin(forceAngle) * moveForce));
        force += Vector2.up * jumpForce;
        //Debug.DrawRay(transform.position, force);
        rigidBody.velocity += force * Time.fixedDeltaTime;
        if (isLanding)
        {
            rigidBody.velocity = Vector2.zero;
        }

    }


	public bool GetFacingRight() {
		return facingRight;
	}
}
