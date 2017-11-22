﻿using System;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Footsteps : MonoBehaviour
{
    private AudioSource a;
    public bool LeftFoot;
    private bool playing = false;
    private bool landing = false;
    public PlayerController player;
    private const float TIMER_RESET = 0.18f;    //HACK maybe should be exposed?

    void Awake()
    {
        a = GetComponent<AudioSource>();
        player = GetComponentInParent<PlayerController>();
    }

    void Update()
    {
        // If player becomes ungrounded, feet will land rather than step
        if (!player.IsGrounded)
        {
            landing = true;
        }
        else if (landing)
        {
            PlayLanding();
            landing = false;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (!playing)
        {
            Audible otherAudible = col.gameObject.GetComponent<Audible>();
            if (otherAudible != null)
            {
                //TODO decide whether to get Footstep or Landing sound based on player state
                AudioClip footstep;
                //if (landing)
                //{
                //    if (LeftFoot)
                //    {
                //        footstep = otherAudible.GetLanding(this.gameObject);
                //    } else
                //    {
                //        //HACK figure out more elegant control structure
                //        landing = false;
                //        return;
                //    }
                //}
                //else
                //{
                    footstep = otherAudible.GetFootstep(this.gameObject, LeftFoot);
                //}
                a.PlayOneShot(footstep);
                playing = true;
                landing = false;
                Invoke("Reset", TIMER_RESET);
            }
        }
    }


    public void PlayLanding()
    {
        if (!playing)
        {
            if (LeftFoot)
            {
                AudioClip landingClip;
                RaycastHit hit;
                if (Physics.Raycast(this.transform.position, Vector3.down, out hit))
                {
                    Audible otherAudible = hit.collider.gameObject.GetComponent<Audible>();
                    if (otherAudible != null)
                    {
                        landingClip = otherAudible.GetLanding(this.gameObject);
                        a.PlayOneShot(landingClip);
                        playing = true;
                        Invoke("Reset", TIMER_RESET);
                    }
                }
            } else
            {
                playing = true;
                Invoke("Reset", TIMER_RESET);
            }
        }

    }

    private void Reset()
    {
        playing = false;
    }
}
