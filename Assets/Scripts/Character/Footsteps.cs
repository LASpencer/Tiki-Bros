using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Footsteps : MonoBehaviour
{

    public AudioClip[] clips;
    private AudioSource a;
    private bool once = false;
    private const float timerReset = 0.18f;

    void Awake()
    {
        a = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider col)
    {
        string currTag = col.tag;
        int currVal = -1;
        if (currTag.Length <= 2)
        {
            currVal = Int32.Parse(currTag);
        }
        if (currVal != -1 && !once)
        {
            a.PlayOneShot(clips[currVal]);
            once = true;
            Invoke ("Reset", timerReset);
        }
    }
    private void Reset()
    {
        once = false;
    }
}
