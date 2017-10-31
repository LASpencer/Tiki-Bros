using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// Move to the next waypoint
[CreateAssetMenu(menuName = "EnemyAI/Action/Patrol")]
public class PatrolAction : EnemyAction
{
    public override void Act(EnemyController controller)
    {
        //TODO follow waypoints
        Vector3 destination = controller.CurrentWaypoint.position;
        NavMeshAgent agent = controller.navAgent;
        if(!agent.hasPath)
        {
            agent.destination = destination;
        } else
        {
            if(agent.remainingDistance <= agent.stoppingDistance)
            {
                // Get next destination
                agent.destination = controller.NextWaypoint().position;
            }
        }
        agent.Resume();
    }
}
