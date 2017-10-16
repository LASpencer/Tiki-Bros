using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
        throw new NotImplementedException();
    }

    public override void OnExit()
    {
        throw new NotImplementedException();
    }

    public override void Update()
    {
        throw new NotImplementedException();
    }
}