using UnityEngine;
using UnityEngine.Rendering;

public class CursorManager : MonoBehaviour
{
    [SerializeField] Texture2D defaultCursor;
    [SerializeField] bool isCentralized = true;
    [ShowIf("isCentralized", false)][SerializeField] Vector2 cursorHotspot = new Vector2(16, 16);

    private void Awake()
    {
        if (defaultCursor != null)
        {
            if (isCentralized)
            {
                Cursor.SetCursor(defaultCursor, new Vector2(defaultCursor.width / 2, defaultCursor.height / 2), CursorMode.Auto);
            }
            else
            { 
                Cursor.SetCursor(defaultCursor, cursorHotspot, CursorMode.Auto);
            }
        }
    }
}
