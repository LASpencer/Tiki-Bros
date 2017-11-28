using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

/// <summary>
/// Action for being damaged and knocked back by an attack
/// </summary>
[CreateAssetMenu(menuName = "EnemyAI/Action/Hurt")]
public class HurtAction : EnemyAction
{
    public override void Act(EnemyController controller)
    {
        // Cannot be injured a second time while already hurt
        controller.Invincible = true;
        // Fling along ground away from hit
        controller.navAgent.velocity = controller.KnockbackDirection * controller.KnockbackSpeed;
    }

    public override void OnEnter(EnemyController controller)
    {
        controller.Invincible = true;
        // Cannot hurt player while injured
        controller.AttackActivated = false;
        // Set knockback direction away from player
        Vector3 knockback = controller.transform.position - controller.player.transform.position;
        controller.KnockbackDirection = knockback.normalized;
        // Keep facing same direction while knocked back
        controller.navAgent.updateRotation = false;
        // Play hurt sound
        controller.audioSource.PlayOneShot(controller.sounds.Knockback, controller.sounds.KnockbackScale);
    }

    public override void OnExit(EnemyController controller)
    {
        // Stop moving and allow enemy to rotate again
        controller.navAgent.updateRotation = true;
        controller.navAgent.velocity = Vector3.zero;
    }
}

