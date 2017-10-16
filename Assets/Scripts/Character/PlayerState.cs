using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EPlayerStates
{
    Idle,
    Run,
    Jump,
    Falling
}

public abstract class PlayerState  {

    private PlayerController player;
    PlayerController Player { get { return Player; } }

    public PlayerState(PlayerController player)
    {
        this.player = player;
    }

    abstract public void OnEnter();

    abstract public void Update();

    abstract public void OnExit();
}

public class IdleState : PlayerState
{
    public IdleState(PlayerController player) : base(player)
    {
    }

    public override void OnEnter()
    {

    }

    public override void OnExit()
    {
    }

    public override void Update()
    {
        //TODO exiting to other states
        // Exit to running on horizontal/vertical input
        // Exit to jump on jump press
        // Exit to falling on not grounded
        throw new NotImplementedException();
    }
}

public class RunState : PlayerState
{
    public RunState(PlayerController player) : base(player)
    {
    }

    public override void OnEnter()
    {
        throw new NotImplementedException();
    }

    public override void OnExit()
    {
        throw new NotImplementedException();
    }

    public override void Update()
    {
        //TODO movement
        //TODO exit to jump on jump press
        // Exit to falling on not grounded
        // Exit to idle on not moving
        throw new NotImplementedException();
    }
}

// Abstract class for shared functionality between aerial states
public abstract class AirState : PlayerState
{
    public AirState(PlayerController player) : base(player)
    {
    }


    public override void Update()
    {
        //TODO apply gravity
        //TODO air movement
        //TODO on grounding, exit to Idle
    }

    public override void OnExit()
    {
        //TODO y speed = 0
    }
}

public class JumpState : AirState
{
    public JumpState(PlayerController player) : base(player)
    {
    }

    public override void OnEnter()
    {
        //TODO apply velocity upward
        throw new NotImplementedException();
    }

    public override void Update()
    {
        // TODO double jump?
        base.Update();
    }

    public override void OnExit()
    {
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
        throw new NotImplementedException();
    }
}