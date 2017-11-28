using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Tests if enemy has recovered from being hit
/// </summary>
[CreateAssetMenu(menuName = "EnemyAI/Decision/Recovered")]
public class Recovered : Decision
{
    /// <summary>
    /// Checks if enemy can recover from being hurt yet
    /// </summary>
    /// <param name="controller">Enemy making test</param>
    /// <returns>True if knockback is over and enemy has some health</returns>
    public override bool Decide(EnemyController controller)
    {
        if (controller.RecoveryTime <= controller.TimeInState)
        {
            return controller.Health > 0;
        }
        else
        {
            return false;
        }
    }
}
