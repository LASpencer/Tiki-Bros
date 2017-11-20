using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Action/Death")]
public class DeathAction : EnemyAction
{

    public override void Act(EnemyController controller)
    {

        if(controller.TimeInState > controller.DeathTime)   //HACK
        {
            Destroy(controller.gameObject);
        }
    }

    public override void OnEnter(EnemyController controller)
    {
        controller.Invincible = true;
        controller.AttackActivated = false;
        controller.Die();
    }

    public override void OnExit(EnemyController controller)
    {
    }
}

