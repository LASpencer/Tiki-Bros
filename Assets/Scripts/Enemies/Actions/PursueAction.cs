using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(menuName = "EnemyAI/Action/Pursue")]
public class PursueAction : EnemyAction {

    public override void Act(EnemyController controller)
    {
        NavMeshAgent agent = controller.navAgent;
        agent.destination = controller.player.transform.position;
    }
}
