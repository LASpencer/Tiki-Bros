using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
            GameManagerController.Instance.AllowSceneActivation();
        }
        if (GameManagerController.Instance.SceneFinished)
        {
            SkipText.text = "CLICK TO SKIP";
        } else
        {
            SkipText.text = "LOADING";
        }
	}
}