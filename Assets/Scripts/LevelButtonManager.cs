using UnityEngine;
using UnityEngine.UI;

public class LevelButtonManager : MonoBehaviour
{
    public Button[] levelButtons; 
    public int unlockedLevel = 0;
    public GameLevelManager gameLevelManager;

    public GameObject[] lockLevels;   // Mảng các đối tượng lockLevel
    public GameObject[] unlockLevels; // Mảng các đối tượng unlockLevel

    private const string UNLOCKED_LEVEL_KEY = "UnlockedLevel"; // Khóa để lưu level đã mở khóa

    private void Start()
    {
        LoadUnlockedLevel(); // Tải trạng thái level đã mở khóa
        SetupLevelButtons();  // Cài đặt các button dựa trên trạng thái mở khóa
    }

    // Tải level đã mở khóa từ PlayerPrefs
    private void LoadUnlockedLevel()
    {
        if (PlayerPrefs.HasKey(UNLOCKED_LEVEL_KEY))
        {
            unlockedLevel = PlayerPrefs.GetInt(UNLOCKED_LEVEL_KEY); 
        }
        else
        {
            unlockedLevel = 0; 
        }
    }

    // Cài đặt các button level theo trạng thái mở khóa
    private void SetupLevelButtons()
    {
        for (int i = 0; i < levelButtons.Length; i++)
        {
            int level = i;
            bool isUnlocked = level <= unlockedLevel;

            // Cài đặt trạng thái tương tác của button
            levelButtons[i].interactable = isUnlocked;

            // Bật unlockLevel và tắt lockLevel nếu level đã mở khóa
            if (unlockLevels[i] != null && lockLevels[i] != null)
            {
                unlockLevels[i].SetActive(isUnlocked);
                lockLevels[i].SetActive(!isUnlocked);
            }

            // Đăng ký sự kiện click nếu button tương tác được
            if (isUnlocked)
            {
                levelButtons[i].onClick.RemoveAllListeners(); // Xóa các listener cũ để tránh đăng ký nhiều lần
                levelButtons[i].onClick.AddListener(() => LoadLevel(level));
            }
        }
    }

    // Phương thức để mở khóa level tiếp theo
    public void UnlockNextLevel(int currentLevel)
    {
        if (currentLevel >= unlockedLevel)
        {
            unlockedLevel = currentLevel + 1; // Mở khóa level tiếp theo
            SaveUnlockedLevel(); // Lưu trạng thái mới
            SetupLevelButtons(); // Cập nhật lại các button và đối tượng khóa/mở khóa
        }
    }

    // Lưu trạng thái level đã mở khóa vào PlayerPrefs
    private void SaveUnlockedLevel()
    {
        PlayerPrefs.SetInt(UNLOCKED_LEVEL_KEY, unlockedLevel);
        PlayerPrefs.Save(); // Lưu dữ liệu xuống bộ nhớ
    }

    private void LoadLevel(int level)
    {
        if (level <= unlockedLevel)
        {
            gameLevelManager.currentLevel = level;
            gameLevelManager.LoadCurrentLevel();
        }
    }
}
