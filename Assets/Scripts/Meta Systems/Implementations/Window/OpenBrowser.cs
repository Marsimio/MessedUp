using UnityEngine;

public class OpenBrowser : MonoBehaviour
{
    [SerializeField] private string url;

    public void Open()
    {
        if (SIMain.Window == null)
        {
            Debug.LogError("SIWindow not found on SIMain.");
            return;
        }

        SIMain.Window.OpenBrowser(url);
    }
}