﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
	public static GameManager Instance { get; private set; }
	private PlayerController player;

	public PlayerController Player
	{
		get => player;
		set => player = value;
	}

	private int deathsPlayerCount = 0;

	public int DeathsPlayerCount
	{
		get => deathsPlayerCount;
		set => deathsPlayerCount = value;
	}

    private int wavesCount = 0;

    public int WavesCount
    {
        get => wavesCount;
        set => wavesCount = value;
    }

	private void OnEnable()
	{
		SceneManager.sceneLoaded += OnLevelFinishedLoadingScene;
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			Application.Quit();
		}
	}

	private void OnDisable()
	{
		SceneManager.sceneLoaded -= OnLevelFinishedLoadingScene;
	}

	//this function is activated every time a scene is loaded
	private void OnLevelFinishedLoadingScene(Scene scene, LoadSceneMode mode)
	{
		Setup();
		Debug.Log("Scene Loaded");
	}

	private void Setup()
	{
		player = FindObjectOfType<PlayerController>();
		//player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
	}

	private void Awake()
	{
		if (Instance != null && Instance != this)
		{
			Destroy(gameObject);
		}
		else
		{
			Instance = this;
			DontDestroyOnLoad(gameObject);
		}

		Setup();
	}

	public void LoadLevel(string nameLevel)
	{
		SceneManager.LoadScene(nameLevel);
	}

    public void End()
    {
        UIManager.Instance.End();
    }


	public void QuitGame()
	{
#if UNITY_EDITOR
		UnityEditor.EditorApplication.isPlaying = false;
#else
		Application.Quit();
#endif
	}
}