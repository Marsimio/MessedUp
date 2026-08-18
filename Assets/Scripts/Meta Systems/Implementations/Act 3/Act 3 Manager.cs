using UnityEngine;

public class Act3Manager : MonoBehaviour
{
    public static Act3Manager Instance;

    public int currentStep;

    [SerializeField] private WindowsAlert alert;

    private const string Act3CompletedKey = "Act3Completed";

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartAct();
    }

    public void StartAct()
    {
        currentStep = 0;

        Debug.Log("Act 3 started.");

        RunStep();
    }

    public void NextStep()
    {
        currentStep++;
        RunStep();
    }

    private void RunStep()
    {
        Debug.Log($"Running Act 3 step: {currentStep}");

        switch (currentStep)
        {
            case 0:
                alert.ShowMessage(0);
                break;

            case 1:
                alert.ShowMessage(1);
                break;

            case 2:
                alert.ShowMessage(2);
                break;

            case 3:
                CompleteAct();
                break;

            default:
                Debug.Log("Act 3 complete.");
                break;
        }
    }

    private void CompleteAct()
    {
        Debug.Log("Act 3 completed. Closing application.");

        PlayerPrefs.SetInt(Act3CompletedKey, 1);
        PlayerPrefs.Save();

        SIMain.Window.CloseApplication();
    }
}