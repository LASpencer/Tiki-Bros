using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Decision/PlayerSpotted")]
public class PlayerSpotted : Decision {

    public override bool Decide(EnemyController controller)
    {
        bool playerSeen = false;
        // Check if player in line of sight
        PlayerController player = controller.player;
        Vector3 displacement = player.transform.position - controller.transform.position;
        float angle = Vector3.Angle(displacement, controller.transform.forward);
        float maxRangeSqr = controller.VisionRange * controller.VisionRange;
        if (angle <= controller.VisionAngle * 0.5f && displacement.sqrMagnitude <=  maxRangeSqr)
        {
            // If within cone, raycast towards player to check for obstructions
            // May need to do additional casts and require a certain number to count as a sighting
            RaycastHit hit;
            //TODO origin of raycast should be from middle of controller
            Vector3 origin = controller.transform.position + controller.navAgent.height * controller.transform.up;
            if (Physics.Raycast(origin, displacement.normalized, out hit, controller.VisionRange))
            {
                Debug.Log("Crab looking at " + hit.collider + ", " + hit.collider.tag);
                if (hit.collider.CompareTag("Player"))
                {
                    playerSeen = true;
                }
            }
        }

        return playerSeen;
    }
}
