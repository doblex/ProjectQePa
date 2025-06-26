using System.Collections.Generic;
using UnityEngine;

public class PersistenceManager : MonoBehaviour
{
    [SerializeField] private int levelNumber = 4;
    private List<LevelData> levelDatas = new List<LevelData>();

    private void Awake()
    {
        InitData();
        LoadData();
    }

    public void InitData()
    {
        // Initialize save file
        for (int i = 0; i < levelNumber; i++)
        {
            string levelName = "level" + i;
            levelDatas[i] = new LevelData(levelName);
        }
    }

    public void SaveData()
    {
        // Save each level's data in PlayerPrefs
        for (int i = 0; i < levelNumber; i++)
        {
            LevelData levelData = levelDatas[i];
            string levelName = "level" + i;
            PlayerPrefs.SetInt(levelName + "checkpointIndex", levelData.checkpointIndex);
            PlayerPrefs.GetInt(levelName + "playerLives", levelData.playerLives);
            PlayerPrefs.GetInt(levelName + "collectibleRecord", levelData.collectibleRecord);
        }
    }

    public void UpdateDataForLevel(int levelIndex, int checkpointIndex, int playerLives, int collectibleRecord)
    {
        string levelName = levelDatas[levelIndex].levelName;
        levelDatas[levelIndex] = new LevelData(levelName, checkpointIndex, playerLives, collectibleRecord);
    }

    public void LoadData()
    {
        // load PlayerPrefs if they exist
        for (int i = 0; i < levelNumber; i++)
        {
            string levelName = "level" + i;
            if (PlayerPrefs.HasKey(levelName + "checkpointIndex"))
            {
                int checkpointIndex = PlayerPrefs.GetInt(levelName + "checkpointIndex");
                int playerLives = PlayerPrefs.GetInt(levelName + "playerLives");
                int collectibleRecord = PlayerPrefs.GetInt(levelName + "collectibleRecord");
                levelDatas[i] = new LevelData(levelName, checkpointIndex, playerLives, collectibleRecord);
            }
        }
    }
}
