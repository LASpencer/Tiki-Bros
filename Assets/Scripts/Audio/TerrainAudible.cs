using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Provides suitable audio clips for player interacting with terrain. Using the 
/// TerrainTextureFinder class, it determines what texture they're interacting with
/// and provides the clip associated with the texture
/// </summary>
[RequireComponent(typeof(Terrain))]
public class TerrainAudible : AudibleBase {

    // AudioMaterials for different textures
    public AudioMaterial GrassSounds;
    public AudioMaterial SandSounds;
    public AudioMaterial RockSounds;


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
        AudioMaterial selectedMaterial = SelectMaterial(other);
        if (leftFoot)
        {
            return selectedMaterial.leftStep;
        } else
        {
            return selectedMaterial.rightStep;
        }
    }

    /// <summary>
    /// Returns appropriate audio clip for landing on object
    /// </summary>
    /// <param name="other">Object requesting audio clip</param>
    /// <returns>Audio clip for landing</returns>
    public override AudioClip GetLanding(GameObject other)
    {
        AudioMaterial selectedMaterial = SelectMaterial(other);
        return selectedMaterial.landing;
    }

    /// <summary>
    /// Determines the AudioMaterial to use based on the predominant
    /// terrain sprite under the other object
    /// </summary>
    /// <param name="other">Object requesting audio clip</param>
    /// <returns>Audio material corresponding to terrain texture</returns>
    private AudioMaterial SelectMaterial(GameObject other)
    {
        // Figure out predominant terrain
        int terrainIndex = TerrainTextureFinder.GetMainTexture(gameObject.GetComponent<Terrain>(), other.transform.position);
        AudioMaterial selectedMaterial;
        // Select that terrain's material
        switch (terrainIndex)
        {
            case 0:     //Rock
                selectedMaterial = RockSounds;
                break;
            case 1:     //Grass
                selectedMaterial = GrassSounds;
                break;
            case 2:     //Sand
                selectedMaterial = SandSounds;
                break;
            default:
                selectedMaterial = RockSounds;
                Debug.Log("Sound fell through");
                break;

        }
        return selectedMaterial;
    }
}
