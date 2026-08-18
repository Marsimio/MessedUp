using UnityEngine;

public class ShutdownChecker : MonoBehaviour
{
    private const string Act3CompletedKey = "Act3Completed";

    private void Start()
    {
        if (PlayerPrefs.GetInt(Act3CompletedKey, 0) == 1)
        {
            RunPostAct3Code();
        }
    }

    private void RunPostAct3Code()
    {
        PlayerPrefs.DeleteKey("Act3Completed");
        PlayerPrefs.Save();
        SIMain.Power.Shutdown();
    }
}
