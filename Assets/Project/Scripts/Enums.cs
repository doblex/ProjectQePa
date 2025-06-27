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

[Serializable]
public struct LevelData
{
    [ReadOnly] public string levelName;
    public int checkpointIndex;
    public int playerLives;
    public int collectibleRecord;

    // default constructor for empty levelData
    public LevelData(string _levelName)
    {
        levelName = _levelName;
        checkpointIndex = -1;
        playerLives = 3;
        collectibleRecord = 0;
    }

    public LevelData(string _levelName, int _checkpointIndex)
    {
        levelName = _levelName;
        checkpointIndex = _checkpointIndex;
        playerLives = 3; // Default lives
        collectibleRecord = 0; // Default collectible record
    }

    // Complete constructor
    public LevelData(string _levelName, int _checkpointIndex, int _playerLives, int _collectibleRecord)
    {
        levelName = _levelName;
        checkpointIndex = _checkpointIndex;
        playerLives = _playerLives;
        collectibleRecord = _collectibleRecord;
    }
}