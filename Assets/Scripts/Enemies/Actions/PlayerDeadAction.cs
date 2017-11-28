using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Actions to take while waiting for player to respawn
/// </summary>
[CreateAssetMenu(menuName = "EnemyAI/Action/PlayerDead")]
public class PlayerDeadAction : EnemyAction
{
    public override void Act(EnemyController controller)
    {
    }

    public override void OnEnter(EnemyController controller)
    {
        // Stop moving while player already dead
        controller.navAgent.isStopped = true;
    }

    public override void OnExit(EnemyController controller)
    {
        // Start moving again
        controller.navAgent.isStopped = false;
    }
}
