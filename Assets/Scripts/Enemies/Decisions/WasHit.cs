using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Decision/WasHit")]
public class WasHit : Decision
{
    public override bool Decide(EnemyController controller)
    {
        return controller.Hit;
    }
}
