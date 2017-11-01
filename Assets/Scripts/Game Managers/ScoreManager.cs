using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour 
{
	public static int scoreValue;

	public Text treasureText;


	// Use this for initialization
	void Start () 
	{
//		treasureText = GetComponent<Text> ();
		scoreValue = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		treasureText.text = "TREASURE: " + GameManagerController.Instance.CoinsCollected + " / " + GameManagerController.Instance.TotalCoins ;
	}
}
