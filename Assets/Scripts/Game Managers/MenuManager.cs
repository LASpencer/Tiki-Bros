﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/// <summary>
/// Base class holding common behaviour used by menus
/// </summary>
public class MenuManager : MonoBehaviour
{
    // UI elements currently being displayed
    protected GameObject CurrentScreen;

    /// <summary>
    /// Starts playing the game, beginning with a cutscene
    /// </summary>
    public void PlayLevel()
    {
        GameManagerController.Instance.LoadScene("CutScene");
    }

    /// <summary>
    /// Transitions back to Main Menu
    /// </summary>
    public void LoadMainMenu()
    {
        GameManagerController.Instance.LoadScene("MainMenu");
    }

    /// <summary>
    /// Restarts the current scene
    /// </summary>
    public void ReloadCurrentLevel()
    {
        GameManagerController.Instance.LoadScene(SceneManager.GetActiveScene().name);
    }

    /// <summary>
    /// Quits the game
    /// </summary>
    public void QuitButton()
    {
        Application.Quit();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="newScreen"></param>
    public void ChangeScreen(GameObject newScreen)
    {
        if (CurrentScreen != null)
        {
            CurrentScreen.SetActive(false);
        }
        newScreen.SetActive(true);
        CurrentScreen = newScreen;
    }
}
