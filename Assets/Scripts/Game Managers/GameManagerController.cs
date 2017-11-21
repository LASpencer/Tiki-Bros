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

    private bool sceneFinished = false;

    public bool SceneFinished { get { return sceneFinished; } }

    private AsyncOperation loadingOperation;

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
        Application.backgroundLoadingPriority = ThreadPriority.Normal;
    }

	// Use this for initialization
	void Start () {
        LoadingCanvas.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    
    public void AllowSceneActivation()
    {
        if(loadingOperation != null)
        {
            loadingOperation.allowSceneActivation = true;
        }
    }

    public void LoadScene(string sceneName, bool waitToActivate = false, bool showCanvas = true)
    {
        if (!sceneLoading)
        {
            StartCoroutine(AsyncSceneLoad(sceneName, waitToActivate));
            LoadingCanvas.enabled = showCanvas;
        }
    }

    void StartSceneLoad()
    {
        sceneLoading = true;
        sceneFinished = false;
    }

    void FinishSceneLoad()
    {
        sceneLoading = false;
        LoadingCanvas.enabled = false;
        loadingOperation = null;
    }

    IEnumerator AsyncSceneLoad(string sceneName, bool waitToActivate)
    {
        StartSceneLoad();
        yield return null;
        loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        loadingOperation.allowSceneActivation = !waitToActivate;
        while (!loadingOperation.isDone)
        {
            // TODO instead set value of loading bar in canvas
            // Loading goes from 0 to 0.9, with 1.0 meaning done
            float progress = Mathf.Clamp01(loadingOperation.progress / 0.9f);
            Debug.Log("Loading: " + (progress * 100) + "%");
            
            sceneFinished = loadingOperation.progress >= 0.9f;
            //TODO maybe have an option to hide the LoadingCanvas on finish?
            yield return null;
        }
        FinishSceneLoad();
    }
}
