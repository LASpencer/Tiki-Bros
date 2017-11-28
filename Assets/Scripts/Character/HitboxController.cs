using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Holds trigger for Hitbox used when player punches
/// </summary>
public class HitboxController : MonoBehaviour {

    public PlayerController player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter(Collider other)
    { 
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().Damage();
        }
    }
}
