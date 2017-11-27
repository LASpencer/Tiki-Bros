using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

[CreateAssetMenu(menuName = "EnemyAI/Action/Hurt")]
public class HurtAction : EnemyAction
{
    public override void Act(EnemyController controller)
    {
        controller.Invincible = true;
        //TODO move back from hit
        //HACK make it a variable from the enemy
        controller.navAgent.velocity = controller.KnockbackDirection * controller.KnockbackSpeed;
    }

    public override void OnEnter(EnemyController controller)
    {
        controller.Invincible = true;
        controller.AttackActivated = false;
        Vector3 knockback = controller.transform.position - controller.player.transform.position;
        controller.KnockbackDirection = knockback.normalized;
        controller.navAgent.updateRotation = false;
        // Play hurt sound
        controller.audioSource.PlayOneShot(controller.sounds.Knockback, controller.sounds.KnockbackScale);
    }

    public override void OnExit(EnemyController controller)
    {
        controller.navAgent.updateRotation = true;
        controller.navAgent.velocity = Vector3.zero;
    }
}

