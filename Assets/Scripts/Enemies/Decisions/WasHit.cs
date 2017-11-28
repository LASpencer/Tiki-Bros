using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tests if enemy was hit this update
/// </summary>
[CreateAssetMenu(menuName = "EnemyAI/Decision/WasHit")]
public class WasHit : Decision
{
    public override bool Decide(EnemyController controller)
    {
        return controller.Hit;
    }
}
