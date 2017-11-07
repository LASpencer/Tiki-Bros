using System.Collections;
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

    public float FootRadius = 0.3f;
    public float FootOffset = 0.15f;
    
    private float jumpVelocity;     // Initial velocity at start of jump
    private float jumpCutoffVelocity;  // Impulse applied when cutting off jump

    public float JumpVelocity { get { return jumpVelocity; } }
    public float JumpCutoffVelocity { get { return jumpCutoffVelocity; } }
    [Header("Jumping")]
    public float MinJumpHeight;
    public float MaxJumpHeight;
    public float JumpChargeTime;
    public float JumpCutoffProportion = 0;  // Proportion of remaining velocity kept when cutting off jump

    public float CoyoteTime;
    public float JumpPressTolerance;

	[Header ("Lives")]
	public int currentlives;
	public int maxlives;
	public int minlives;

	[Header ("UI Elements")]

	public Text livesText;


    public CharacterController controller;
    public Animator animator;
    public CameraController PlayerCamera;
    public Transform CameraTarget;
    public Vector3 CameraTargetOffset;
    public Renderer ModelRenderer;

    public Vector3 velocity;

    public LevelManager level;

    // States
    private Dictionary<EPlayerStates, PlayerState> states;
    public PlayerState currentState;
    public EPlayerStates stateName; //HACK

    
    bool isGrounded = false;

    public bool IsGrounded { get { return isGrounded; } }

    // Returns bounds around player's mesh
    public Bounds bounds { get { return ModelRenderer.bounds; } }

	// Use this for initialization
	void Start () {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();

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

        if (!level.IsPaused)
        {
            currentState.CheckTransition();
            currentState.Update();

            // Apply gravity
            velocity.y += Physics.gravity.y * gravityScale * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);

            CheckIfGrounded();

            // TODO rotate to movement direction
            // TODO: rotation should be more smooth
            // TODO: maybe target has some offset?
            Vector3 moveDirection = new Vector3(velocity.x, 0, velocity.z);
            Vector3 targetVelocity = GetTargetVelocity();
            if (targetVelocity.magnitude != 0)
            {
                transform.forward = targetVelocity;
            }
            CameraTarget.transform.position = transform.position + CameraTargetOffset;
        }

		livesText.text = "LIVES: " + currentlives + " / " + maxlives ;
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

        // Cutoff jump version

        jumpVelocity = Mathf.Sqrt(2.0f * MaxJumpHeight * g);
        float minJumpVelocity = Mathf.Sqrt(2.0f * MinJumpHeight * g);
        jumpCutoffVelocity = minJumpVelocity - jumpVelocity;
    }

    public Vector3 GetTargetVelocity()
    {
        //HACK transform.forward may need to change once character isn't just following camera angle
        Vector3 forward = PlayerCamera.transform.forward;
        forward.y = 0;
        forward.Normalize();
        if(forward.magnitude == 0)
        {

        }
        Vector3 inputDirection = CameraTarget.transform.forward * Input.GetAxis("Vertical") + CameraTarget.transform.right * Input.GetAxis("Horizontal");
        Vector3 targetVelocity = Vector3.ClampMagnitude(inputDirection, 1.0f) * GroundSpeed;
        return targetVelocity;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Vector3 normal = hit.normal;
        
        float mag = Vector3.Dot(normal, velocity);
        velocity += -mag * normal;
        //TODO: If hitting a wall greater than walkable slope, move away from it
        //if (Vector3.Angle(normal, Vector3.up) > (controller.slopeLimit) && Vector3.Angle(normal, Vector3.up) < (180 - controller.slopeLimit))
        //{
        //    //velocity += normal;
        //    //TODO
        //}
    }

    void CheckIfGrounded()
    {
        //TODO write test based on ground distance directly below
        RaycastHit groundHit;
        Debug.DrawRay(transform.position, Vector3.down, Color.green);

        if (Physics.SphereCast(transform.position + ((FootRadius + FootOffset) * Vector3.up), FootRadius, Vector3.down, out groundHit, FootRadius))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        //isGrounded = controller.isGrounded;
    }
}
