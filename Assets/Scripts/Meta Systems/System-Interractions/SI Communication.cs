using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class SICommunication : MonoBehaviour
{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
    [DllImport("user32.dll", CharSet = CharSet.Unicode)]
    private static extern int MessageBox(IntPtr hWnd, string text, string caption, int type);
#endif

    [Flags]
    public enum AlertType
    {
        // Buttons
        OK = 0x00000000,
        OKCancel = 0x00000001,
        YesNo = 0x00000004,

        // Icons
        IconError = 0x00000010,
        IconQuestion = 0x00000020,
        IconWarning = 0x00000030,
        IconInfo = 0x00000040
    }

    public int ShowAlert(string title, string message, AlertType type = AlertType.OK)
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        int result = MessageBox(IntPtr.Zero, message, title, (int)type);

        Debug.Log($"Alert shown: {title} - {message}");
        Debug.Log($"User response: {result}");

        return result;
#else
        Debug.Log($"ALERT (Fallback): {title} - {message}");
        return -1;
#endif
    }
    
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SetWindowText(IntPtr hWnd, string lpString);

    [DllImport("user32.dll")]
    private static extern IntPtr GetActiveWindow();
#endif

    public void SetWindowName(string newTitle)
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        IntPtr handle = GetActiveWindow();

        if (handle == IntPtr.Zero)
        {
            Debug.LogError("Failed to get window handle.");
            return;
        }

        bool result = SetWindowText(handle, newTitle);

        if (result)
        {
            Debug.Log($"Window title changed to: {newTitle}");
        }
        else
        {
            Debug.LogError("Failed to change window title.");
        }
#else
    Debug.Log($"[Fallback] Change window name to: {newTitle}");
#endif
    }
}