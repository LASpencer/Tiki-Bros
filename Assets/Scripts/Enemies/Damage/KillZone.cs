using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillZone : MonoBehaviour
{

    public LevelManager levelManager;


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
            //TODO make player responsible for dying instead
            levelManager.RespawnPlayer();
            Debug.Log("Player Has died");
        }

    }
}
