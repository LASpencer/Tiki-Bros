using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Tests if the player was killed
/// </summary>
[CreateAssetMenu(menuName = "EnemyAI/Decision/PlayerKilled")]
public class PlayerKilled : Decision {

    public override bool Decide(EnemyController controller)
    {
        return controller.player.IsDead;
    }
}
