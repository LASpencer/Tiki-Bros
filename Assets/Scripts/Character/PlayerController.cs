﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//TODO: On head colliding with roof (or anything above?) set vertical speed to 0

public class PlayerController : MonoBehaviour
{
    public float GroundSpeed; 
    public float AirAcceleration;
    public float GroundAcceleration;
    public float gravityScale;
    public float Friction;
	public int treasureCollected;

    private float jumpVelocity;     // Initial velocity at start of jump
    private float jumpHoldForce;    // Force applied while holding jump
    private float jumpCutoffVelocity;  // Impulse applied when cutting off jump

    public float JumpVelocity { get { return jumpVelocity; } }
    public float JumpHoldForce { get { return jumpHoldForce; } }
    public float JumpCutoffVelocity { get { return jumpCutoffVelocity; } }

    public float MinJumpHeight;
    public float MaxJumpHeight;
    public float JumpChargeTime;
    public float JumpCutoffProportion = 0;  // Proportion of remaining velocity kept when cutting off jump

	[Header ("Lives")]
	public int currentlives;
	public int maxlives;
	public int minlives;

	[Header ("UI Elements")]

	public Text livesText;


    public CharacterController controller;

    public Vector3 velocity;

    // States
    private Dictionary<EPlayerStates, PlayerState> states;
    public PlayerState currentState;
    public EPlayerStates stateName; //HACK

	// Use this for initialization
	void Start () {
        controller = GetComponent<CharacterController>();

        // Set up player states
        states = new Dictionary<EPlayerStates, PlayerState>();
        states.Add(EPlayerStates.Idle, new IdleState(this));
        states.Add(EPlayerStates.Run, new RunState(this));
        states.Add(EPlayerStates.Jump, new JumpState(this));
        states.Add(EPlayerStates.Falling, new FallingState(this));

        currentState = states[EPlayerStates.Idle];
        stateName = EPlayerStates.Idle;

        velocity = new Vector3();
	}
	
	// Update is called once per frame
	void Update ()
    {
        //float yStore = moveDirection.y;
        ////Debug.Log("vert: " + Input.GetAxis("Vertical") + " horiz: " + Input.GetAxis("Horizontal"));
        //moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
        ////moveDirection = moveDirection.normalized * moveSpeed;
        //moveDirection = Vector3.ClampMagnitude(moveDirection, 1f) * GroundSpeed;
        //moveDirection.y = yStore;

        //if (controller.isGrounded)
        //{
        //    moveDirection.y = 0f;
        //    if (Input.GetButtonDown("Jump"))
        //    {
        //        moveDirection.y = jumpForce;
        //    }
        //}

        //moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale * Time.deltaTime);

        //controller.Move(moveDirection * Time.deltaTime);

        currentState.CheckTransition();
        currentState.Update();

        // Apply gravity
        velocity.y += Physics.gravity.y * gravityScale * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
        
        if (controller.isGrounded)
        {
            velocity.y = 0f;
        }

		livesText.text = "Lives Remaining: " + currentlives + " / " + maxlives ;
    }

    public void ChangeState(EPlayerStates state)
    {
        stateName = state; //HACK may want to just save stateName and not the actual state
        currentState.OnExit();
        currentState = states[state];
        currentState.OnEnter();
    }

    public void CalculateJumpParameters()
    {
        float g = Physics.gravity.magnitude * gravityScale;

        // Jetpack jump version
        
        //float t = JumpChargeTime;
        //jumpVelocity = Mathf.Sqrt(2.0f * MinJumpHeight * g);
        //// HACK equation is still wrong
        //float discriminant = 4.25f * Mathf.Pow(g, 2) * Mathf.Pow(t, 4) + 8 * g * MaxJumpHeight * Mathf.Pow(t, 2) - 6 * JumpVelocity * g * Mathf.Pow(t, 3);
        //// TODO check discriminant positive, figure out other checks
        //jumpHoldForce = (1.5f * g * Mathf.Pow(t, 2) - 2 * JumpVelocity * t + Mathf.Sqrt(discriminant)) / (2 * Mathf.Pow(t,2));

        // Cutoff jump version

        jumpVelocity = Mathf.Sqrt(2.0f * MaxJumpHeight * g);
        float minJumpVelocity = Mathf.Sqrt(2.0f * MinJumpHeight * g);
        jumpCutoffVelocity = minJumpVelocity - jumpVelocity;
    }

    public Vector3 GetTargetVelocity()
    {
        //HACK transform.forward may need to change once character isn't just following camera angle
        Vector3 inputDirection = transform.forward * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal");
        Vector3 targetVelocity = Vector3.ClampMagnitude(inputDirection, 1.0f) * GroundSpeed;
        return targetVelocity;
    }
}
