using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]

public class VideoScript : MonoBehaviour {

	public MovieTexture CutSceneMaterial;

	private MeshRenderer meshRenderer;

	void Start(){
		meshRenderer = GetComponent<MeshRenderer>();
		meshRenderer.material.mainTexture = CutSceneMaterial;

		AudioSource audio = GetComponent<AudioSource>();
		audio.Play();

		CutSceneMaterial.Play ();
	}

	void OnMouseDown()
	{
		CutSceneMaterial.Stop ();
		//Application.LoadLevel("S1_Tutorial");
		GameManagerController.Instance.LoadScene("S1_Tutorial"); 
	}

	void Update()
	{
		if (!CutSceneMaterial.isPlaying) GameManagerController.Instance.LoadScene("S1_Tutorial");
	}
}