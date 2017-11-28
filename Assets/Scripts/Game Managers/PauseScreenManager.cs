using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Controls the in game pause menu
/// </summary>
public class PauseScreenManager : MenuManager
{
    public GameObject PauseScreen;
    public GameObject Settings;
    public GameObject HowToPlay;
	public GameObject HowToPlay2;

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    void OnEnable()
    {
        // Sets initial screens
        Settings.SetActive(false);
        HowToPlay.SetActive(false);
		HowToPlay2.SetActive(false);
        ChangeScreen(PauseScreen);
    }


}
