using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script for coins, treasure, and other pickups
/// </summary>
public class Collectable : MonoBehaviour
{
    public int pointsToAdd;

    public float rotateSpeed = 80f;

    public AudioClip CollectSound;

    void OnTriggerEnter(Collider other)
    {
        // If touched by player, player gets points and treasure destroyed
        if (other.GetComponent<PlayerController>() == null)
        {
            return;
        }

		ScoreManager.scoreValue += pointsToAdd;
        GameManagerController.Instance.CoinsCollected++;

        //Play audio
        Camera.main.gameObject.GetComponent<AudioSource>().PlayOneShot(CollectSound); 

        Destroy(gameObject);

    }

    void Update()
    {
        // Treasure slowly rotates
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }
}
