using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//TODO decide if it makes more sense to just be its own class
public class TerrainAudible : Audible {

    public AudioMaterial GrassSounds;
    public AudioMaterial SandSounds;
    public AudioMaterial RockSounds;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override AudioClip GetFootstep(GameObject other)
    {
        //TODO figure out relevant terrain texture
        //TODO pick from correct AudioMaterial
        return base.GetFootstep(other);
    }
}
