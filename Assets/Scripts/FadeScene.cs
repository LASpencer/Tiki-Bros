using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeScene : MonoBehaviour {

	Renderer renderer;
	public float alpha = 1.0f;

	IEnumerator Wait()
	{
		yield return new WaitForSeconds (2f);
	}

	public void Update ();
	{

	}

}
