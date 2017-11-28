using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Singleton object which persists between scenes. It is responsible
/// for managing scene transitions as well as containing any data which
/// needs to be used by multiple scenes
/// </summary>
public class GameManagerController : MonoBehaviour {

    public static GameManagerController Instance;

    public int CoinsCollected;

    public int TotalCoins;

    [Tooltip("Canvas holding default elements to display while loading")]
    public Canvas LoadingCanvas;

    bool sceneLoading = false;

    private bool sceneFinished = false;

    // Is a scene ready to be activated?
    public bool SceneFinished { get { return sceneFinished; } }

    private AsyncOperation loadingOperation;

    // Is a scene currently loading?
    public bool SceneLoading { get { return sceneLoading; } }

    void Awake()
    {
        if(Instance == null)
        {
            // Keep instance alive between scenes
            DontDestroyOnLoad(gameObject);
            Instance = this;
        } else if (Instance != this)
        {
            // Destroy redundant instances
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
    
    /// <summary>
    /// Tells scene loading async operation to activate the scene
    /// </summary>
    public void AllowSceneActivation()
    {
        if(loadingOperation != null)
        {
            loadingOperation.allowSceneActivation = true;
        }
    }

    /// <summary>
    /// Asynchronously loads the requested scene, if there isn't already one in the queue
    /// </summary>
    /// <param name="sceneName">Scene to load</param>
    /// <param name="waitToActivate">Whether to wait for permission to activate the scene</param>
    /// <param name="showCanvas">Whether to show the loading canvas</param>
    public void LoadScene(string sceneName, bool waitToActivate = false, bool showCanvas = true)
    {
        if (!sceneLoading)
        {
            StartCoroutine(AsyncSceneLoad(sceneName, waitToActivate));
            LoadingCanvas.enabled = showCanvas;
        }
    }

    /// <summary>
    /// Sets flags for scene being loaded
    /// </summary>
    void StartSceneLoad()
    {
        sceneLoading = true;
        sceneFinished = false;
    }

    /// <summary>
    /// Cleans up after scene load ends
    /// </summary>
    void FinishSceneLoad()
    {
        sceneLoading = false;
        LoadingCanvas.enabled = false;
        loadingOperation = null;
    }

    /// <summary>
    /// Coroutine which creates an asynchronous operation to load a scene and provides updates on its progress
    /// </summary>
    /// <param name="sceneName">Scene to load</param>
    /// <param name="waitToActivate">Whether async operation requires permission before activating scene</param>
    /// <returns></returns>
    IEnumerator AsyncSceneLoad(string sceneName, bool waitToActivate)
    {
        StartSceneLoad();
        yield return null;
        // Create operation to load scene
        loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        loadingOperation.allowSceneActivation = !waitToActivate;
        // Do loop every frame until scene loaded and activated
        while (!loadingOperation.isDone)
        {
            // Loading goes from 0 to 0.9, with 1.0 meaning done
            float progress = Mathf.Clamp01(loadingOperation.progress / 0.9f);
            Debug.Log("Loading: " + (progress * 100) + "%");
            
            // Sets SceneFinished true when scene is fully loaded
            sceneFinished = loadingOperation.progress >= 0.9f;
            yield return null;
        }
        FinishSceneLoad();
    }
}
