using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Decision/PlayerEscaped")]
public class PlayerEscaped : Decision
{
    public override bool Decide(EnemyController controller)
    {
        PlayerController player = controller.player;
        Vector3 displacement = player.transform.position - controller.transform.position;
        if(displacement.sqrMagnitude > controller.VisionRange * controller.VisionRange)
        {
            return true;
        } else
        {
            return false;
        }
    }
}
