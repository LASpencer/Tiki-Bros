using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract base for Audible and TerrainAudible
/// </summary>
abstract public class AudibleBase : MonoBehaviour {

    /// <summary>
    /// Returns appropriate sound for walking on object
    /// </summary>
    /// <param name="other">Object requesting audio clip</param>
    /// <param name="leftFoot">Is the object a left or right foot?</param>
    /// <returns>Audio clip for footstep</returns>
    public abstract AudioClip GetFootstep(GameObject other, bool leftFoot);

    /// <summary>
    /// Returns appropriate audio clip for landing on object
    /// </summary>
    /// <param name="other">Object requesting audio clip</param>
    /// <returns>Audio clip for landing</returns>
    public abstract AudioClip GetLanding(GameObject other);
}
