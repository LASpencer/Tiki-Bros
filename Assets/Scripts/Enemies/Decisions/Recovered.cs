using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Decision/Recovered")]
public class Recovered : Decision
{
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
