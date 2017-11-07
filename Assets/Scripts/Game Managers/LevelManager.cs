using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Checkpoint currentCheckpoint;

	public Scene currentScene;

    public Canvas PauseScreen;

    private PlayerController player;

    private bool isPaused;

    public bool IsPaused { get { return isPaused; } }

    public float TimeScale = 1;

	// Use this for initialization
	void Start ()
    {
        player = FindObjectOfType<PlayerController>();

        // Initialize pause stuff as unpaused
        PauseScreen.gameObject.SetActive(false);//Hide pause screen		
        isPaused = false;
        Time.timeScale = TimeScale;

        // Reset coin count and check coins needed
        GameManagerController.Instance.CoinsCollected = 0;
        GameManagerController.Instance.TotalCoins = GameObject.FindGameObjectsWithTag("Coin").Length;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Pause"))
        {
            if (isPaused)
            {
                Unpause();
            } else
            {
                Pause();
            }
        }
	}

    public void RespawnPlayer ()
    {
        player.transform.position = currentCheckpoint.spawnPoint.transform.position;
		player.currentlives = player.currentlives  - 1;
        Debug.Log(" Respawn player");

		if (player.currentlives <= player.minlives) 
		{
			player.currentlives = 0;
            //TODO rewrite killing player ie play animation then go to game over scene
            Cursor.lockState = CursorLockMode.None;
            GameManagerController.Instance.LoadScene("GameOver");
		}
    }

    public void Pause()
    {
        isPaused = true;
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
        PauseScreen.gameObject.SetActive(true);
        Debug.Log("pause");
    }

    public void Unpause()
    {
        isPaused = false;
        Time.timeScale = TimeScale;
        Cursor.lockState = CursorLockMode.Locked;
        PauseScreen.gameObject.SetActive(false);
        Debug.Log("unpause");
    }

    void OnDestroy()
    {
        // Make sure time is started again
        Time.timeScale = 1;
    }
}
