using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextPresenter : MonoBehaviour
{
    [SerializeField] TMPro.TMP_Text shownText;


    public void PresentText(string text) {
        shownText.text = text;
    }
}
