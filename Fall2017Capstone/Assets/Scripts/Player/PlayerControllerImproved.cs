using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerImproved : MonoBehaviour {

    enum MovementState { GROUNDED, AIRBORNE, FALLING, LEDGE, DYING };

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
    public float dodgeForce = 200.0F;
    public GameObject silhouettePrefab;
    public float silhouetteRate;
	public AudioClip StepAudio1;
	public AudioClip StepAudio2;
	public AudioClip LandAudio;
	public bool canJump = true;

	private int stepCount;
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
    private bool dodgeTriggered;
    private float silhouetteTime;
    private Queue<GameObject> silhouettes;
	private bool cutscenePlaying;
	private bool isDead;
	private bool highJumpTriggered;
	private bool highJumping;
    
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
        jumpTime = 0;
		facingRight = true;
        deathInitPhysics = false;
        deathAnimTriggered = false;
        dodgeTriggered = false;
        silhouetteTime = -1;
        silhouettes = new Queue<GameObject>();
		cutscenePlaying = false;
		isDead = false;
		stepCount = 0;
		highJumpTriggered = false;
		highJumping = false;
	}

    void Update()
    {
        
        if (1==1)//movementState == MovementState.GROUNDED || movementState == MovementState.LEDGE)
        {
            if ((Input.GetButtonDown("Jump") || TouchInput.Jump) && !FreezeMovement() && canJump)
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

        if(jumped)
        {
            animator.SetBool("jump", true);
            jumpTime = Time.time;
            jumped = false;
        }

        if (animator.GetCurrentAnimatorStateInfo(0).IsName("jump_r") || animator.GetCurrentAnimatorStateInfo(0).IsName("jump_l"))
        {
            animator.SetBool("jump", false);
        }


        animator.SetBool("grounded", movementState == MovementState.GROUNDED);
        animator.SetBool("airborne", movementState == MovementState.AIRBORNE);
        animator.SetBool("falling", movementState == MovementState.FALLING);
        animator.SetBool("ledge", movementState == MovementState.LEDGE);
        animator.SetBool("isRunning", Mathf.Abs(rigidBody.velocity.x) >= sprintAnimationSpeedCutoff);
        animator.SetBool("skidding", isSkidding);
		animator.SetFloat("horizontalInput", FreezeMovement() ? 0 : Input.GetAxisRaw("Horizontal") + TouchInput.Horizontal);
		animator.SetFloat("verticalInput", FreezeMovement() ? 0 : Input.GetAxisRaw("Vertical") + TouchInput.Vertical);
        animator.SetFloat("horizontalVelocity", rigidBody.velocity.x);
        animator.SetFloat("verticalVelocity", rigidBody.velocity.y);
        animator.SetFloat("jumpTime", Time.time - jumpTime);
        //Debug.Log("" + animator.GetFloat("jumpTime") + "/" + (Time.time - jumpTime));
        if(movementState == MovementState.DYING && !deathAnimTriggered)
        {
            animator.SetTrigger("death");
            deathAnimTriggered = true;
        }

        // Calculate facingRight boolean
        float x = Mathf.Abs(rigidBody.velocity.x);
		if(x > 0.01f) { // Make sure the player isn't standing virtually still
			if(facingRight && rigidBody.velocity.x < 0)
				facingRight = false;
			else if(!facingRight && rigidBody.velocity.x > 0)
				facingRight = true;
		}

        if (Mathf.Abs(rigidBody.velocity.x) > speedLimit + 0.5F)
        {
            if(silhouetteTime == -1)
            {
                silhouetteTime = Time.time - silhouetteRate;
            }
            if(Time.time - silhouetteTime >= silhouetteRate)
            {
                silhouetteTime = Time.time;
                GameObject silhouette = Instantiate(silhouettePrefab);
                silhouette.transform.position = transform.position;
                silhouette.transform.localScale = transform.localScale;
                SpriteRenderer renderer = silhouette.GetComponent<SpriteRenderer>();
                renderer.sprite = GetComponent<SpriteRenderer>().sprite;
                renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, 0.5F);
                silhouettes.Enqueue(silhouette);
            }
        }
        else
        {
            silhouetteTime = -1;
        }
        //Debug.Log(silhouetteTime);
		foreach(GameObject silhouette in silhouettes.ToArray())
        {
            SpriteRenderer renderer = silhouette.GetComponent<SpriteRenderer>();
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, renderer.color.a - (0.35F*Time.deltaTime));
            if(renderer.color.a <= 0)
            {
                silhouettes.Dequeue();
				Destroy(silhouette, 0.1f);
            }
        }
    }

    void FixedUpdate () {
		float horizontalAxis = FreezeMovement() ? 0 : Input.GetAxisRaw("Horizontal") + TouchInput.Horizontal;
        float distanceToGround = float.MaxValue;
        float forceAngle = 0.0F;
        bool wasAirborne = true;
        if(movementState == MovementState.GROUNDED || movementState == MovementState.AIRBORNE || movementState == MovementState.FALLING)
        {
            if(movementState == MovementState.GROUNDED)
            {
                wasAirborne = false;
                initFallY = transform.position.y;
                movementState = MovementState.AIRBORNE;
            }
            float angle = 10000;
            foreach (Transform t in groundPoints)
            {
                RaycastHit2D groundRay = Physics2D.Raycast(t.position, -Vector2.up, 10.0F, groundLayer);
                if (groundRay.collider != null)
                {
                    if (groundRay.distance < distanceToGround)
                    {
                        distanceToGround = groundRay.distance;
                        angle = Vector2.Angle(Vector2.up, groundRay.normal);
                        forceAngle = (Vector2.Angle(Vector2.right, groundRay.normal) - 90) * Mathf.Deg2Rad;
                    }
                }

            }

            if (angle <= slopeAngleLimit && distanceToGround <= 0.1F)
            {
                movementState = MovementState.GROUNDED;
                if(wasAirborne && horizontalAxis == 0)
                {
                    rigidBody.velocity = new Vector2(0, rigidBody.velocity.y);
                }
            }

        }
       


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
                jumped = true;
                shouldJump = false;
            }
            if (Mathf.Abs(rigidBody.velocity.x) <= speedLimit)
            {
                moveForce = horizontalAxis * acceleration;
            }
            else if (Mathf.Abs(rigidBody.velocity.x) > speedLimit)
            {
                moveForce -= horizontalAxis * acceleration;
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
            if(shouldJump)
            {
                shouldJump = false;
            }
            gravForce = gravity;
            if (Mathf.Abs(rigidBody.velocity.x) <= airControl || Mathf.Sign(rigidBody.velocity.x) != Mathf.Sign(horizontalAxis))
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
                }
            }
        }
        else if(movementState == MovementState.LEDGE)
        {
            if (Input.GetAxisRaw("Vertical") + TouchInput.Vertical < 0 || (Input.GetButton("DimensionShift") || TouchInput.Shift))
            {
                movementState = MovementState.AIRBORNE;
                rigidBody.simulated = true;
                jumpTime = Time.time;
            }
            if (shouldJump)
            {
                jumpTrigger = true;
                movementState = MovementState.AIRBORNE;
                rigidBody.simulated = true;
                jumpTime = Time.time;
                shouldJump = false;
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
        
        isSkidding = Mathf.Sign(Input.GetAxisRaw("Horizontal") + TouchInput.Horizontal) != Mathf.Sign(rigidBody.velocity.x) && Mathf.Abs(rigidBody.velocity.x) >= skidAnimationSpeedCutoff;
        Vector2 force = new Vector2((Mathf.Cos(forceAngle)*moveForce), (Mathf.Sin(forceAngle) * moveForce));
        if(movementState == MovementState.AIRBORNE || movementState == MovementState.DYING)
        {
            force += Vector2.up * gravForce;
        }
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
		if(dodgeTriggered) // NOTE: No longer called; dodge is done in DodgeScript instead
        {
			Vector2 velocity = rigidBody.velocity;
			rigidBody.velocity = new Vector2(dodgeForce*horizontalAxis, 0);
            dodgeTriggered = false;
        }
		if(highJumpTriggered) { // If jump is triggered by input
			if(!highJumping && !FreezeMovement()) {
				bool jumped2 = jumpTime != -1 && Time.time > jumpTime + 0.02f;
				bool noJump = jumpTime == -1 && movementState == MovementState.AIRBORNE;
				if(jumped2 || noJump) {
					rigidBody.velocity = new Vector2(rigidBody.velocity.x, jumpStrength);
					highJumping = true;
				}
			}
			highJumpTriggered = false;
		}
		if(highJumping && movementState != MovementState.AIRBORNE && movementState != MovementState.FALLING) {
			highJumping = false;
		}

    }


	public bool GetFacingRight() {
		return facingRight;
	}

    void KillPlayer()
    {
		isDead = true;
        if(movementState != MovementState.DYING)
        {
            movementState = MovementState.DYING;
            deathInitPhysics = true;
        }
    }

    void DimensionShift()
    {
        movementState = MovementState.AIRBORNE;
        rigidBody.simulated = true;
        initFallY = transform.position.y;
    }

	public void SetCutscenePlaying(bool playing) {
		cutscenePlaying = playing;
	}

	public bool FreezeMovement() {
		return isDead || cutscenePlaying;
	}

	/* NOTE: No longer called; dodge is done in DodgeScript instead */
    void Dodge()
    {
        dodgeTriggered = true;
    }

	void HighJump()
	{
		highJumpTriggered = true;
	}

	void StepSound() {
		if (stepCount == 0) {
			AudioSource.PlayClipAtPoint (StepAudio1, Camera.main.transform.position, 3f);
			stepCount = 1;
		} else {
			AudioSource.PlayClipAtPoint (StepAudio1, Camera.main.transform.position, 3f);
			stepCount = 0;
		}

	}

	void LandSound() {
		AudioSource.PlayClipAtPoint(LandAudio, Camera.main.transform.position, 4f);
	}
}
