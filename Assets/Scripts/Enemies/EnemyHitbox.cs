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
                PlayerController player = other.GetComponent<PlayerController>();
                if (!player.IsDead)
                {
                    Controller.Attack();
                    Vector3 displacement = player.transform.position - Controller.transform.position;
                    player.Damage(displacement.normalized);
                }
            }
        }
    }
}
