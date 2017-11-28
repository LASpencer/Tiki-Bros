using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPlayerStates
{
    Idle,
    Run,
    Jump,
    Falling,
    Punching,
    CombatDeath,
    Drowning
}

/// <summary>
/// Base class for all states, providing constructor, method declarations, and fields
/// </summary>
public abstract class PlayerState  {

    public const float IDLE_SPEED = 0.01f; // If x and z components less than this, treat as in idle state

    protected PlayerController player;
    public PlayerController Player { get { return Player; } }

    public PlayerState(PlayerController player)
    {
        this.player = player;
    }

    /// <summary>
    /// Checks if the current state should be changed
    /// </summary>
    abstract public void CheckTransition();

    /// <summary>
    /// Performs any actions required on starting the state
    /// </summary>
    abstract public void OnEnter();

    /// <summary>
    /// Performs behaviour associated with the state
    /// </summary>
    abstract public void Update();

    /// <summary>
    /// Performs any actions required on exiting the state, and cleanup
    /// </summary>
    abstract public void OnExit();
}

/// <summary>
/// State for the player not doing anything
/// </summary>
public class IdleState : PlayerState
{
    public IdleState(PlayerController player) : base(player)
    {
    }

    public override void CheckTransition()
    {
        // Exit to falling on not grounded
        if (!player.IsGrounded)
        {
            player.ChangeState(EPlayerStates.Falling);
        }
        // Exit to jump on jump press
        else if (Input.GetButtonDown("Jump"))
        {
            player.ChangeState(EPlayerStates.Jump);
        }
        // Exit to Punch on punch press
        else if (Input.GetButtonDown("Punch") && player.PunchCooldown == 0.0f)
        {
            player.ChangeState(EPlayerStates.Punching);
        }
        // Exit to running on horizontal/vertical input
        else if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            player.ChangeState(EPlayerStates.Run);
        }
    }

    public override void OnEnter()
    {
        // kill velocity along ground
        player.velocity.x = 0;
        player.velocity.z = 0;
    }

    public override void OnExit()
    {
    }

    public override void Update()
    {
        // Calculate slope of ground below player
        Vector3 groundSlope = Vector3.up;
        RaycastHit groundHit;
        float distance = 0.2f; //HACK
        if (Physics.Raycast(player.transform.position, Vector3.down, out groundHit, distance))
        {
            groundSlope = groundHit.normal;
        }
        // Apply force to player keeping their velocity perpendicular to ground slope
        Vector3 groundVelocity = player.velocity;
        groundVelocity += -(Vector3.Dot(groundVelocity, groundSlope)) * groundSlope;
        Vector3 difference =  -groundVelocity;
        float acceleration = player.GroundAcceleration;
        player.velocity += Vector3.ClampMagnitude(difference, acceleration * Time.deltaTime);
        // Animation parameters
        player.animator.SetFloat("groundSpeed", 0.0f);
    }
}

public class RunState : PlayerState
{
    public RunState(PlayerController player) : base(player)
    {
    }

    public override void CheckTransition()
    {
        
        // Exit to falling on not grounded
        if (!player.IsGrounded)
        {
            player.ChangeState(EPlayerStates.Falling);
        }
        // exit to jump on jump press
        else if (Input.GetButtonDown("Jump"))
        {
            player.ChangeState(EPlayerStates.Jump);
        }
        else if (Input.GetButtonDown("Punch") && player.PunchCooldown == 0.0f)
        {
            player.ChangeState(EPlayerStates.Punching);
        }
        // Exit to idle on not moving
        else if (Mathf.Abs(player.velocity.x) < IDLE_SPEED && Mathf.Abs(player.velocity.y) < IDLE_SPEED)
        {
            player.ChangeState(EPlayerStates.Idle);
        }
    }

    public override void OnEnter()
    {
        
    }

    public override void OnExit()
    {
        
    }

    public override void Update()
    {
        // Get target velocity from input
        Vector3 targetVelocity = player.GetTargetVelocity();
        Vector3 adjustedTarget = targetVelocity;
        // Get slope of ground below player
        Vector3 groundSlope = Vector3.up;
        RaycastHit groundHit;
        float distance = 0.2f; //HACK
        if(Physics.Raycast(player.transform.position, Vector3.down, out groundHit, distance))
        {
            groundSlope = groundHit.normal;
        }
        // Alter target velocity to be perpendicular to ground slope
        adjustedTarget += -(Vector3.Dot(adjustedTarget, groundSlope)) * groundSlope;
        Vector3 groundVelocity = player.velocity;
        groundVelocity += -(Vector3.Dot(groundVelocity, groundSlope)) * groundSlope;
        Vector3 difference = adjustedTarget - groundVelocity;
        float acceleration = player.GroundAcceleration;
        float groundSpeed = groundVelocity.magnitude;
        float targetSpeed = adjustedTarget.magnitude;
        // Determine how much extra force can be applied by "braking" as player moves against current direction
        float frictionRatio = 0f;
        if(targetSpeed == 0)
        {
            frictionRatio = 1;
        } else if(targetSpeed != 0 && groundSpeed != 0)
        {
            float cosForce = Vector3.Dot(groundVelocity, adjustedTarget) / (targetSpeed * groundSpeed);
            frictionRatio = (0.5f - 0.5f * cosForce);
        }
        acceleration += player.BrakingAcceleration * frictionRatio;
        // Apply force to player towards desired velocity, at current acceleration rate
        player.velocity += Vector3.ClampMagnitude(difference, acceleration * Time.deltaTime);

        // Turn player to face desired movement direction
        if (targetVelocity != Vector3.zero)
        { 
            player.transform.forward = targetVelocity;
        }

        // Animation parameters
        player.animator.SetFloat("groundSpeed", groundSpeed);
    }
}

