using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        //TODO if punching an enemy, enemy killed
        //TODO give enemies rigidbody/character controller so they can be punched
        if (other.CompareTag("Enemy"))
        {
            other.GetComponent<EnemyController>().Damage();
        }
    }
}
