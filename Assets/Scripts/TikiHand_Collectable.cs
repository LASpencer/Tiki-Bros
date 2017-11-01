using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TikiHand_Collectable : MonoBehaviour {


    public float rotateSpeed = 35f;

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerController>() == null)
            return;
        Destroy(gameObject);

        // Currently displays Game Over screen.
        SceneManager.LoadScene("GameOver");

        // Check for coins
        // if all coins not collected load bad scene.
        // Else, load good ending.


    }

    void Update()
    {
        transform.Rotate(0, rotateSpeed * Time.deltaTime,  0);
    }
}
