using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotateScript : MonoBehaviour {

	public float rotateSpeed = 50f;

	void Update()
		{
			transform.Rotate(0, rotateSpeed * Time.deltaTime,  0);
		}
}
