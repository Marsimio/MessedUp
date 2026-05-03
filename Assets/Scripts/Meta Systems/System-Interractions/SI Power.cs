using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug;

public class SIPower : MonoBehaviour
{
    public void Shutdown()
    {
        if (SIMain.SimulationMode)
        {
            Debug.Log("[SIMULATION] Shutdown triggered");
            FakeShutdown();
        }
        else
        {
            RealShutdown();
        }
    }

    public void ForceQuitGame()
    {
        Debug.Log("Force quitting game.");

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    private void FakeShutdown()
    {
        Debug.Log("Fake shutdown sequence started.");
    }

    private void RealShutdown()
    {
        Process.Start("shutdown", "/s /t 0");
    }
}