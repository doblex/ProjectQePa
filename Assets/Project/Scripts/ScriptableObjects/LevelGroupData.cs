using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/LevelGroup")]
public class LevelGroupData : ScriptableObject
{
    public LevelData[] Levels;
}

