using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float GroundSpeed;
    public float AirSpeed; // TODO not yet implemented
    public float jumpForce;
    public float gravityScale;
	public int treasureColleceted;

	[Header ("Lives")]
	public int currentlives;
	public int maxlives;
	public int minlives;

	[Header ("UI Elements")]

	public Text livesText;


    public CharacterController controller;

    private Vector3 moveDirection;

    // States
    private Dictionary<EPlayerStates, PlayerState> states;
    private PlayerState currentState;

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

	}
	
	// Update is called once per frame
	void Update ()
    { 
        float yStore = moveDirection.y;
        //Debug.Log("vert: " + Input.GetAxis("Vertical") + " horiz: " + Input.GetAxis("Horizontal"));
        moveDirection = (transform.forward * Input.GetAxis("Vertical")) + (transform.right * Input.GetAxis("Horizontal"));
        //moveDirection = moveDirection.normalized * moveSpeed;
        moveDirection = Vector3.ClampMagnitude(moveDirection, 1f) * GroundSpeed;
        moveDirection.y = yStore;

        if (controller.isGrounded)
        {
            moveDirection.y = 0f;
            if (Input.GetButtonDown("Jump"))
            {
                moveDirection.y = jumpForce;
            }
        }

        moveDirection.y = moveDirection.y + (Physics.gravity.y * gravityScale * Time.deltaTime);

        controller.Move(moveDirection * Time.deltaTime);

		livesText.text = "Lives Remaining: " + currentlives + " / " + maxlives ;
    }
}
