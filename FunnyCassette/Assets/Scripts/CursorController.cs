using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CursorController : MonoBehaviour
{
    [SerializeField] private Transform cursorTransform;
    [SerializeField] private Image cursorImage;
    [SerializeField] private Image pointerImage;

    public static CursorController singleton;

    private void Awake()
    {
        singleton = this;
        Pointer();
    }
    
    private void Update()
    {
        cursorTransform.position = Input.mousePosition;
        Cursor.visible = false;
    }

    public void Pointer() {
        cursorImage.gameObject.SetActive(false);
        pointerImage.gameObject.SetActive(true);
    }

    public void Hover() {
        cursorImage.gameObject.SetActive(true);
        pointerImage.gameObject.SetActive(false);
    }
}
