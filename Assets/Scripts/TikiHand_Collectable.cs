using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


//TODO maybe this should inherit from collectable?
public class TikiHand_Collectable : MonoBehaviour {

    public float rotateSpeed = 35f;
	public GameObject FadePanel;

	/*
	IEnumerator WaitForFade()
	{
		//FadePanel.SetActive(true);
		yield return new WaitForSeconds(4);
	}
	*/

	 void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Player")){
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
