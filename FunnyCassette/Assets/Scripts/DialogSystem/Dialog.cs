using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

[Serializable]
public class Dialog
{
    public List<DialogPhrase> dialogs;
    
    // start at dialog_00!
    private const int dialogsCount = 11;

    private static int CurrentDialogIndex = -1;
    
    public static Dialog CreateFromJSON()
    {
        if(CurrentDialogIndex == -1)
            CurrentDialogIndex = UnityEngine.Random.Range(0, dialogsCount);
        else
            CurrentDialogIndex = (CurrentDialogIndex + 1) % dialogsCount;
        
        var fileName = "dialog_" + CurrentDialogIndex.ToString("00") + ".json";
        
        string jsonString = File.ReadAllText(Application.streamingAssetsPath + "/" + fileName);
        var dialog = JsonUtility.FromJson<Dialog>(jsonString);
        dialog.dialogs = dialog.dialogs.OrderBy(x => Guid.NewGuid()).ToList();
        return dialog;
    }
}