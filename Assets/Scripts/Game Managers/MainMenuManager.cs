﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuManager : MenuManager {

    //TODO consider changing these to canvases?
    public GameObject MenuOptions;
    public GameObject Settings;
    public GameObject HowToPlay;
    public GameObject Credits;

    

	// Use this for initialization
	void Start () {
        Settings.SetActive(false);
        HowToPlay.SetActive(false);
        Credits.SetActive(false);
        ChangeScreen(MenuOptions);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    
}