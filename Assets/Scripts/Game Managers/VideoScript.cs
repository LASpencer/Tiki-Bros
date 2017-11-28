using UnityEngine;
using UnityEngine.UI;
using System.Collections;

/// <summary>
/// Manages cutscene
/// </summary>
[RequireComponent (typeof(AudioSource))]

public class VideoScript : MonoBehaviour {

	public MovieTexture CutSceneMaterial;

    public Text SkipText;

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
        // On click, allow game level to activate
		CutSceneMaterial.Stop ();
        GameManagerController.Instance.AllowSceneActivation();
    }

	void Update()
	{
        if (!GameManagerController.Instance.SceneLoading)
        {
            //HACK Start is called before GameManagerController calls FinishSceneLoad
            // so it won't accept it. 
            GameManagerController.Instance.LoadScene("S1_Tutorial", true, false);
        }
		if (!CutSceneMaterial.isPlaying)
        {
            // Allow activation when cutscene ends
            GameManagerController.Instance.AllowSceneActivation();
        }
        if (GameManagerController.Instance.SceneFinished)
        {
            // When scene finishes loading, tell player they can activate the level
            SkipText.text = "CLICK TO SKIP";
        } else
        {
            SkipText.text = "LOADING...";
        }
	}
}