/// <summary>
/// Abstract base class for shared functionality between aerial states
/// </summary>
public abstract class AirState : PlayerState
{
    // Counts down after jump press, allowing input to be registered if player
    // presses jump slightly too early
    protected float JumpToleranceTimer;     

    public AirState(PlayerController player) : base(player)
    {
    }

    public override void OnEnter()
    {
        JumpToleranceTimer = 0;
    }

    public override void Update()
    {
        // Get player's target velocity along x-z plane
        Vector3 target = player.GetTargetVelocity();
        Vector3 groundVelocity = player.velocity;
        groundVelocity.y = 0;
        // Apply force towards target velocity at air movement acceleration rate
        Vector3 difference = target - groundVelocity;
        player.velocity += Vector3.ClampMagnitude(difference, player.AirAcceleration * Time.deltaTime);

        // Run timer for input tolerance
        JumpToleranceTimer = Mathf.Max(0, JumpToleranceTimer - Time.deltaTime);
        // Check if player is trying to jump and reset timer if so
        if (Input.GetButtonDown("Jump"))
        {
            JumpToleranceTimer = player.JumpPressTolerance;
        }
        // Turn player to face desired direction
        if (target != Vector3.zero)
        {
            player.transform.forward = target;
        }
    }

    public override void OnExit()
    {
    }
}

/// <summary>
/// State for when player jumps
/// </summary>
public class JumpState : AirState
{
    float TimeInJump = 0;
    // True while moving up and jump button held down, indicating it can be released to cut off the jump
    bool InUpswing = false;         

    public JumpState(PlayerController player) : base(player)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        player.CalculateJumpParameters();       // HACK: Placed here so developers can alter jump parameters in runtime. Probably belongs in PlayerController.start
        //apply velocity upward
        player.velocity.y = player.JumpVelocity;
        TimeInJump = 0;
        InUpswing = true;
        player.animator.SetTrigger("jumping");
    }

    public override void Update()
    {
        base.Update();
        TimeInJump += Time.deltaTime;
        
        // Check if player wants to cut off jump
        if (InUpswing)
        {
            if (player.velocity.y <= 0)
            {
                InUpswing = false;
            } else if (!Input.GetButton("Jump"))
            {
                // Apply downward impulse to cut off jump, down to a minimum proportion of velocity
                player.velocity.y = Mathf.Max(player.velocity.y + player.JumpCutoffVelocity, player.velocity.y * player.JumpCutoffProportion);
                InUpswing = false;
            }
        }
    }

    public override void OnExit()
    {
        InUpswing = false;
        base.OnExit();
    }

    public override void CheckTransition()
    {
        if (!InUpswing && player.IsGrounded)
        {
            if (JumpToleranceTimer > 0)
            {
                // If landing and player just hit jump button, jump again
                player.ChangeState(EPlayerStates.Jump);
            }
            else if (Mathf.Abs(player.velocity.x) < IDLE_SPEED && Mathf.Abs(player.velocity.y) < IDLE_SPEED)
            {
                // If landing and stationary, idle state
                player.ChangeState(EPlayerStates.Idle);
            }
            else
            {
                // If landing with some ground speed, run state
                player.ChangeState(EPlayerStates.Run);
            }
        }
    }
}

/// <summary>
/// State for when player steps off an edge and falls
/// </summary>
public class FallingState : AirState
{
    // Time spent in state, checked to determine if player can recover into a jump
    float TimeFalling;
    public FallingState(PlayerController player) : base(player)
    {
    }

