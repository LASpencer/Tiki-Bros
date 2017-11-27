using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public int pointsToAdd;

    public float rotateSpeed = 80f;

    public AudioClip CollectSound;

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() == null)
            return;
		ScoreManager.scoreValue += pointsToAdd;
        GameManagerController.Instance.CoinsCollected++;

        //Play audio
        //AudioSource.PlayClipAtPoint(CollectSound, transform.position);
        Camera.main.gameObject.GetComponent<AudioSource>().PlayOneShot(CollectSound); //HACK make nicer way to get camera's audio source

        Destroy(gameObject);

    }

    void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
    }
}
