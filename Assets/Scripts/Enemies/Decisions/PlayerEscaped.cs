using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Decision/PlayerEscaped")]
public class PlayerEscaped : Decision
{
    // Returns true if player is further than escape range
    public override bool Decide(EnemyController controller)
    {
        //TODO maybe also check if player is reachable?
        PlayerController player = controller.player;
        Vector3 displacement = player.transform.position - controller.transform.position;
        if(displacement.sqrMagnitude > controller.EscapeRange * controller.EscapeRange)
        {
            return true;
        } else
        {
            return false;
        }
    }
}
