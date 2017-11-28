using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for controlling deadly areas such as deep water and pools of lava
/// </summary>
public class KillZone : MonoBehaviour
{
    public LevelManager levelManager;
    public AudioClip dieSound;          // Clip to play when player falls into killzone

	// Use this for initialization
	void Start ()
    {
        levelManager = FindObjectOfType<LevelManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // If player enters killzone, let them know so they can die
            PlayerController player = other.GetComponent<PlayerController>();
            if (!player.IsDead)
            {
                player.EnterKillzone(this);
            }
        }

    }
}
