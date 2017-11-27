using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// This class holds various audio clips for an object
/// </summary>
[CreateAssetMenu(menuName = "Audible/AudioMaterial")]
public class AudioMaterial : ScriptableObject {
    
    public AudioClip leftStep;
    public AudioClip rightStep;
    public AudioClip landing;
}
