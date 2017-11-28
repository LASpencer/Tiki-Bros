using UnityEngine;
using UnityEngine.UI;
using System.Collections;

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
		if (cutsceneStarted) {
			GameManagerController.Instance.AllowSceneActivation ();
		} else {
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
		if (!cutsceneStarted) {
			SkipText.text = "CLICK TO CONTINUE";
		}
		else if	(GameManagerController.Instance.SceneFinished)
        {
            SkipText.text = "CLICK TO SKIP";
        } else
        {
            SkipText.text = "LOADING...";
        }
	}

	public void StartCutscene(){
		cutsceneStarted = true;
		StartTextGroup.SetActive (false);
		CutSceneMaterial.Play();
	}
}