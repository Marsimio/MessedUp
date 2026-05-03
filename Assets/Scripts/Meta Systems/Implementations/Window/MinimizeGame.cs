using UnityEngine;

public class MinimizeGame : MonoBehaviour
{
    [Header("Trigger Settings")]
    [SerializeField] private bool minimizeOnStart = false;
    [SerializeField] private KeyCode triggerKey = KeyCode.None;

    private void Start()
    {
        if (minimizeOnStart)
        {
            Minimize();
        }
    }

    private void Update()
    {
        if (triggerKey != KeyCode.None && Input.GetKeyDown(triggerKey))
        {
            Minimize();
        }
    }

    public void Minimize()
    {
        if (SIMain.Window == null)
        {
            Debug.LogError("SIWindow not found on SIMain.");
            return;
        }

        SIMain.Window.GameMinimize();
    }
}