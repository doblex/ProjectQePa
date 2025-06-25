using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Command")]
public class CommandData : ScriptableObject
{
    public string CommandName;
    public KeyCode Value;
    public Texture2D Icon;
}