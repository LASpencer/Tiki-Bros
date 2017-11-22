using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSounds : MonoBehaviour {

    public AudioClip AttackGrunt;
    public AudioClip Hurt;

    public AudioSource source;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//TODO countdown timer so only one sound played at once?
	}

    public void PlayPunchSound()
    {
        //TODO
    }

    public void PlayHurtSound()
    {
        //TODO
    }


}
