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

public struct LevelData
{
    public string levelName;
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
    
    // Complete constructor
    public LevelData(string _levelName, int _checkpointIndex, int _playerLives, int _collectibleRecord)
    {
        levelName = _levelName;
        checkpointIndex = _checkpointIndex;
        playerLives = _playerLives;
        collectibleRecord = _collectibleRecord;
    }
}