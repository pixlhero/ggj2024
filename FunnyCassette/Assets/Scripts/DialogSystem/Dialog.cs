using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[Serializable]
public class Dialog
{
    public List<DialogPhrase> dialogs;
    
    public static Dialog CreateFromJSON()
    {
        string jsonString = File.ReadAllText(Application.streamingAssetsPath + "/dialog.json");
        var dialog = JsonUtility.FromJson<Dialog>(jsonString);
        dialog.dialogs = dialog.dialogs.OrderBy(x => Guid.NewGuid()).ToList();
        return dialog;
    }
}