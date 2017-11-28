using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Checks if the player can be seen
/// </summary>
[CreateAssetMenu(menuName = "EnemyAI/Decision/PlayerSpotted")]
public class PlayerSpotted : Decision {

    /// <summary>
    /// Checks whether player is within vision cone, and if so checks for obstructions
    /// </summary>
    /// <param name="controller">Enemy making test</param>
    /// <returns>True if enemy can see player</returns>
    public override bool Decide(EnemyController controller)
    {
        bool playerSeen = false;
        // Check if player in within line of sight cone
        PlayerController player = controller.player;
        Vector3 displacement = player.transform.position - controller.transform.position;
        float angle = Vector3.Angle(displacement, controller.transform.forward);
        float maxRangeSqr = controller.VisionRange * controller.VisionRange;
        if (angle <= controller.VisionAngle * 0.5f && displacement.sqrMagnitude <=  maxRangeSqr)
        {
            // If within cone, raycast towards player to check for obstructions
            // HACK May need to do additional casts and require a certain number to count as a sighting
            Bounds tikiBounds = player.bounds;
            Vector3 origin = controller.transform.position + controller.navAgent.height * controller.transform.up;
            if(IsTargetVisible(tikiBounds.center, origin, controller.VisionRange))
            {
                playerSeen = true;
            }
        }

        return playerSeen;
    }


    /// <summary>
    /// Makes a raycast testing if any obstructions block sight between two positions. Since this is
    /// used to check if the player is seen, a hit on the player counts as a success, rather than an
    /// obstruction
    /// </summary>
    /// <param name="target">Position being looked at</param>
    /// <param name="origin">Position being looked from</param>
    /// <param name="range">Maximum distance from which things can be seen</param>
    /// <returns>True if no obstructions hit (apart from the player)</returns>
    bool IsTargetVisible(Vector3 target, Vector3 origin, float range)
    {
        bool visible = false;
        Vector3 displacement = target - origin;
        float distance = displacement.magnitude;
        // Check if target is within visible range
        if(distance <= range)
        {
            RaycastHit hit;
            // Make raycast out to distance of target, so it doesn't overshoot and detect obstruction on far side
            if(Physics.Raycast(origin, displacement.normalized, out hit, distance))
            {
                Debug.Log("Crab looking at " + hit.collider + ", " + hit.collider.tag);
                if (hit.collider.CompareTag("Player"))
                {
                    // If player's collider hit, player visible
                    //HACK could make this test more reusable by taking object wanted and checking if
                    // collider is for that object
                    visible = true;
                }
            } else
            {
                // If no obstructions hit before reaching target point, point is visible
                visible = true;
            }
        }
        return visible;
    }
}
