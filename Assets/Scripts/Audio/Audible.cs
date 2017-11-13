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
    public virtual AudioClip GetFootstep(GameObject other, bool leftFoot)
    {
        if (leftFoot)
        {
            return Sounds.leftStep;
        } else
        {
            return Sounds.rightStep;
        }
    }

    public virtual AudioClip GetLanding(GameObject other)
    {
        return Sounds.landing;
    }
}
