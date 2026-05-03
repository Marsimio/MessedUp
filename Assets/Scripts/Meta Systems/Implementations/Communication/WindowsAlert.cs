using UnityEngine;

public class WindowsAlert : MonoBehaviour
{
    [SerializeField] private WindowsAlertData[] alerts;

    public void ShowMessage(int alertNo)
    {
        if (alerts == null || alerts.Length == 0) return;
        if (alertNo < 0 || alertNo >= alerts.Length) return;

        WindowsAlertData alert = alerts[alertNo];
        if (alert == null) return;

        string playerName = SIMain.PC.GetUserName();

        string finalTitle = alert.title.Replace("{name}", playerName);
        string finalDescription = alert.description.Replace("{name}", playerName);

        SIMain.Communication.ShowAlert(
            finalTitle,
            finalDescription,
            alert.alertType | alert.iconType
        );
    }
}