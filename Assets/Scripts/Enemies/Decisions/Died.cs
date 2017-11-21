using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Decision/Died")]
public class Died : Decision {
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
