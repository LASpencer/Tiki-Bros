﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MenuManager {

    public Text TreasureCollectedText;


	// Use this for initialization
	void Start () {
        float treasureCollected = GameManagerController.Instance.CoinsCollected;
        float totalTreasure = GameManagerController.Instance.TotalCoins;

        TreasureCollectedText.text = "Treasure Collected:\n" + treasureCollected.ToString() + "/" + totalTreasure.ToString();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
