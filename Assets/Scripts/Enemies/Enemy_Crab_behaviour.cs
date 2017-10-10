using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Crab_behaviour : MonoBehaviour 
{
	public Transform target;

	public float movementSpeed;
	
	// Update is called once per frame
	void Update () 
	{
		float step = movementSpeed * Time.deltaTime;

		transform.position = Vector3.MoveTowards( transform.position, target.position, step);

	}
}
