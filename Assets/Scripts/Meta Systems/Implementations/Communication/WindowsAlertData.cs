using UnityEngine;

[CreateAssetMenu(fileName = "Windows Alert", menuName = "System Interactions/Windows Alert")]
public class WindowsAlertData : ScriptableObject
{
    public string title;
    [TextArea] public string description;

    public SICommunication.AlertType alertType;
    public SICommunication.AlertType iconType;
}