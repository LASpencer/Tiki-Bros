using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitbox : MonoBehaviour {

    public EnemyController Controller;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //TODO on trigger enter, check if player and if so damage them
    void OnTriggerEnter(Collider other)
    {
        if (Controller.AttackActivated)
        {
            if (other.CompareTag("Player"))
            {
                other.GetComponent<PlayerController>().Damage();
            }
        }
    }
}
