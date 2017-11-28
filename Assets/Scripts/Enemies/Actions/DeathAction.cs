using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Action for enemy being destroyed
/// </summary>
[CreateAssetMenu(menuName = "EnemyAI/Action/Death")]
public class DeathAction : EnemyAction
{

    public override void Act(EnemyController controller)
    {
        // After set time, destroy the enemy
        if(controller.TimeInState > controller.DeathTime)
        {
            Destroy(controller.gameObject);
        }
    }

    public override void OnEnter(EnemyController controller)
    {
        // Hide dying enemy
        controller.Invincible = true;
        controller.AttackActivated = false;
        controller.Die();
    }

    public override void OnExit(EnemyController controller)
    {
    }
}

