using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PersistenceManager : MonoBehaviour
{
    public static PersistenceManager Instance { get; private set; }
    public List<LevelDataWrapper> LevelDataWrappers { get => levelDataWrappers; }

    [SerializeField] private List<LevelDataWrapper> levelDataWrappers = new List<LevelDataWrapper>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        InitData();
        LoadData();
    }

    public void InitData()
    {
        // Initialize save file
        for (int i = 0; i < levelDataWrappers.Count; i++)
        {
            string levelName = "level" + i;

            levelDataWrappers[i].Index = i;

            levelDataWrappers[i].level = new LevelData(levelName);

            if (i == 0)
            { 
                levelDataWrappers[i].level.checkpointIndex = 0; // Unlock the first level
            }
        }
    }

    public void SaveData()
    {
        // Save each level's data in PlayerPrefs
        for (int i = 0; i < levelDataWrappers.Count; i++)
        {
            LevelData levelData = levelDataWrappers[i].level;
            string levelName = "level" + i;
            int isCompleted = levelData.isCompleted ? 1 : 0;
            PlayerPrefs.SetInt(levelName + "checkpointIndex", levelData.checkpointIndex);
            PlayerPrefs.SetInt(levelName + "playerLives", levelData.playerLives);
            PlayerPrefs.SetInt(levelName + "collectibleRecord", levelData.collectibleRecord);
            PlayerPrefs.SetInt(levelName + "isCompleted", isCompleted);
        }
    }

    public void UnlockNextLevel(int levelIndex)
    {
        int newIndex = levelIndex + 1;

        if (newIndex >= levelDataWrappers.Count)
        {
            Debug.LogWarning("No more levels to unlock.");
            return;
        }

        UpdateDataForLevel(newIndex, 0);
    }

    public void UpdateDataForLevel(int levelIndex, int checkpointIndex, int playerLives = 3, int collectibleRecord = 0, bool isCompleted = false)
    {
        string levelName = levelDataWrappers[levelIndex].level.levelName;
        levelDataWrappers[levelIndex].level = new LevelData(levelName, checkpointIndex, playerLives, collectibleRecord, isCompleted);
        SaveData();
    }

    public void LoadData()
    {
        // load PlayerPrefs if they exist
        for (int i = 0; i < levelDataWrappers.Count; i++)
        {
            string levelName = "level" + i;
            if (PlayerPrefs.HasKey(levelName + "checkpointIndex"))
            {
                int checkpointIndex = PlayerPrefs.GetInt(levelName + "checkpointIndex");
                int playerLives = PlayerPrefs.GetInt(levelName + "playerLives");
                int collectibleRecord = PlayerPrefs.GetInt(levelName + "collectibleRecord");
                bool isCompleted = PlayerPrefs.GetInt(levelName + "isComplete") != 0 ? true : false;
                levelDataWrappers[i].level = new LevelData(levelName, checkpointIndex, playerLives, collectibleRecord, isCompleted);
            }
        }
    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteAll();
        InitData();
    }
}
