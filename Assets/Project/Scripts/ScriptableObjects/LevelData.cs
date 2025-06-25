using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Level")]
public class LevelData : ScriptableObject
{
    public string LevelName;
    public string SceneName;
    public Texture2D Planet;
    public Texture2D LevelScreen;
    public string Description;
    public bool IsEnded = false;
    public bool comingSoon = false;
}

