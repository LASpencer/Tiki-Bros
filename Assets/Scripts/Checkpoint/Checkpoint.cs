using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            levelManager.currentCheckpoint = this;
            gameObject.GetComponent<AudioSource>().PlayOneShot(ActivateSound);
			if (CheckpointParticles != null)
				CheckpointParticles.SetActive (true);
        }
    }
}
