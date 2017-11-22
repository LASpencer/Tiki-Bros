using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Audible/EnemySounds")]
public class EnemySounds : ScriptableObject
{

    public AudioClip Attack;
    public AudioClip Knockback;
    public AudioClip Death;

}
