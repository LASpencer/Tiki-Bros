using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//TODO change how Jump Tolerance works, so not jumping while off ground

public class PlayerController : MonoBehaviour
{
    [Tooltip("Maximum speed character moves along ground")]
    public float GroundSpeed; 
    [Tooltip("Acceleration applied while in air")]
    public float AirAcceleration;
    [Tooltip("Acceleration applied while running")]
    public float GroundAcceleration;
    public float gravityScale;
    public float drowningGravityScale;
    [Tooltip("Extra acceleration for stopping or turning around")]
    public float BrakingAcceleration;
	public int treasureCollected;

    [Tooltip("Radius of spherecast to check grounding")]
    public float FootRadius = 0.3f;
    [Tooltip("Distance above ground bottom of sphere starts at")]
    public float FootOffset = 0.15f;
    
    private float jumpVelocity;     // Initial velocity at start of jump
    private float jumpCutoffVelocity;  // Impulse applied when cutting off jump

    public float JumpVelocity { get { return jumpVelocity; } }
    public float JumpCutoffVelocity { get { return jumpCutoffVelocity; } }
    [Header("Jumping")]
    [Tooltip("Jump height if button tapped for only one frame")]
    public float MinJumpHeight;
    [Tooltip("Jump height when button held until top reached")]
    public float MaxJumpHeight;
    [Tooltip("Proportion of velocity retained when jump button released")]
    public float JumpCutoffProportion = 0;  // Proportion of remaining velocity kept when cutting off jump

    [Tooltip("Time player can still jump after beginning to fall")]
    public float CoyoteTime;
    [Tooltip("Time before landing in which a jump press will be accepted")]
    public float JumpPressTolerance;

    [Header("Punching")]
    public HitboxController Hitbox;
    [Tooltip("Total time in punch state")]
    public float PunchTime = 0.5f;
    [Tooltip("Time it takes for punch hitbox to activate")]
    public float PunchWindup = 0.2f;
    [Tooltip("Time before another punch can be made")]
    public float PunchCooldownTime = 0.1f;
    [Tooltip("Speed moved during punch")]
    public float PunchMoveSpeed = 3.0f;

    [HideInInspector]
    public float PunchCooldown = 0.0f;


	[Header ("Lives")]
	public int currentlives;
	public int maxlives;
	public int minlives;

    public float DeathTime;

	[Header ("UI Elements")]

	public Text livesText;


    public CharacterController controller;
    public Animator animator;
    public CameraController PlayerCamera;
    public Transform CameraTarget;
    [Tooltip("Position of camera target relative to player")]
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

    [HideInInspector]
    public bool IsDead = false;

    [HideInInspector]
    public bool Invincible = false;

    // Returns bounds around player's mesh
    public Bounds bounds { get { return ModelRenderer.bounds; } }

    [HideInInspector]
    public bool CameraFollows = true; //HACK might only use until camera redone

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
        states.Add(EPlayerStates.Punching, new PunchingState(this));
        states.Add(EPlayerStates.CombatDeath, new CombatDeathState(this));
        states.Add(EPlayerStates.Drowning, new DrowiningState(this));

        currentState = states[EPlayerStates.Idle];
        stateName = EPlayerStates.Idle;

        velocity = new Vector3();

        // Disable hitbox
        Hitbox.gameObject.SetActive(false);
        Invincible = false;
	}
	
	// Update is called once per frame
	void Update ()
    {

        if (!level.IsPaused)
        {
            currentState.CheckTransition();
            currentState.Update();

            // Run timers
            PunchCooldown = Mathf.Max(0.0f, PunchCooldown - Time.deltaTime);

            // Apply gravity
            velocity.y += Physics.gravity.y * gravityScale * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);

            CheckIfGrounded();
            animator.SetBool("isGrounded", isGrounded);

            // TODO: maybe target has some offset?
            Vector3 moveDirection = new Vector3(velocity.x, 0, velocity.z);

            // TODO: change how camera position controlled
            // Make CameraController responsible for actually moving CameraTarget, and just tell it our position + offset
            if (CameraFollows)
            {
                CameraTarget.transform.position = transform.position + CameraTargetOffset;
            }
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

        if (Physics.SphereCast(transform.position + ((FootRadius + FootOffset) * Vector3.up), FootRadius, Vector3.down, out groundHit, FootRadius, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
        //isGrounded = controller.isGrounded;
    }

    public bool Damage(Vector3 knockbackDirection)
    {
        if (!IsDead && !Invincible)
        {
            //TODO change to CombatDeath state
            currentlives -= 1;
            //HACK make proper field
            velocity = knockbackDirection * 2.0f;
            ChangeState(EPlayerStates.CombatDeath);
            return true;
        } else
        {
            return false;
        }
    }


    public void EnterKillzone()
    {
        if (!IsDead)
        {
            //TODO change to KillzoneDeath state
            currentlives -= 1;
            //TODO respawning happens in Dying state
            ChangeState(EPlayerStates.Drowning);
        }
    }
}
