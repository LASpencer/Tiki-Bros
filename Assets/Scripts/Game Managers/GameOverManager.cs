using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script for Game Over scenes
/// </summary>
public class GameOverManager : MenuManager {

    public Text TreasureCollectedText;


	// Use this for initialization
	void Start () {
        // Display how much treasure was collected
        float treasureCollected = GameManagerController.Instance.CoinsCollected;
        float totalTreasure = GameManagerController.Instance.TotalCoins;

        TreasureCollectedText.text = "TREASURE FOUND:\n" + treasureCollected.ToString() + "/" + totalTreasure.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
