using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScreenManager : MenuManager
{
    public GameObject PauseScreen;
    public GameObject Settings;
    public GameObject HowToPlay;
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
        Settings.SetActive(false);
        HowToPlay.SetActive(false);
        ChangeScreen(PauseScreen);
    }


}
