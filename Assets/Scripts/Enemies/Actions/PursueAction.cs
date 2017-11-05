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
    }
}
