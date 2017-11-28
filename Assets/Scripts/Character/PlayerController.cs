using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Controls player behaviour. This class has fields for altering how the player
/// moves, methods for other objects interacting with the player, and a state 
/// machine for controlling the player's actions in response to input.
/// </summary>
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
    [Tooltip("Speed when hit by enemy")]
    public float KnockbackSpeed = 5.0f;
    [Tooltip("Length of time player is knocked back by enemy hit before stopping")]
    public float KnockbackTime;

    [Tooltip("Particle effect spawned on player death")]
    public GameObject DeathEffect;

    [HideInInspector]
    public float PunchCooldown = 0.0f;  // Counts down from last punch made


	[Header ("Lives")]
	public int currentlives;
	public int maxlives;
	public int minlives;

    [Tooltip("Time spent in Drowning state before respawning")]
    public float DrowningDeathTime;
    [Tooltip("Time spent in Combat Death state before respawning")]
    public float CombatDeathTime;


	[Header ("UI Elements")]

	public Text livesText;

    [Header("Audio")]
    public PlayerSounds sounds;
    public AudioSource audioSource;

    [Header ("")]
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
    public EPlayerStates stateName; // Used to show the current state in the editor

    
    bool isGrounded = false;

    public bool IsGrounded { get { return isGrounded; } }

    [HideInInspector]
    public bool IsDead = false;

    [HideInInspector]
    public bool Invincible = false;

    [HideInInspector]
    public bool PlayFootsteps = true;

    // Returns bounds around player's mesh
    public Bounds bounds { get { return ModelRenderer.bounds; } }

    [HideInInspector]
    public bool CameraFollows = true; 

	// Use this for initialization
	void Start () {
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();

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
            // Update state
            currentState.CheckTransition();
            currentState.Update();

            // Run timers
            PunchCooldown = Mathf.Max(0.0f, PunchCooldown - Time.deltaTime);

            // Apply gravity
            velocity.y += Physics.gravity.y * gravityScale * Time.deltaTime;

            controller.Move(velocity * Time.deltaTime);

            CheckIfGrounded();
            animator.SetBool("isGrounded", isGrounded);
        }

		livesText.text = "LIVES: " + currentlives + " / " + maxlives ;
    }

    /// <summary>
    /// Exits the current state and enters a new state
    /// </summary>
    /// <param name="state">Key of new state wanted</param>
    public void ChangeState(EPlayerStates state)
    {
        stateName = state; 
        currentState.OnExit();
        currentState = states[state];
        currentState.OnEnter();
    }

    /// <summary>
    /// Based on MaxJumpHeight and MinJumpHeight set, calculate the velocity at
    /// the start of a jump and the impulse applied to cut off the jump early
    /// </summary>
    public void CalculateJumpParameters()
    {
        float g = Physics.gravity.magnitude * gravityScale;

        // Energy to move up h units is mass * g * h. Kinetic energy is 0.5 * mass * v squared
        // Cancelling mass out, to jump to height h you need velocity of sqrt(2 * h * g)
        jumpVelocity = Mathf.Sqrt(2.0f * MaxJumpHeight * g);
        float minJumpVelocity = Mathf.Sqrt(2.0f * MinJumpHeight * g);
        // Get downward impulse which, if applied right at start of jump, will at least reach the min jump height
        jumpCutoffVelocity = minJumpVelocity - jumpVelocity;
    }

    /// <summary>
    /// Determines target velocity based on input axes. The direction of movement is 
    /// relative to the camera's forward direction
    /// </summary>
    /// <returns>Velocity along ground given by player input</returns>
    public Vector3 GetTargetVelocity()
    {
        Vector3 inputDirection = CameraTarget.transform.forward * Input.GetAxis("Vertical") + CameraTarget.transform.right * Input.GetAxis("Horizontal");
        Vector3 targetVelocity = Vector3.ClampMagnitude(inputDirection, 1.0f) * GroundSpeed;
        return targetVelocity;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Apply impulse away from collision, such that there is no movement towards colliding object
        Vector3 normal = hit.normal;
        float mag = Vector3.Dot(normal, velocity);
        velocity += -mag * normal;
    }

    /// <summary>
    /// Test whether the player is standing on ground, and set isGrounded property based on result
    /// As the player's capsule collider is wider than its feet, using the default grounding test 
    /// can result in the player seeming floating in midair. So, a spherecast down a short distance
    /// is used instead
    /// </summary>
    void CheckIfGrounded()
    {
        RaycastHit groundHit;
        Debug.DrawRay(transform.position + Vector3.up*FootOffset, Vector3.down *FootRadius, Color.green);

        // Sphere ends movement after its radius - foot offset distance below player transform position
        if (Physics.SphereCast(transform.position + ((FootRadius + FootOffset) * Vector3.up), FootRadius, Vector3.down, out groundHit, FootRadius, Physics.DefaultRaycastLayers, QueryTriggerInteraction.Ignore))
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    /// <summary>
    /// Tells player it has been attacked. If not already dead or in a dying state, knocks 
    /// the player back and begins CombatDeath state
    /// </summary>
    /// <param name="knockbackDirection">Direction in which to fling player</param>
    /// <returns>True if player could be successfully injured</returns>
    public bool Damage(Vector3 knockbackDirection)
    {
        if (!IsDead && !Invincible)
        {
            currentlives -= 1;
            // apply knockback as enemy makes hit
            velocity = knockbackDirection * KnockbackSpeed;
            ChangeState(EPlayerStates.CombatDeath);
            audioSource.PlayOneShot(sounds.Hurt, sounds.HurtScale);
            return true;
        } else
        {
            return false;
        }
    }

    /// <summary>
    /// Tells player they moved into a lethal zone, such as water or lava. This moves
    /// them into the Drowning state
    /// </summary>
    /// <param name="zone">Script for KillZone entered by player</param>
    public void EnterKillzone(KillZone zone)
    {
        if (!IsDead)
        {
            currentlives -= 1;
            ChangeState(EPlayerStates.Drowning);
            AudioSource.PlayClipAtPoint(zone.dieSound, transform.position);
        }
    }
    
    /// <summary>
    /// Sets renderers of this and all children to active or inactive
    /// </summary>
    /// <param name="active">Whether renderers are to be activated or deactivated</param>
    public void SetRenderersActive(bool active)
    {
        foreach(Renderer r in gameObject.GetComponentsInChildren<Renderer>())
        {
            r.enabled = active;
        }
    }

    /// <summary>
    /// Plays attack grunt sound from PlayerSounds asset
    /// </summary>
    public void PlayAttackSound()
    {
        audioSource.PlayOneShot(sounds.AttackGrunt, sounds.AttackGruntScale);
    }
}
