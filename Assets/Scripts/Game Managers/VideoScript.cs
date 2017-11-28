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
                // If at end text, start level
                GameManagerController.Instance.AllowSceneActivation();
            } else
            {
                // If in cutscene, end cutscene
                CutSceneMaterial.Stop();
            }
        }
        else
        {
            // If at starting text, start cutscene
            StartCutscene();
        }
	}

	void Update()
	{
        if (!GameManagerController.Instance.SceneLoading)
        {
            //HACK Start is called before GameManagerController calls FinishSceneLoad
            // so it won't accept a new scene there, and it must go here
            GameManagerController.Instance.LoadScene("S1_Tutorial", true, false);
        }
		if (cutsceneStarted && !CutSceneMaterial.isPlaying)
        {
            // When cutscene ends, show end text
			FinishTextGroup.SetActive (true);
        }

        if(GameManagerController.Instance.SceneFinished)
        {
            // Prompt to continue past text or skip cutscene
            if (CutSceneMaterial.isPlaying)
            {
                SkipText.text = "CLICK TO SKIP";
            } else
            {
                SkipText.text = "CLICK TO CONTINUE";
            }
        } else
        {
            // While not loaded, say it's loading
            SkipText.text = "LOADING";
        }
	}

    /// <summary>
    /// Closes the Start Text and begins playing the cutscene
    /// </summary>
	public void StartCutscene(){
		cutsceneStarted = true;
		StartTextGroup.SetActive (false);
		CutSceneMaterial.Play();
	}
}