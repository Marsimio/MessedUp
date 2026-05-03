using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class SIWindow : MonoBehaviour
{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();

    private const int SW_MINIMIZE = 6;
#endif

    public void OpenBrowser(string url)
    {
        Debug.Log($"Opening browser: {url}");
        Application.OpenURL(url);
    }

    public bool DetectAltTab()
    {
        return !Application.isFocused;
    }

    public void GameMinimize()
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        IntPtr handle = GetActiveWindow();

        if (handle == IntPtr.Zero)
        {
            Debug.LogError("Failed to get window handle.");
            return;
        }

        ShowWindow(handle, SW_MINIMIZE);
        Debug.Log("Game minimized.");
#else
        Debug.Log("[Fallback] Minimize not supported on this platform.");
#endif
    }
}