    public override void CheckTransition()
    {
        if (TimeFalling < player.CoyoteTime && Input.GetButtonDown("Jump"))
        {
            // If jump pressed early enough, jump
            player.ChangeState(EPlayerStates.Jump);
        }
        else if(player.IsGrounded)
        {
            if (JumpToleranceTimer > 0)
            {
                // If grounded and just pressed jump, jump
                player.ChangeState(EPlayerStates.Jump);
            }
            else if (Mathf.Abs(player.velocity.x) < IDLE_SPEED && Mathf.Abs(player.velocity.y) < IDLE_SPEED)
            {
                // If grounded, go to idle or run based on ground speed
                player.ChangeState(EPlayerStates.Idle);
            }
            else
            {
                player.ChangeState(EPlayerStates.Run);
            }
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();
        TimeFalling = 0;
    }

    public override void Update()
    {
        base.Update();
        TimeFalling += Time.deltaTime;
        
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}

/// <summary>
/// State for player punching. While in this state, player's attack
/// hitbox is activated allowing them to damage enemies
/// </summary>
public class PunchingState : PlayerState
{
    // Used to check when windup finished and when to exit state
    float timeInState;

    public PunchingState(PlayerController player) : base(player)
    {
    }

    public override void CheckTransition()
    {
        if(timeInState > player.PunchTime)
        {
            // At end of punch, transition to idle or run based on movement
            if (Mathf.Abs(player.velocity.x) < IDLE_SPEED && Mathf.Abs(player.velocity.y) < IDLE_SPEED)
            {
                player.ChangeState(EPlayerStates.Idle);
            } else
            {
                player.ChangeState(EPlayerStates.Run);
            }
        }
    }

    public override void OnEnter()
    {
        timeInState = 0.0f;
        player.animator.SetTrigger("hasPunched");
        // Make player lunge forward during punch
        Vector3 target = player.GetTargetVelocity();
        if(target != Vector3.zero)
        {
            player.transform.forward = target.normalized;
        }
        player.velocity = player.transform.forward * player.PunchMoveSpeed;

        // Player can't be damaged during punch
        player.Invincible = true;

        
    }

    public override void OnExit()
    {
        // deactivate punching hitbox
        player.Hitbox.gameObject.SetActive(false);
        // Start cooldown
        player.PunchCooldown = player.PunchCooldownTime;
        player.Invincible = false;
    }

    public override void Update()
    {
        timeInState += Time.deltaTime;
        //activate punching hitbox after windup
        if (timeInState > player.PunchWindup)
        {
            player.Hitbox.gameObject.SetActive(true);
        }
    }
}

/// <summary>
/// Abstract base class for states where player is killed
/// </summary>
public abstract class DyingState : PlayerState
{
    protected float timeInState;

    public DyingState(PlayerController player) : base(player)
    {
    }

    public override void Update()
    {
        timeInState += Time.deltaTime;
    }

    public override void OnEnter()
    {
        player.IsDead = true;
        timeInState = 0.0f;
    }

    public override void OnExit()
    {
        player.IsDead = false;
        player.level.RespawnPlayer();
    }
}

/// <summary>
/// State for dying from being hit by enemy
/// </summary>
public class CombatDeathState : DyingState
{
    bool knockbackFinished = false;
    GameObject particles;       // Instance of particle effect spawned on death

    public CombatDeathState(PlayerController player) : base(player)
    {
    }

    public override void Update()
    {
        base.Update();
        // After being knocked back, player explodes
        if(!knockbackFinished && timeInState >= player.KnockbackTime)
        {
            knockbackFinished = true;
            // Stop movement and spawn particle effects
            player.velocity = Vector3.zero;
            particles = GameObject.Instantiate(player.DeathEffect, player.transform.position, player.transform.rotation);
            player.audioSource.PlayOneShot(player.sounds.Explode, player.sounds.ExplodeScale);
            // camera freezes to watch effect
            player.CameraFollows = false;
            // make player invisible
            player.SetRenderersActive(false);
        }
    }

    public override void CheckTransition()
    {
        // Return to idle on respawn
        if (timeInState >= player.CombatDeathTime)
        {
            player.ChangeState(EPlayerStates.Idle);
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();
        player.animator.SetTrigger("hasBeenHit");
        knockbackFinished = false;
    }

    public override void OnExit()
    {
        base.OnExit();
        // Get rid of explosion particles
        if(particles != null)
        {
            GameObject.Destroy(particles);
        }
        // Reactivate camera movement
        player.CameraFollows = true;
        // Draw player again
        player.SetRenderersActive(true);
    }
}

/// <summary>
/// State for dying by falling into a water or lava killzone
/// </summary>
public class DrowiningState : DyingState
{
    // Proportion by which player is slowed when landing in water/lava
    public const float IMPACT_SPEED_FACTOR = 0.1f;

    public DrowiningState(PlayerController player) : base(player)
    {
    }

    public override void Update()
    {
        base.Update();
    }

    public override void CheckTransition()
    {
        // Exit to idle on respawn
        if (timeInState >= player.DrowningDeathTime)
        {
            player.ChangeState(EPlayerStates.Idle);
        }
    }

    public override void OnEnter()
    {
        base.OnEnter();
        //camera freezes to watch death
        player.CameraFollows = false;
        // Player slows down and falls into water at slower rate
        player.gravityScale = player.drowningGravityScale;
        player.velocity *= IMPACT_SPEED_FACTOR;
        // Prevent sounds from landing on lakebed
        player.PlayFootsteps = false;
    }

    public override void OnExit()
    {
        base.OnExit();
        // camera unfreezes
        player.CameraFollows = true;
        // Gravity back to normal
        player.gravityScale = 1;
        // Reactivate feet
        player.PlayFootsteps = true;
    }
}