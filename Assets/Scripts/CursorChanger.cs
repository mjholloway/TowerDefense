using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorChanger : MonoBehaviour
{
    [SerializeField] Texture2D cursorTexture;

    CursorMode cursorMode = CursorMode.Auto;
    Vector2 hotSpot;

    public void SetCursor()
    {
        hotSpot = new Vector2(cursorTexture.height / 2, cursorTexture.width / 2);
        Cursor.SetCursor(cursorTexture, hotSpot, cursorMode);
    }

    public void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }
}
