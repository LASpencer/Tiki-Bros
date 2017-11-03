using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
