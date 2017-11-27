using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{

    public LevelManager levelManager;
    public AudioClip dieSound;

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
        Debug.Log(other.name + "Has entered Kill Zone");
        if (other.CompareTag("Player"))
        {
            PlayerController player = other.GetComponent<PlayerController>();
            if (!player.IsDead)
            {
                player.EnterKillzone(this);
            }
            Debug.Log("Player Has died");
        }

    }
}
