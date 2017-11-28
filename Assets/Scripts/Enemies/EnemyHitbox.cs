using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls enemy attack hitbox
/// </summary>
public class EnemyHitbox : MonoBehaviour {

    public EnemyController Controller;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // Using OnTriggerStay to allow for player losing invincibility while within trigger
    void OnTriggerStay(Collider other)
    {
        if (Controller.AttackActivated)
        {
            if (other.CompareTag("Player"))
            {
                // If enemy's attack is active, and player in trigger, damage them
                PlayerController player = other.GetComponent<PlayerController>();
                // Calculate direction to knockback player
                Vector3 displacement = player.transform.position - Controller.transform.position;
                player.Damage(displacement.normalized);
            }
        }
    }
}
