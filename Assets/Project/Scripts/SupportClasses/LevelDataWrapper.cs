﻿using System;
using UnityEngine;
using UnityEngine.Video;

[Serializable]
public class LevelDataWrapper
{
    int index = 0; // Index of the level in the list
    public LevelData level;
    public string SceneName;
    public Texture2D Planet;
    public Texture2D LevelScreen;
    public bool comingSoon = false;
    public VideoClip CutScene;

    public int Index { get => index; set => index = value; }

    public LevelDataWrapper()
    {
        Index = 0;
        level = new LevelData();
        SceneName = string.Empty;
        Planet = null;
        LevelScreen = null;
        comingSoon = false;
        CutScene = null;
    }

    public bool IsLocked() { return level.checkpointIndex == -1; }

    public void Unlock() 
    {
        if (comingSoon) return;
        level.checkpointIndex = 0; 
    }
}

