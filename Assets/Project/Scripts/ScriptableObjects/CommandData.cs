using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Command")]
public class CommandData : ScriptableObject
{
    public string Name;
    public KeyCode Value;
    public Texture2D Icon;
}

