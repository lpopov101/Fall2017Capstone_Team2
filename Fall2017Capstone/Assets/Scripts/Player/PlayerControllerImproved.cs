using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerImproved : MonoBehaviour {

    enum MovementState { GROUNDED, AIRBORNE, FALLING, LEDGE, LADDER, DYING };

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
    public float fallDistance = 1.0F;
    public float ledgeGrabRange = 2.0F;
    public float ledgeGrabTimeMin = 0.1F;
    public float ledgeGrabTimeMax = 0.5F;
    public Transform ledgePoint;
    public Transform[] groundPoints;
    public LayerMask groundLayer;
    public float deathBounceForce = 1.0F;
    public float ladderSpeed = 1.0F;

    private Rigidbody2D rigidBody;
    private Animator animator;
    private MovementState movementState;
    private bool shouldJump;
    private bool jumped;
    private bool isSkidding;
    private float initFallY;
    private float jumpTime;
	private bool facingRight;
    private bool deathInitPhysics;
    private bool deathAnimTriggered;
    private bool ladderAnimTriggered;
    private bool collidingWithLadder;
	private bool isDead;
	private bool cutscenePlaying;
    
	void Start ()
    {
        Physics2D.queriesStartInColliders = false;
        Mathf.Clamp(acceleration, 0, Mathf.Infinity);
        Mathf.Clamp(speedLimit, 0, Mathf.Infinity);
        Mathf.Clamp(jumpStrength, 0, Mathf.Infinity);
        Mathf.Clamp(airAcceleration, 0, Mathf.Infinity);
        Mathf.Clamp(airControl, 0, Mathf.Infinity);
        Mathf.Clamp(ledgeGrabRange, 0, Mathf.Infinity);
        Mathf.Clamp(sprintAnimationSpeedCutoff, 0, speedLimit);
        Mathf.Clamp(skidAnimationSpeedCutoff, 0, speedLimit);

        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        movementState = MovementState.GROUNDED;
        shouldJump = false;
        jumped = false;
        isSkidding = false;
        initFallY = 0;
        jumpTime = -1;
		facingRight = true;
        deathInitPhysics = false;
        deathAnimTriggered = false;
        ladderAnimTriggered = false;
        collidingWithLadder = false;
		isDead = false;
		cutscenePlaying = false;
	}

    void Update()
    {
        if (movementState == MovementState.GROUNDED || movementState == MovementState.LEDGE || movementState == MovementState.LADDER)
        {
			if (Input.GetButtonDown("Jump") && !FreezeMovement())
            {
                shouldJump = true;
                jumped = false;
            }
        }
        else if(movementState == MovementState.AIRBORNE)
        {
            if(transform.position.y <= initFallY - fallDistance)
            {
                movementState = MovementState.FALLING;
            }
        }

        animator.SetBool("grounded", movementState == MovementState.GROUNDED);
        animator.SetBool("airborne", movementState == MovementState.AIRBORNE);
        animator.SetBool("falling", movementState == MovementState.FALLING);
        animator.SetBool("ledge", movementState == MovementState.LEDGE);
        animator.SetBool("jump", shouldJump);
        animator.SetBool("isRunning", Mathf.Abs(rigidBody.velocity.x) >= sprintAnimationSpeedCutoff);
        animator.SetBool("skidding", isSkidding);
		animator.SetFloat("horizontalInput", FreezeMovement() ? 0 : Input.GetAxisRaw("Horizontal"));
		animator.SetFloat("verticalInput", FreezeMovement() ? 0 : Input.GetAxisRaw("Vertical"));
        animator.SetFloat("horizontalVelocity", rigidBody.velocity.x);
        animator.SetFloat("verticalVelocity", rigidBody.velocity.y);
        animator.SetFloat("jumpTime", Time.time - jumpTime);
        if(movementState == MovementState.DYING && !deathAnimTriggered)
        {
            animator.SetTrigger("death");
            deathAnimTriggered = true;
        }
        if(movementState == MovementState.LADDER && !ladderAnimTriggered)
        {
            animator.SetTrigger("ladder");
            ladderAnimTriggered = true;
        }
        if (movementState == MovementState.LADDER && rigidBody.velocity.y == 0)
        {
            animator.speed = 0;
        }
        else
        {
            animator.speed = 1;
        }

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
		float horizontalAxis = FreezeMovement() ? 0 : Input.GetAxisRaw("Horizontal");
        float distanceToGround = float.MaxValue;
        float forceAngle = 0.0F;
        if(movementState == MovementState.GROUNDED || movementState == MovementState.AIRBORNE || movementState == MovementState.FALLING || movementState == MovementState.LADDER)
        {
            if(movementState == MovementState.GROUNDED)
            {
                initFallY = transform.position.y;
                movementState = MovementState.AIRBORNE;
            }
            foreach (Transform t in groundPoints)
            {
                RaycastHit2D groundRay = Physics2D.Raycast(t.position, -Vector2.up, 10.0F, groundLayer);
                //Debug.DrawRay(t.position, -Vector2.up * 0.05F);
                if (groundRay.collider != null)
                {
                    float angle = Vector2.Angle(Vector2.up, groundRay.normal);
                    if (groundRay.distance < distanceToGround)
                    {
                        distanceToGround = groundRay.distance;
                        forceAngle = (Vector2.Angle(Vector2.right, groundRay.normal) - 90) * Mathf.Deg2Rad;
                    }
                    if (angle <= slopeAngleLimit && groundRay.distance <= 0.05F && (movementState != MovementState.LADDER || Input.GetAxisRaw("Vertical") < 0))
                    {
                        movementState = MovementState.GROUNDED;
                        ladderAnimTriggered = false;
                        jumpTime = -1;
                    }
                }

            }
            if(movementState != MovementState.LADDER && collidingWithLadder && Input.GetAxisRaw("Vertical") > 0)
            {
                movementState = MovementState.LADDER;
            }

            if(movementState != MovementState.LADDER && collidingWithLadder && Input.GetAxisRaw("Vertical") < 0 && movementState != MovementState.GROUNDED)
            {
                movementState = MovementState.LADDER;
            }
        }

        //Debug.Log(distanceToGround + ", " + isGrounded + ", " + forceAngle);


        float moveForce = 0.0F;
        float gravForce = 0.0F;
        bool jumpTrigger = false;
        bool isLanding = animator.GetCurrentAnimatorStateInfo(0).IsName("landing_r")
                || animator.GetCurrentAnimatorStateInfo(0).IsName("landing_l");
        if(movementState == MovementState.GROUNDED)
        {
            if (shouldJump)
            {
                jumpTrigger = true;
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
        else if (movementState == MovementState.AIRBORNE || movementState == MovementState.FALLING)
        {
            gravForce = gravity;
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
                Debug.DrawRay(ledgePoint.position, -Vector2.up * ledgeGrabRange, Color.green);
                RaycastHit2D ledgeRayCast = Physics2D.Raycast(ledgePoint.position, -Vector2.up, ledgeGrabRange, groundLayer);
                if (ledgeRayCast.collider != null && ledgeRayCast.distance >= 0.01)
                {
                    hitLedge = true;
                }
                if (jumpMode == 2)
                {
                    ledgePoint.localPosition = new Vector3(ledgePoint.localPosition.x * -1, ledgePoint.localPosition.y, ledgePoint.localPosition.z);
                }
                if (hitLedge)
                {
                    transform.position = new Vector2(transform.position.x, ledgeRayCast.point.y - 0.6F);
                    rigidBody.simulated = false;
                    rigidBody.velocity = Vector2.zero;
                    movementState = MovementState.LEDGE;
                    jumpTime = -1;
                }
            }
        }
        else if(movementState == MovementState.LADDER)
        {
            if(!collidingWithLadder)
            {
                movementState = MovementState.AIRBORNE;

                ladderAnimTriggered = false;
            }
            if(shouldJump && !jumped)
            {
                jumpTrigger = true;
                movementState = MovementState.AIRBORNE;
                ladderAnimTriggered = false;
                jumped = true;
            }
        }
        else if(movementState == MovementState.LEDGE)
        {
            if (Input.GetAxisRaw("Vertical") < 0 || Input.GetButton("DimensionShift"))
            {
                movementState = MovementState.AIRBORNE;
                rigidBody.simulated = true;
            }
            if (shouldJump && !jumped)
            {
                jumpTrigger = true;
                movementState = MovementState.AIRBORNE;
                rigidBody.simulated = true;
                jumped = true;
            }
        }
        else if(movementState == MovementState.DYING)
        {
            bool deathGrounded = false;
            foreach (Transform t in groundPoints)
            {
                RaycastHit2D groundRay = Physics2D.Raycast(t.position + (Vector3.up*0.5F), -Vector2.up, 10.0F, groundLayer);
                Debug.DrawRay(t.position + (Vector3.up * 0.5F), -Vector2.up);
                if (groundRay.collider != null)
                {
                    if (groundRay.distance <= 0.05F)
                    {
                        deathGrounded = true;
                    }
                }
            }
            //Debug.Log(deathGrounded);
            if(deathGrounded)
            {
                float horizontalVelocity = rigidBody.velocity.x;
                float verticalVelocity = rigidBody.velocity.y;
                rigidBody.velocity = new Vector2(Mathf.Lerp(horizontalVelocity, 0, friction * Time.fixedDeltaTime), Mathf.Lerp(verticalVelocity, 0, friction * Time.fixedDeltaTime));
            }
            gravForce = gravity;
        }
        
        if(animator.GetCurrentAnimatorStateInfo(0).IsName("jump_r") || animator.GetCurrentAnimatorStateInfo(0).IsName("jump_l"))
        {
            shouldJump = false;
        }
        isSkidding = Mathf.Sign(Input.GetAxisRaw("Horizontal")) != Mathf.Sign(rigidBody.velocity.x) && Mathf.Abs(rigidBody.velocity.x) >= skidAnimationSpeedCutoff;
        Vector2 force = new Vector2((Mathf.Cos(forceAngle)*moveForce), (Mathf.Sin(forceAngle) * moveForce));
        force += Vector2.up * gravForce;
        //Debug.DrawRay(transform.position, force);
        rigidBody.velocity += force * Time.fixedDeltaTime;
        if(jumpTrigger)
        {
            rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpStrength);
        }
        if(deathInitPhysics)
        {
            CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
            collider.offset = new Vector2(0, 3.5F);
            rigidBody.position += Vector2.up * 0.04F;
            rigidBody.velocity = new Vector2(-Mathf.Sign(rigidBody.velocity.x)*deathBounceForce, deathBounceForce);
            deathInitPhysics = false;
        }
        if (isLanding)
        {
            rigidBody.velocity = Vector2.zero;
        }
        if(movementState == MovementState.LADDER)
        {
			rigidBody.velocity = new Vector2(0, FreezeMovement() ? 0 : Input.GetAxis("Vertical") * ladderSpeed);
        }

    }


	public bool GetFacingRight() {
		return facingRight;
	}

    public void KillPlayer()
    {
		isDead = true;
        if(movementState != MovementState.DYING)
        {
            movementState = MovementState.DYING;
            deathInitPhysics = true;
        }
    }

    void LadderEnter()
    {
        collidingWithLadder = true;
    }

    void LadderExit()
    {
        collidingWithLadder = false;
    }

	public void SetCutscenePlaying(bool playing) {
		cutscenePlaying = playing;
	}

	public bool FreezeMovement() {
		return isDead || cutscenePlaying;
	}
}
