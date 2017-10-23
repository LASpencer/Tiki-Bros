using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// Holds common behaviours and functions for menus
public class MenuManager : MonoBehaviour
{
    public void PlayLevel()
    {
        SceneManager.LoadScene("S1_Tutorial v2");
    }

    //TODO move stuff from PauseScreen into here
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void ReloadCurrentLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
}
