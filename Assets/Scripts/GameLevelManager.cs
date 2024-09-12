using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelManager : MonoBehaviour
{
    public GameManager gameManager;
    public GameLevelReader levelReader;

    public int currentLevel = 0;
    private const int MAX_LEVEL = 8;
    private const string LEVEL_PATH = "Level/Level_";

    private void Start()
    {
        LoadCurrentLevel();
    }

    public void LoadCurrentLevel()
    {

        string levelPath = LEVEL_PATH + currentLevel;
        TextAsset levelTextAsset = Resources.Load<TextAsset>(levelPath);

        if (levelTextAsset != null)
        {
            levelReader.LoadLevel(levelTextAsset);
        }
        else
        {
            Debug.LogError($"Could not load level at path: {levelPath}");
        }
    }

    public void NextLevel()
    {
        if (currentLevel < MAX_LEVEL)
        {
            gameManager.DestroyCurrentLevel();
            currentLevel++;
            LoadCurrentLevel();
        }
    }

    public void PreviousLevel()
    {
        if (currentLevel > 0)
        {
            currentLevel--;
            LoadCurrentLevel();
        }
    }

    public void RestartLevel()
    {
        LoadCurrentLevel();
    }

    public void OnLevelComplete()
    {
        NextLevel();
    }
}

