using System;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointManager : MonoBehaviour
{
    public static CheckPointManager Instance { get; private set; }

    [SerializeField] CameraController maincamera;
    [SerializeField] float defaultCameraSize = 10f;

    [ReadOnly][SerializeField] List<CheckPoint> checkPoints;

    [ReadOnly][SerializeField] int currentCheckPointIndex = 0;

    public Transform GetSpawnPoint()
    {
        return checkPoints[currentCheckPointIndex].Spawnpoint;
    }

    private void Awake()
    {
        SetInstance();
    }

    private void Start()
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
        }

        currentCheckPointIndex = 0;
    }

    private void OnCheckPointReached(int id, Transform newCameraPosition, bool changeCameraSize, float cameraSize)
    {
        currentCheckPointIndex = id;

        if (changeCameraSize)
        { 
            maincamera.GetComponent<Camera>().orthographicSize = cameraSize;
        }
        else
        {
            maincamera.GetComponent<Camera>().orthographicSize = defaultCameraSize;
        }


        if (checkPoints[currentCheckPointIndex].Type != CheckPointType.end)
        {
            maincamera.PanCamera(newCameraPosition);
        }
        else
        {
            Debug.Log("End of the game reached. No more checkpoints to pan to.");
        }
    }

    private void SetInstance()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
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
}
