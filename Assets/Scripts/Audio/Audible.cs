using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Provides appropriate audio clips when player interacts with object
public class Audible : MonoBehaviour {

    public AudioMaterial Sounds;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //TODO figure out what should be passed through
    public virtual AudioClip GetFootstep(GameObject other)
    {
        //TODO: Alternate between providing leftFoot and rightFoot
        return Sounds.footstep;
    }

    public virtual AudioClip GetLanding(GameObject other)
    {
        Debug.Log("Landing sound");
        return Sounds.landing;
    }
}
