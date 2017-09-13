using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerImproved : MonoBehaviour {
    
    public float acceleration = 3.5F;
    public float speedLimit = 2.0F;
    public float sprintAnimationSpeedCutoff = 1.5F;
    public float skidAnimationSpeedCutoff = 6.0F;
    public float jumpStrength = 600.0F;
    public float airAcceleration = 10F;
    public float airControl = 1.0F;
    public float fallAnimationTriggerTime = 0.3F;
    public float ledgeGrabRange = 2.0F;
    public float ledgeGrabYVelocityMin = -0.3F;
    public Transform ledgePoint;
    public Transform groundPoint;
    public LayerMask groundLayer;

    private Rigidbody2D rigidBody;
    private Animator animator;
    private bool isGrounded;
    private bool shouldJump;
    private bool isSkidding;
    private bool ledgeMode;
    private float fallTime;
    
	void Start ()
    {
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
        isSkidding = false;
        ledgeMode = false;
        fallTime = -1;
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
    }

    void FixedUpdate () {
        float horizontalAxis = Input.GetAxisRaw("Horizontal");
        isGrounded = Physics2D.OverlapCircle(groundPoint.position, 0.2F, groundLayer) || ledgeMode;


        float moveForce = 0.0F; 
        float jumpForce = 0.0F;
        bool isLanding = animator.GetCurrentAnimatorStateInfo(0).IsName("landing_r")
                || animator.GetCurrentAnimatorStateInfo(0).IsName("landing_l");
        if (isGrounded)
        {
            if(!ledgeMode)
            {
                if (shouldJump && !isLanding)
                {
                    jumpForce = jumpStrength;
                }
                if (Mathf.Abs(rigidBody.velocity.x) < speedLimit && !isLanding)
                {
                    moveForce = horizontalAxis * acceleration;
                }
            }
            else
            {
                if (Input.GetAxisRaw("Vertical") < 0)
                {
                    isGrounded = false;
                    ledgeMode = false;
                    rigidBody.simulated = true;
                }
                if (shouldJump)
                {
                    jumpForce = jumpStrength;
                    isGrounded = false;
                    ledgeMode = false;
                    rigidBody.simulated = true;
                }
            }
        }
        else
        {
            if (Mathf.Abs(rigidBody.velocity.x) <= airControl)
            {
                moveForce = horizontalAxis * airAcceleration;
            }

            if(Input.GetAxisRaw("Vertical") > 0 && rigidBody.velocity.y > ledgeGrabYVelocityMin)
            {
                bool hitLedge = false;
                Debug.DrawRay(ledgePoint.position, -Vector2.up * ledgeGrabRange, Color.green);
                RaycastHit2D ledgeRayCast = Physics2D.Raycast(ledgePoint.position, -Vector2.up, ledgeGrabRange, groundLayer);
                if (ledgeRayCast.collider != null)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("jump_r"))
                    {
                        hitLedge = true;
                    }
                }
                ledgePoint.localPosition = new Vector3(ledgePoint.localPosition.x * -1, ledgePoint.localPosition.y, ledgePoint.localPosition.z);
                ledgeRayCast = Physics2D.Raycast(ledgePoint.position, -Vector2.up, ledgeGrabRange, groundLayer);
                Debug.DrawRay(ledgePoint.position, -Vector2.up * ledgeGrabRange, Color.green);
                if (ledgeRayCast.collider != null)
                {
                    if (animator.GetCurrentAnimatorStateInfo(0).IsName("jump_l"))
                    {
                        hitLedge = true;
                    }
                }
                ledgePoint.localPosition = new Vector3(ledgePoint.localPosition.x * -1, ledgePoint.localPosition.y, ledgePoint.localPosition.z);
                if (hitLedge)
                {
                    rigidBody.MovePosition(ledgeRayCast.point - rigidBody.position);
                    rigidBody.simulated = false;
                    rigidBody.velocity = Vector2.zero;
                    isGrounded = true;
                    ledgeMode = true;
                }
            }
            

        }
        shouldJump = false;
        isSkidding = moveForce * rigidBody.velocity.x < 0 && Mathf.Abs(rigidBody.velocity.x) >= skidAnimationSpeedCutoff;
        Vector2 force = new Vector2(moveForce, jumpForce);
        rigidBody.AddForce(force);
        
    }


}
