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
	public GameObject StoryText1;
	public GameObject StartTextGroup;
	public GameObject FinishTextGroup;

	private MeshRenderer meshRenderer;

	bool cutsceneStarted;

	void Start(){
		meshRenderer = GetComponent<MeshRenderer>();
		meshRenderer.material.mainTexture = CutSceneMaterial;

		AudioSource audio = GetComponent<AudioSource>();
		audio.Play();

		cutsceneStarted = false;

		StartTextGroup.SetActive (true);
		FinishTextGroup.SetActive (false);

    }

	void OnMouseDown()
	{
        if (cutsceneStarted)
        {
            if (FinishTextGroup.activeInHierarchy)
            {
                GameManagerController.Instance.AllowSceneActivation();
            } else
            {
                CutSceneMaterial.Stop();
            }
        }
        else
        {
            StartCutscene();
        }
	}

	void Update()
	{
        if (!GameManagerController.Instance.SceneLoading)
        {
            //HACK Start is called before GameManagerController calls FinishSceneLoad
            // so it won't accept it. 
            GameManagerController.Instance.LoadScene("S1_Tutorial", true, false);
        }
		if (cutsceneStarted && !CutSceneMaterial.isPlaying)
        {
			FinishTextGroup.SetActive (true);
        }
		//if (!cutsceneStarted) {
		//	SkipText.text = "CLICK TO CONTINUE";
		//}
		//else if	(GameManagerController.Instance.SceneFinished)
  //      {
  //          // When scene finishes loading, tell player they can activate the level
            
  //      } else
  //      {
  //          SkipText.text = "LOADING...";
  //      }
        if(GameManagerController.Instance.SceneFinished)
        {
            if (CutSceneMaterial.isPlaying)
            {
                SkipText.text = "CLICK TO SKIP";
            } else
            {
                SkipText.text = "CLICK TO CONTINUE";
            }
        } else
        {
            SkipText.text = "LOADING";
        }
	}

	public void StartCutscene(){
		cutsceneStarted = true;
		StartTextGroup.SetActive (false);
		CutSceneMaterial.Play();
	}
}