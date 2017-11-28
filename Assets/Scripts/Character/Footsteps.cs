using System;
using UnityEngine;

/// <summary>
/// Controls playing audio clips for character's footsteps and landing
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class Footsteps : MonoBehaviour
{
    private AudioSource a;
    public bool LeftFoot;               // Is this the left or right foot?
    private bool playing = false;       // Is a clip already being played?
    private bool landing = false;       // Is the next contact with ground landing or walking?
    public PlayerController player;     
    private const float TIMER_RESET = 0.18f;    // Time before another clip is allowed to play

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
            // If player was previously ungrounded, play landing sound
            PlayLanding();
            landing = false;
        }
    }

    void OnTriggerEnter(Collider col)
    {
        if (player.PlayFootsteps && !playing)
        {
            AudibleBase otherAudible = col.gameObject.GetComponent<AudibleBase>();
            if (otherAudible != null)
            {
                // Get appropriate sound from object walked on
                AudioClip footstep;
                footstep = otherAudible.GetFootstep(this.gameObject, LeftFoot);
                // Play sound
                a.PlayOneShot(footstep);
                playing = true;
                landing = false;
                Invoke("Reset", TIMER_RESET);
            }
        }
    }


    public void PlayLanding()
    {
        if (player.PlayFootsteps && !playing)
        {
            if (LeftFoot)
            {
                // Check for appropriate sound on object below player
                AudioClip landingClip;
                RaycastHit hit;
                if (Physics.Raycast(this.transform.position, Vector3.down, out hit))
                {
                    Audible otherAudible = hit.collider.gameObject.GetComponent<Audible>();
                    if (otherAudible != null)
                    {
                        // Play landing sound
                        landingClip = otherAudible.GetLanding(this.gameObject);
                        a.PlayOneShot(landingClip);
                        playing = true;
                        Invoke("Reset", TIMER_RESET);
                    }
                }
            } else
            {
                // Stop right foot from playing footstep audio
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
