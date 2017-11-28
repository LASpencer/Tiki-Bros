using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script for Tiki Hand, which ends the game on being collected
/// </summary>
public class TikiHand_Collectable : MonoBehaviour {


    public float rotateSpeed = 35f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            // If picked up by player, end the game
            Destroy(gameObject);
			GameManagerController gameManager = GameManagerController.Instance;
			if (gameManager.CoinsCollected == gameManager.TotalCoins) {
				GameManagerController.Instance.LoadScene("WinScene");
			} else {
				GameManagerController.Instance.LoadScene("LoseScene");
			}    
            
        }

    }

    void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime,  0);
    }
}
