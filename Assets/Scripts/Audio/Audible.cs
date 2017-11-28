using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides appropriate audio clips when player interacts with object
/// </summary>
public class Audible : AudibleBase {

    // AudioMaterial holding object's audio clips
    public AudioMaterial Sounds;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// Returns appropriate sound for walking on object
    /// </summary>
    /// <param name="other">Object requesting audio clip</param>
    /// <param name="leftFoot">Is the object a left or right foot?</param>
    /// <returns>Audio clip for footstep</returns>
    public override AudioClip GetFootstep(GameObject other, bool leftFoot)
    {
        if (leftFoot)
        {
            return Sounds.leftStep;
        } else
        {
            return Sounds.rightStep;
        }
    }

    /// <summary>
    /// Returns appropriate audio clip for landing on object
    /// </summary>
    /// <param name="other">Object requesting audio clip</param>
    /// <returns>Audio clip for landing</returns>
    public override AudioClip GetLanding(GameObject other)
    {
        return Sounds.landing;
    }
}
