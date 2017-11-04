using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerController : MonoBehaviour {

    public static GameManagerController Instance;

    public int CoinsCollected;

    public int TotalCoins;

    public Canvas LoadingCanvas;

    bool sceneLoading = false;

    // Whether a scene is being loaded
    public bool SceneLoading { get { return sceneLoading; } }

    void Awake()
    {
        if(Instance == null)
        {
            DontDestroyOnLoad(gameObject);
            Instance = this;
        } else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

	// Use this for initialization
	void Start () {
        LoadingCanvas.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void LoadScene(string sceneName)
    {
        if (!sceneLoading)
        {
            StartCoroutine(AsyncSceneLoad(sceneName));
        }
    }

    void StartSceneLoad()
    {
        sceneLoading = true;
        LoadingCanvas.enabled = true;
    }

    void FinishSceneLoad()
    {
        sceneLoading = false;
        LoadingCanvas.enabled = false;
    }

    IEnumerator AsyncSceneLoad(string sceneName)
    {
        StartSceneLoad();
        yield return null;
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        while (!asyncLoad.isDone)
        {
            // TODO instead set value of loading bar in canvas
            // Loading goes from 0 to 0.9, with 1.0 meaning done
            float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
            Debug.Log("Loading: " + (progress * 100) + "%");
            yield return null;
        }
        FinishSceneLoad();
    }
}
