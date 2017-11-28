using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tests if enemy was killed
/// </summary>
[CreateAssetMenu(menuName = "EnemyAI/Decision/Died")]
public class Died : Decision {

    /// <summary>
    /// Checks if enemy has no health left when knockback finishes
    /// </summary>
    /// <param name="controller">Enemy making test</param>
    /// <returns>True if out of health and knockback ends</returns>
    public override bool Decide(EnemyController controller)
    {
        if (controller.RecoveryTime <= controller.TimeInState)
        {
            return controller.Health <= 0;
        } else
        {
            return false;
        }
    }
}
