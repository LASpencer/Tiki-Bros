using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// Action makes enemy patrol its waypoints
/// </summary>
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
            // Set destination as waypoint
            agent.destination = controller.CurrentWaypoint.position;

            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                // Get next destination if waypoint reached
                agent.destination = controller.NextWaypoint().position;
            }
        agent.isStopped = false;
        }
    }

    public override void OnEnter(EnemyController controller)
    {
        controller.Invincible = false;
        controller.AttackActivated = true;
    }

    public override void OnExit(EnemyController controller)
    {
    }
}
