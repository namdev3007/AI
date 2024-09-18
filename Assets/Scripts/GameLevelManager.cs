using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLevelManager : MonoBehaviour
{
    public GameManager gameManager;
    public GameLevelReader levelReader;
    public LevelButtonManager levelButtonManager;
    public HomeUIManager homeUIManager; // Tham chiếu đến HomeUIManager

    public int currentLevel = 0;
    private const string LEVEL_PATH = "Level/Level_";
    private const string CURRENT_LEVEL_KEY = "CurrentLevel"; // Khóa để lưu level hiện tại

    private void Start()
    {
        LoadSavedLevel(); // Tải level đã lưu
        LoadCurrentLevel(); // Tải level hiện tại
    }

    // Tải level đã lưu từ PlayerPrefs
    private void LoadSavedLevel()
    {
        if (PlayerPrefs.HasKey(CURRENT_LEVEL_KEY))
        {
            currentLevel = PlayerPrefs.GetInt(CURRENT_LEVEL_KEY); // Lấy giá trị level đã lưu
        }
        else
        {
            currentLevel = 0; // Mặc định là level 0 nếu không có dữ liệu lưu
        }
    }

    public void LoadCurrentLevel()
    {
        string levelPath = LEVEL_PATH + currentLevel;
        TextAsset levelTextAsset = Resources.Load<TextAsset>(levelPath);
        levelReader.LoadLevel(levelTextAsset);

        homeUIManager.SetLevelText(currentLevel + 1); // Hiển thị currentLevel lên UI
    }

    public void NextLevel()
    {
        gameManager.DestroyCurrentLevel();
        currentLevel++;
        SaveCurrentLevel(); // Lưu level mới sau khi hoàn thành
        LoadCurrentLevel();
    }

    public void PreviousLevel()
    {
        if (currentLevel > 0)
        {
            currentLevel--;
            SaveCurrentLevel(); // Lưu level mới khi quay về level trước
            LoadCurrentLevel();
        }
    }

    public void ReloadLevel()
    {
        LoadCurrentLevel();
    }

    public void OnLevelComplete()
    {
        levelButtonManager.UnlockNextLevel(currentLevel); // Mở khóa level tiếp theo
        NextLevel();
    }

    // Lưu level hiện tại vào PlayerPrefs
    private void SaveCurrentLevel()
    {
        PlayerPrefs.SetInt(CURRENT_LEVEL_KEY, currentLevel);
        PlayerPrefs.Save(); // Lưu dữ liệu xuống bộ nhớ
    }
}
