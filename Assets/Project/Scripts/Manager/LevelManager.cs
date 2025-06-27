using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using utilities.Controllers;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance { get; private set; }

    [Header("Level Info")]
    [ReadOnly][SerializeField] LevelDataWrapper currentLevelDataWrapper;
    [ReadOnly][SerializeField] List<CheckPoint> checkPoints;
    [ReadOnly][SerializeField] int currentCheckPointIndex = 0;

    [Header("Camera Settings")]
    [SerializeField] float defaultCameraSize = 10f;

    [Header("Prefabs")]
    [SerializeField] GameObject SnailPrefab;

    [ReadOnly][SerializeField] GameObject currentSnail;
    CameraController currentCamera;

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
    }

    public void SetCurrentLevelDataWrapper(LevelDataWrapper levelDataWrapper)
    {
        currentLevelDataWrapper = levelDataWrapper;
        Initialize();
    }

    private void Initialize()
    {
        currentCamera = Camera.main.GetComponent<CameraController>();
        LoadCheckPoints();

        currentSnail = Instantiate(SnailPrefab, Vector3.zero, Quaternion.identity);
        currentSnail.GetComponent<HealthController>().CurrentHp = currentLevelDataWrapper.level.playerLives;
        ScoreManager.Instance.SetScore(currentLevelDataWrapper.level.collectibleRecord);

        SetCheckPoint(currentLevelDataWrapper.level.checkpointIndex);
    }

    private void LoadCheckPoints()
    {
        GameObject[] checkPointObjects = GameObject.FindGameObjectsWithTag("CheckPoint");

        checkPoints = new List<CheckPoint>();

        foreach (GameObject checkPointObject in checkPointObjects)
        {
            if (checkPointObject.TryGetComponent<CheckPoint>(out CheckPoint checkPoint))
            {
                checkPoint.CheckPointReached += OnCheckPointReached;
                AddInOrderByDistance(ref checkPoint);
            }
        }

        for (int i = 0; i < checkPoints.Count; i++)
        {
            checkPoints[i].Id = i;

            if (i == 0)
            {
                checkPoints[i].Type = CheckPointType.start;
            }

            if (i == checkPoints.Count - 1)
            {
                checkPoints[i].Type = CheckPointType.end;
            }

            if (checkPoints[i].Flag != null)
            { 
                checkPoints[i].Flag.SetActive(false);
            }
        }

        currentCheckPointIndex = 0;
    }

    private void AddInOrderByDistance(ref CheckPoint checkPoint)
    {
        Vector3 startingPoint = Vector3.zero;

        for (int i = 0; i < checkPoints.Count; i++)
        {
            if (Vector3.Distance(startingPoint, checkPoint.transform.position) < Vector3.Distance(startingPoint, checkPoints[i].transform.position))
            {
                checkPoints.Insert(i, checkPoint);
                return;
            }
        }

        checkPoints.Add(checkPoint);
    }

    public void SetCheckPoint(int checkPointIndex)
    {
        checkPoints[checkPointIndex].SpawnOnCheckPoint(currentSnail);
    }

    public Transform GetSpawnPoint()
    {
        return checkPoints[currentCheckPointIndex].Spawnpoint;
    }

    public void EndLevel(bool levelFinished = true)
    {
        SaveLevelData(levelFinished);

        if (levelFinished)
        { 
            currentLevelDataWrapper.level.isCompleted = true;
            PersistenceManager.Instance.UnlockNextLevel(currentLevelDataWrapper.Index);
        }

        PersistenceManager.Instance.SaveData();

        //END LEVEL Graphics
        UIController.Instance.ReturnToMenu();
    }

    private void SaveLevelData(bool levelFinished = false)
    {
        // Bisogna passare le vite del giocatore, 
        int checkPointIndex = currentCheckPointIndex;
        int playerLives = currentSnail.GetComponent<HealthController>().CurrentHp;
        int collectibleRecord = ScoreManager.Instance.GetCollected;

        PersistenceManager.Instance.UpdateDataForLevel(currentLevelDataWrapper.Index, checkPointIndex, playerLives, collectibleRecord, levelFinished);
    }

    private void OnCheckPointReached(int id, Transform newCameraPosition, bool changeCameraSize, float cameraSize)
    {
        currentCheckPointIndex = id;

        if (changeCameraSize)
        {
            currentCamera.GetComponent<Camera>().orthographicSize = cameraSize;
        }
        else
        {
            currentCamera.GetComponent<Camera>().orthographicSize = defaultCameraSize;
        }

        // Save checkpoint reached and records so far
        SaveLevelData();

        if (checkPoints[currentCheckPointIndex].Type != CheckPointType.end)
        {
            currentCamera.PanCamera(newCameraPosition);
        }
        else
        {
            EndLevel();
        }
    }

    // Reloads scene which restarts player at the last checkpoint reached and takes away a life
    public void ResetToCheckPoint()
    {
        int checkPointIndex = currentCheckPointIndex;
        int playerLives = currentSnail.GetComponent<HealthController>().CurrentHp - 1;
        int collectibleRecord = currentLevelDataWrapper.level.collectibleRecord; // Reset to collectibles from previous checkpoint
        bool levelFinished = false;

        if(playerLives == 0) return; // Don't allow reset

        PersistenceManager.Instance.UpdateDataForLevel(currentLevelDataWrapper.Index, checkPointIndex, playerLives, collectibleRecord, levelFinished);

        SceneManager.LoadScene(currentLevelDataWrapper.SceneName);
        //SceneManager.LoadScene("Giovanni"); // DEBUG
    }
}
