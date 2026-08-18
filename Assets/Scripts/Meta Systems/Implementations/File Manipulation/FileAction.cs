using System;
using System.IO;
using UnityEngine;

public class FileAction : MonoBehaviour
{
    public enum ActionType
    {
        Open,
        Delete,
        Edit,
        Create
    }

    [SerializeField] private string filePath;
    [SerializeField] private ActionType actionType;

    [TextArea]
    [SerializeField] private string content;

    public void Execute()
    {
        if (SIMain.File == null)
        {
            Debug.LogError("SIFile not found on SIMain.");
            return;
        }

        string fullPath = SIMain.File.GetDocumentsPath(filePath);
        
        Debug.Log($"FileAction path: {fullPath}");

        switch (actionType)
        {
            case ActionType.Open:
                SIMain.File.Open(fullPath);
                break;

            case ActionType.Delete:
                SIMain.File.Delete(fullPath);
                break;

            case ActionType.Edit:
                SIMain.File.Edit(fullPath, content);
                break;

            case ActionType.Create:
                SIMain.File.Edit(fullPath, content);
                break;
        }
    }
}