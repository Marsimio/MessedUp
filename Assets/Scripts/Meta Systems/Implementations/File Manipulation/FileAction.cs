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
    [SerializeField] private string content; // used for Edit/Create

    public void Execute()
    {
        if (SIMain.File == null)
        {
            Debug.LogError("SIFile not found on SIMain.");
            return;
        }

        switch (actionType)
        {
            case ActionType.Open:
                SIMain.File.Open(filePath);
                break;

            case ActionType.Delete:
                SIMain.File.Delete(filePath);
                break;

            case ActionType.Edit:
                SIMain.File.Edit(filePath, content);
                break;

            case ActionType.Create:
                SIMain.File.Edit(filePath, content); // same as create
                break;
        }
    }
}