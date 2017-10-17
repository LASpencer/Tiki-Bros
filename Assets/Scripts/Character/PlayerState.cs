﻿using System;
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
        if (Input.GetAxis("Vertical") == 0 && Input.GetAxis("Horizontal") == 0)
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
        Vector3 inputDirection = player.transform.forward * Input.GetAxis("Vertical") + player.transform.right * Input.GetAxis("Horizontal");
        Vector3 move = inputDirection.normalized * player.GroundSpeed;
        player.velocity.x = move.x;
        player.velocity.z = move.z;
        //TODO figure out how to stop bouncing on hills
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
            //TODO exit to idle instead if stopped?
            player.ChangeState(EPlayerStates.Run);
        }
    }
    public override void Update()
    {
        //TODO air movement applying force, not just set speed
        Vector3 inputDirection = player.transform.forward * Input.GetAxis("Vertical") + player.transform.right * Input.GetAxis("Horizontal");
        Vector3 move = inputDirection.normalized * player.AirSpeed;
        player.velocity.x = move.x;
        player.velocity.z = move.z;
    }

    public override void OnExit()
    {
        player.velocity.y = 0;
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
        //TODO apply velocity upward
        //TODO keep applying force while button held, until finished time to reach max jump height
        player.velocity.y = player.JumpVelocity;
        TimeInJump = 0;
        InUpswing = true;

    }

    public override void Update()
    {
        // TODO double jump?
        base.Update();
        TimeInJump += Time.deltaTime;
        // Increase jump height while jump held
        if (InUpswing)
        {
            if (Input.GetButton("Jump"))
            {
                // apply up force
                player.velocity.y += player.JumpHoldForce * Time.deltaTime;
                if(TimeInJump > player.JumpChargeTime)
                {
                    InUpswing = false;
                    player.velocity.y += player.JumpHoldForce * (player.JumpChargeTime - TimeInJump);
                }
            } else
            {
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