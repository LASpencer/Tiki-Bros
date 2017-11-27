using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Audible/PlayerSounds")]
public class PlayerSounds : ScriptableObject {

    public AudioClip AttackGrunt;
    public float AttackGruntScale = 1;
    public AudioClip Hurt;
    public float HurtScale = 1;
    public AudioClip Explode;
    public float ExplodeScale = 1;

}
