using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//TODO maybe this should inherit from collectable?
public class TikiHand_Collectable : MonoBehaviour {


    public float rotateSpeed = 35f;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")){
            Destroy(gameObject);

            // Currently displays Game Over screen.
            GameManagerController.Instance.LoadScene("GameOver");
        }
        // Check for coins
        // if all coins not collected load bad scene.
        // Else, load good ending.


    }

    void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime,  0);
    }
}
