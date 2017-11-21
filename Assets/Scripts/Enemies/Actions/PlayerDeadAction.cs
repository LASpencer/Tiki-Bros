using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Action/PlayerDead")]
public class PlayerDeadAction : EnemyAction
{
    public override void Act(EnemyController controller)
    {
    }

    public override void OnEnter(EnemyController controller)
    {
        controller.navAgent.isStopped = true;
    }

    public override void OnExit(EnemyController controller)
    {
        controller.navAgent.isStopped = false;
    }
}
