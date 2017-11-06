using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EPlayerStates
{
    Idle,
    Run,
    Jump,
    Falling
}

public abstract class PlayerState  {

    public const float IDLE_SPEED = 0.0001f; // If x and y components less than this, treat as in idle state

    protected PlayerController player;
    public PlayerController Player { get { return Player; } }

    public PlayerState(PlayerController player)
    {
        this.player = player;
    }

    abstract public void CheckTransition();

    abstract public void OnEnter();

    abstract public void Update();

    abstract public void OnExit();
}

public class IdleState : PlayerState
{
    public IdleState(PlayerController player) : base(player)
    {
    }

    public override void CheckTransition()
    {
        // Exit to running on horizontal/vertical input
        if (Input.GetAxis("Vertical") != 0 || Input.GetAxis("Horizontal") != 0)
        {
            player.ChangeState(EPlayerStates.Run);
        }
        // Exit to jump on jump press
        if (Input.GetButtonDown("Jump"))
        {
            player.ChangeState(EPlayerStates.Jump);
        }
        // Exit to falling on not grounded
        if (!player.IsGrounded)
        {
            player.ChangeState(EPlayerStates.Falling);
        }
    }

    public override void OnEnter()
    {
        //TODO if entry condition changes, may not need to kill velocity
        player.velocity.x = 0;
        player.velocity.z = 0;
    }

    public override void OnExit()
    {
    }

    public override void Update()
    {
        //TODO exiting to other states
        
    }
}

public class RunState : PlayerState
{
    public RunState(PlayerController player) : base(player)
    {
    }

    public override void CheckTransition()
    {
        //TODO exit to jump on jump press
        if (Input.GetButtonDown("Jump"))
        {
            player.ChangeState(EPlayerStates.Jump);
        }
        // Exit to falling on not grounded
        if (!player.IsGrounded)
        {
            player.ChangeState(EPlayerStates.Falling);
        }
        // Exit to idle on not moving
        //TODO rewrite if movement rewritten
        if (Mathf.Abs(player.velocity.x) < IDLE_SPEED && Mathf.Abs(player.velocity.y) < IDLE_SPEED)
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
        //TODO write a more fluid movement (accellerate in/decellerate out)
        Vector3 target = player.GetTargetVelocity();
        Vector3 groundSlope = Vector3.up;
        RaycastHit groundHit;
        float distance = 0.2f; //HACK
        if(Physics.Raycast(player.transform.position, Vector3.down, out groundHit, distance))
        {
            groundSlope = groundHit.normal;
        }
        target += -(Vector3.Dot(target, groundSlope)) * groundSlope;
        Vector3 groundVelocity = player.velocity;
        //groundVelocity.y = 0;
        groundVelocity += -(Vector3.Dot(groundVelocity, groundSlope)) * groundSlope;
        Vector3 difference = target - groundVelocity;
        float acceleration = player.GroundAcceleration;
        float groundSpeed = groundVelocity.magnitude;
        float targetSpeed = target.magnitude;
        float frictionRatio = 0f;
        if(targetSpeed == 0)
        {
            frictionRatio = 1;
        } else if(targetSpeed != 0 && groundSpeed != 0)
        {
            float cosForce = Vector3.Dot(groundVelocity, target) / (targetSpeed * groundSpeed);
            frictionRatio = (0.5f - 0.5f * cosForce);
        }
        acceleration += player.Friction * frictionRatio;
        player.velocity += Vector3.ClampMagnitude(difference, acceleration * Time.deltaTime);
        //TODO figure out how to stop bouncing on hills
        // Maybe use raycast/capsulecast to check if ground within margin of error, and if so move down to force collision (do in PlayerController)
        // Alternately, just treat that as being grounded for state change purpose
    }
}

// Abstract class for shared functionality between aerial states
public abstract class AirState : PlayerState
{
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
        //TODO air movement applying force, not just set speed
        Vector3 target = player.GetTargetVelocity();
        Vector3 groundVelocity = player.velocity;
        groundVelocity.y = 0;
        Vector3 difference = target - groundVelocity;
        player.velocity += Vector3.ClampMagnitude(difference, player.AirAcceleration * Time.deltaTime);

        // Check if player trying to jump
        JumpToleranceTimer = Mathf.Max(0, JumpToleranceTimer - Time.deltaTime);
        if (Input.GetButtonDown("Jump"))
        {
            JumpToleranceTimer = player.JumpPressTolerance;
        }
    }

    public override void OnExit()
    {
    }
}

public class JumpState : AirState
{
    float TimeInJump = 0;
    bool InUpswing = false;

    public JumpState(PlayerController player) : base(player)
    {
    }

    public override void OnEnter()
    {
        base.OnEnter();
        player.CalculateJumpParameters();       //TODO figure out how to only do it once, or just move this to player.start() and ignore field changes at runtime
        //apply velocity upward
        player.velocity.y = player.JumpVelocity;
        TimeInJump = 0;
        InUpswing = true;

    }

    public override void Update()
    {
        // TODO double jump?
        base.Update();
        TimeInJump += Time.deltaTime;

        // Cutoff jump version
        
        if (InUpswing)
        {
            if (player.velocity.y <= 0)
            {
                InUpswing = false;
            } else if (!Input.GetButton("Jump"))
            {
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
        // Don't land from jumping until controller collider actually hits
        if (player.controller.isGrounded)
        {
            if (JumpToleranceTimer > 0)
            {
                player.ChangeState(EPlayerStates.Jump);
            }
            else if (Mathf.Abs(player.velocity.x) < IDLE_SPEED && Mathf.Abs(player.velocity.y) < IDLE_SPEED)
            {
                player.ChangeState(EPlayerStates.Idle);
            }
            else
            {
                player.ChangeState(EPlayerStates.Run);
            }
        }
    }
}

public class FallingState : AirState
{
    float TimeFalling;
    //TODO allow jumping shortly after entering fall as "grace period"
    public FallingState(PlayerController player) : base(player)
    {
    }

    public override void CheckTransition()
    {
        if (TimeFalling < player.CoyoteTime && Input.GetButtonDown("Jump"))
        {
            player.ChangeState(EPlayerStates.Jump);
        }
        else if(player.IsGrounded)
        {
            if (JumpToleranceTimer > 0)
            {
                player.ChangeState(EPlayerStates.Jump);
            }
            else if (Mathf.Abs(player.velocity.x) < IDLE_SPEED && Mathf.Abs(player.velocity.y) < IDLE_SPEED)
            {
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