using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class Dialog
{
    public List<DialogPhrase> dialogs;
    
    public static Dialog CreateFromJSON()
    {
        string jsonString = File.ReadAllText(Application.streamingAssetsPath + "/dialog.json");
        return JsonUtility.FromJson<Dialog>(jsonString);
    }
}