using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audible/EnemySounds")]
public class EnemySounds : ScriptableObject
{

    public AudioClip Attack;
    public float AttackScale = 1;
    public AudioClip Knockback;
    public float KnockbackScale = 1;
    public AudioClip Death;
    public float DeathScale = 1;

}
