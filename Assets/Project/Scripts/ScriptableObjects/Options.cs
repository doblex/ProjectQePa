using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Options", menuName = "Scriptable Objects/Options")]
[Serializable]
public class Options : ScriptableObject
{
    public List<string> Resolution;
    public int SelectedResolutionIndex = 3;

    public List<string> FPS;
    public int SelectedFPSIndex = 1;

    public List<CommandData> commands;

    public FullScreenMode Fullscreen;

    public float MasterVolume = 1.0f;
    public float SFXVolume = 1.0f;

    public Options()
    {
        Resolution = new List<string>
        {
            "640x480",
            "800x600",
            "1024x768",
            "1280x720",
            "1920x1080"
        };

        SelectedResolutionIndex = 4; // Default to 1920x1080

        FPS = new List<string>
        {
            "noCap",
            "30",
            "60",
            "120",
            "144",
            "240"
        };

        SelectedFPSIndex = 2; // Default to 60 FPS

        Fullscreen = FullScreenMode.FullScreenWindow;
        MasterVolume = 1.0f;
        SFXVolume = 1.0f;
    }

}

