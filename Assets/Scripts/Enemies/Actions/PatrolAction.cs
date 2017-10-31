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
        // If no waypoints, do nothing
        if (controller.Waypoints.Count == 0)
        {
            return;
        }
        else
        {   // Follow waypoints
            NavMeshAgent agent = controller.navAgent;
            // If no path set yet, set destination
            if (!agent.hasPath)
            {
                agent.destination = controller.CurrentWaypoint.position;
            }
            else
            {
                if (agent.remainingDistance <= agent.stoppingDistance)
                {
                    // Get next destination
                    agent.destination = controller.NextWaypoint().position;
                }
            }
            agent.isStopped = false;
        }
    }
}
