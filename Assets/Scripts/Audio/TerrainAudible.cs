﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO decide if it makes more sense to just be its own class
[RequireComponent(typeof(Terrain))]
public class TerrainAudible : Audible {

    //TODO maybe both this and Audible have list. Audible always takes from 0, this picks based on texture index
    public AudioMaterial GrassSounds;
    public AudioMaterial SandSounds;
    public AudioMaterial RockSounds;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

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

    public override AudioClip GetLanding(GameObject other)
    {
        AudioMaterial selectedMaterial = SelectMaterial(other);
        return selectedMaterial.landing;
    }

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
                selectedMaterial = Sounds;
                Debug.Log("Sound fell through");
                break;

        }
        return selectedMaterial;
    }
}
