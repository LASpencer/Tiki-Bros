using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for checkpoints in level, which set a new respawn point when reached
/// </summary>
public class Checkpoint : MonoBehaviour
{

    public LevelManager levelManager;
    public AudioClip ActivateSound;

    [Tooltip("Location at which player will respawn")]
    public GameObject spawnPoint;

	public GameObject CheckpointParticles;

    // Use this for initialization
    void Start()
    {
        levelManager = FindObjectOfType<LevelManager>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && levelManager.currentCheckpoint != this)
        {
            // If player reaches checkpoint that's not the current one, set it as the current checkpoint
            if(levelManager.currentCheckpoint != null)
            {
                // Deactivate last checkpoint's effects
                levelManager.currentCheckpoint.Deactivate();
            }
            levelManager.currentCheckpoint = this;
            // Play audio and turn on light, particle effects
            this.Activate();
        }
    }

    /// <summary>
    /// Turn on particle effects and play activation sound
    /// </summary>
    public void Activate()
    {
        gameObject.GetComponent<AudioSource>().PlayOneShot(ActivateSound);
        if (CheckpointParticles != null)
            CheckpointParticles.SetActive(true);
    }

    /// <summary>
    /// Turns off effects
    /// </summary>
    public void Deactivate()
    {
        if (CheckpointParticles != null)
            CheckpointParticles.SetActive(false);
    }
}
