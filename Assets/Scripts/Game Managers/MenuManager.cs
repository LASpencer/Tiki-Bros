using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// Holds common behaviours and functions for menus
public class MenuManager : MonoBehaviour
{
    protected GameObject CurrentScreen;

    public void PlayLevel()
    {
        //SceneManager.LoadScene("S1_Tutorial");
        GameManagerController.Instance.LoadScene("CutScene");
    }

    //TODO move stuff from PauseScreen into here
    public void LoadMainMenu()
    {
        GameManagerController.Instance.LoadScene("MainMenu");
    }

    public void ReloadCurrentLevel()
    {
        GameManagerController.Instance.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitButton()
    {
        Application.Quit();
    }
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
