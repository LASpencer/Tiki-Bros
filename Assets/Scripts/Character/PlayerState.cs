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
        if (!player.controller.isGrounded)
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
        if (!player.controller.isGrounded)
        {
            player.ChangeState(EPlayerStates.Falling);
        }
        // Exit to idle on not moving
        //TODO rewrite if movement rewritten
        if (player.velocity.x == 0 && player.velocity.z == 0)
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
        Vector3 groundVelocity = player.velocity;
        groundVelocity.y = 0;
        Vector3 difference = target - groundVelocity;
        float acceleration = player.GroundAcceleration;
        float groundSpeed = groundVelocity.magnitude;
        float speedDifference = difference.magnitude;
        if(speedDifference != 0 && groundSpeed != 0)
        {
            float cosForce = Vector3.Dot(groundVelocity, difference) / (speedDifference * groundSpeed);
            acceleration += (0.5f - 0.5f * cosForce) * player.Friction;
        }

        player.velocity += Vector3.ClampMagnitude(difference, acceleration * Time.deltaTime);
        //TODO figure out how to stop bouncing on hills
        // Maybe use raycast/capsulecast to check if ground within margin of error, and if so move down to force collision (do in PlayerController)
        // Alternately, just treat that as being grounded for state change purpose
    }
}

// Abstract class for shared functionality between aerial states
public abstract class AirState : PlayerState
{
    public AirState(PlayerController player) : base(player)
    {
    }

    public override void CheckTransition()
    {
        if (player.controller.isGrounded)
        {
            if (player.velocity.x == 0 && player.velocity.z == 0)
            {
                player.ChangeState(EPlayerStates.Idle);
            }
            else
            {
                player.ChangeState(EPlayerStates.Run);
            }
        }
    }
    public override void Update()
    {
        //TODO air movement applying force, not just set speed
        Vector3 target = player.GetTargetVelocity();
        Vector3 groundVelocity = player.velocity;
        groundVelocity.y = 0;
        Vector3 difference = target - groundVelocity;
        player.velocity += Vector3.ClampMagnitude(difference, player.AirAcceleration * Time.deltaTime);
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
}

public class FallingState : AirState
{
    //TODO allow jumping shortly after entering fall as "grace period"
    public FallingState(PlayerController player) : base(player)
    {
    }

    public override void OnEnter()
    {
 
    }

    public override void Update()
    {
        base.Update();
    }

    public override void OnExit()
    {
        base.OnExit();
    }
}