using System;
using UnityEngine;

[Serializable]
public class LevelDataWrapper
{
    int index = 0; // Index of the level in the list
    public LevelData level;
    public string SceneName;
    public Texture2D Planet;
    public Texture2D LevelScreen;
    public bool comingSoon = false;

    public int Index { get => index; set => index = value; }

    public LevelDataWrapper()
    {
        Index = 0;
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

