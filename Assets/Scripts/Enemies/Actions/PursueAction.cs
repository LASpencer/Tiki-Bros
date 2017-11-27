using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "EnemyAI/Action/Pursue")]
public class PursueAction : EnemyAction {

    public override void Act(EnemyController controller)
    {
        // Move towards player
        NavMeshAgent agent = controller.navAgent;
        agent.destination = controller.player.transform.position;
        agent.isStopped = false;

        // Snap every so often
        if(controller.AttackCD <= 0)
        {
            controller.AttackCD = controller.AttackTime;
            controller.Attack();
        }
    }

    public override void OnEnter(EnemyController controller)
    {
        controller.Invincible = false;
        controller.AttackActivated = true;

        controller.AttackCD = controller.AttackTime;
    }

    public override void OnExit(EnemyController controller)
    {
    }
}
