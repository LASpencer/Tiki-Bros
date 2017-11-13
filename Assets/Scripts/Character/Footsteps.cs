using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Footsteps : MonoBehaviour
{

    public AudioClip[] clips;
    private AudioSource a;
    private bool playing = false;
    private const float timerReset = 0.18f;

    void Awake()
    {
        a = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider col)
    {
        //string currTag = col.tag;
        //int currVal = -1;
        //if (currTag.Length <= 2)
        //{
        //    currVal = Int32.Parse(currTag);
        //}
        //if (currVal != -1 && !playing)
        //{
        //    a.PlayOneShot(clips[currVal]);
        //    playing = true;
        //    Invoke ("Reset", timerReset);
        //}
        if (!playing)
        {
            Audible otherAudible = col.gameObject.GetComponent<Audible>();
            if (otherAudible != null)
            {
                AudioClip footstep = otherAudible.GetFootstep(this.gameObject);
                a.PlayOneShot(footstep);
                playing = true;
                Invoke("Reset", timerReset);
            }
        }
    }


    private void Reset()
    {
        playing = false;
    }
}
