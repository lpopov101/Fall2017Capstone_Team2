using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdScript : MonoBehaviour {
    public float triggerDistance;
    public float flySpeed;

    private Animator animator;
    private Transform playerTransform;
    private int flyDir;
    private bool flyMode;
    private float flyCoefficent;
	// Use this for initialization
	void Start () {
        animator = GetComponent<Animator>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        if (Random.value > 0.5f)
        {
            flyDir = 1;
        }
        else
        {
            flyDir = -1;
            GetComponent<SpriteRenderer>().flipX = true;
        }
        flyMode = false;
        flyCoefficent = (Random.value / 2.0F) + 0.5F;
    }
	
	// Update is called once per frame
	void Update () {
		if(Vector2.Distance(playerTransform.position, transform.position) <= triggerDistance && !flyMode)
        {
            animator.SetTrigger("fly");
            flyMode = true;
        }
        if(flyMode)
        {
            if (flyDir == 1)
            {
                transform.Translate(flySpeed * Time.deltaTime * flyCoefficent, flySpeed * Time.deltaTime * (1.0F/flyCoefficent), 0);
            }
            else if (flyDir == -1)
            {
                transform.Translate(-flySpeed * Time.deltaTime * flyCoefficent, flySpeed * Time.deltaTime * (1.0F / flyCoefficent), 0);
            }
        }
	}
}
