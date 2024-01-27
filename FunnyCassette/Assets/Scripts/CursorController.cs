using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField] private Texture2D cursor;
    [SerializeField] private Texture2D cursorHover;

    public static CursorController singleton;

    private void Awake()
    {
        singleton = this;

        Pointer();
    }

    private void ChangeCursor(Texture2D cursor) {
        Cursor.SetCursor(cursor, new Vector2(0, 0), CursorMode.Auto);
    }

    public void Pointer() {
        ChangeCursor(cursor);
    }

    public void Hover() {
        ChangeCursor(cursorHover);
    }
}
