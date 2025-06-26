using System;
using UnityEngine;

[Serializable]
public class LevelDataWrapper
{
    public LevelData level;
    public string SceneName;
    public Texture2D Planet;
    public Texture2D LevelScreen;
    public bool comingSoon = false;

    public LevelDataWrapper()
    {
        level = new LevelData();
        SceneName = string.Empty;
        Planet = null;
        LevelScreen = null;
        comingSoon = false;
    }

    public bool IsLocked() { return level.checkpointIndex == -1; }

    public void Unlock() 
    {
        if (comingSoon) return;
        level.checkpointIndex = 0; 
    }
}

