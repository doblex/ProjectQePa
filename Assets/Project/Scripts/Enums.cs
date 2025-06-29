using System;

public enum SelectionMode
{
    click,
    selection,
}

public enum SatelliteDirection
{
    clockwise,
    counterClockwise,
}

public enum CheckPointType
{
    normal,
    start,
    end
}

public enum AudioMode
{
    Once,
    Loop,
    Unselect
}

public enum DocFadeType
{
    FadeIn,
    FadeOut,
}

[Serializable]
public struct LevelData
{
    [ReadOnly] public string levelName;
    public int checkpointIndex;
    public int playerLives;
    public int collectibleRecord;
    public bool isCompleted;

    // default constructor for empty levelData
    public LevelData(string _levelName)
    {
        levelName = _levelName;
        checkpointIndex = -1;
        playerLives = 3;
        collectibleRecord = 0;
        isCompleted = false;
    }

    public LevelData(string _levelName, int _checkpointIndex)
    {
        levelName = _levelName;
        checkpointIndex = _checkpointIndex;
        playerLives = 3; // Default lives
        collectibleRecord = 0; // Default collectible record
        isCompleted = false; // level not marked as complete by default
    }

    // Complete constructor
    public LevelData(string _levelName, int _checkpointIndex, int _playerLives, int _collectibleRecord, bool _isCompleted)
    {
        levelName = _levelName;
        checkpointIndex = _checkpointIndex;
        playerLives = _playerLives;
        collectibleRecord = _collectibleRecord;
        isCompleted = _isCompleted;
    }
}