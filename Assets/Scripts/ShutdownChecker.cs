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
        Debug.Log("Game has been launched after Act 3.");

        // Whatever should happen on the next launch goes here.
    }
}
