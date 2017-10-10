using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public GameObject currentCheckpoint;

	public Scene currentScene;

    private PlayerController player;




	// Use this for initialization
	void Start ()
    {
        player = FindObjectOfType<PlayerController>();		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void RespawnPlayer ()
    {
        player.transform.position = currentCheckpoint.transform.position;
		player.currentlives = -1;
        Debug.Log(" Respawn player");
    }
}
