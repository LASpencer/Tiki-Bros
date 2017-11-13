using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(menuName = "Audible/AudioMaterial")]
// This class holds various audio clips for an object
public class AudioMaterial : ScriptableObject {

    public AudioClip footstep;
    public AudioClip leftStep;
    public AudioClip rightStep;
    public AudioClip landing;
    //TODO landing audio?
    //TODO punched audio
}
