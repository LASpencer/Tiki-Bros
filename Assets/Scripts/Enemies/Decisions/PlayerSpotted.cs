using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Decision/PlayerSpotted")]
public class PlayerSpotted : Decision {

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
            // May need to do additional casts and require a certain number to count as a sighting
            Bounds tikiBounds = player.bounds;
            Vector3 origin = controller.transform.position + controller.navAgent.height * controller.transform.up;
            if(IsTargetVisible(tikiBounds.center, origin, controller.VisionRange))
            {
                playerSeen = true;
            }
        }

        return playerSeen;
    }

    // Returns true if target within range of origin, and raycast doesn't hit anything else on the way
    bool IsTargetVisible(Vector3 target, Vector3 origin, float range)
    {
        bool visible = false;
        Vector3 displacement = target - origin;
        float distance = displacement.magnitude;
        if(distance <= range)
        {
            RaycastHit hit;
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
