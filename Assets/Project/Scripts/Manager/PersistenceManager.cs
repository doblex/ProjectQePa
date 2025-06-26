using System.Collections.Generic;
using UnityEngine;

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
            levelDataWrappers[i].level = new LevelData(levelName);
        }
    }

    public void SaveData()
    {
        // Save each level's data in PlayerPrefs
        for (int i = 0; i < levelDataWrappers.Count; i++)
        {
            LevelData levelData = levelDataWrappers[i].level;
            string levelName = "level" + i;
            PlayerPrefs.SetInt(levelName + "checkpointIndex", levelData.checkpointIndex);
            PlayerPrefs.GetInt(levelName + "playerLives", levelData.playerLives);
            PlayerPrefs.GetInt(levelName + "collectibleRecord", levelData.collectibleRecord);
        }
    }

    public void UpdateDataForLevel(int levelIndex, int checkpointIndex, int playerLives, int collectibleRecord)
    {
        string levelName = levelDataWrappers[levelIndex].level.levelName;
        levelDataWrappers[levelIndex].level = new LevelData(levelName, checkpointIndex, playerLives, collectibleRecord);
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
                levelDataWrappers[i].level = new LevelData(levelName, checkpointIndex, playerLives, collectibleRecord);
            }
        }
    }
}
