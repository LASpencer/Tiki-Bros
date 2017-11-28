using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tests if player has escaped a pursuing enemy
/// </summary>
[CreateAssetMenu(menuName = "EnemyAI/Decision/PlayerEscaped")]
public class PlayerEscaped : Decision
{
    /// <summary>
    /// Compares distance to player with escape range for enemy
    /// </summary>
    /// <param name="controller">Enemy making test</param>
    /// <returns>True if player further than escape range</returns>
    public override bool Decide(EnemyController controller)
    {
        // Just checks distance, not whether player is reachable. This means they will stop and
        // menace a player beyond a barrier/gap until they move away
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
