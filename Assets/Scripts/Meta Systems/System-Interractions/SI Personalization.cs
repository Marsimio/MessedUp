using System;
using System.IO;
using System.Runtime.InteropServices;
using UnityEngine;

public class SIPersonalization : MonoBehaviour
{
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
    [DllImport("user32.dll", SetLastError = true)]
    private static extern bool SystemParametersInfo(
        int uAction,
        int uParam,
        string lpvParam,
        int fuWinIni
    );

    [DllImport("user32.dll")]
    private static extern IntPtr FindWindow(string className, string windowName);

    [DllImport("user32.dll")]
    private static extern IntPtr FindWindowEx(
        IntPtr parentHandle,
        IntPtr childAfter,
        string className,
        string windowTitle
    );

    [DllImport("user32.dll")]
    private static extern IntPtr SendMessage(
        IntPtr hWnd,
        int msg,
        IntPtr wParam,
        IntPtr lParam
    );

    private const int SPI_SETDESKWALLPAPER = 20;
    private const int SPIF_UPDATEINIFILE = 0x01;
    private const int SPIF_SENDCHANGE = 0x02;

    private const int LVM_FIRST = 0x1000;
    private const int LVM_GETITEMCOUNT = LVM_FIRST + 4;
    private const int LVM_SETITEMPOSITION = LVM_FIRST + 15;
#endif

    public void ChangeBackground(string imagePath)
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        if (!File.Exists(imagePath))
        {
            Debug.LogError($"Image not found: {imagePath}");
            return;
        }

        bool result = SystemParametersInfo(
            SPI_SETDESKWALLPAPER,
            0,
            imagePath,
            SPIF_UPDATEINIFILE | SPIF_SENDCHANGE
        );

        if (result)
            Debug.Log("Wallpaper changed successfully.");
        else
            Debug.LogError("Failed to change wallpaper.");
#else
        Debug.Log($"[Fallback] Change background: {imagePath}");
#endif
    }

    public void ShuffleDesktopIcons()
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        IntPtr desktopListView = GetDesktopListView();

        if (desktopListView == IntPtr.Zero)
        {
            Debug.LogError("Could not find desktop icon ListView.");
            return;
        }

        int iconCount = SendMessage(
            desktopListView,
            LVM_GETITEMCOUNT,
            IntPtr.Zero,
            IntPtr.Zero
        ).ToInt32();

        if (iconCount <= 0)
        {
            Debug.LogWarning("No desktop icons found.");
            return;
        }

        System.Random random = new System.Random();

        int screenWidth = Screen.currentResolution.width;
        int screenHeight = Screen.currentResolution.height;

        int padding = 80;

        for (int i = 0; i < iconCount; i++)
        {
            int x = random.Next(padding, screenWidth - padding);
            int y = random.Next(padding, screenHeight - padding);

            IntPtr position = MakeLParam(x, y);

            SendMessage(
                desktopListView,
                LVM_SETITEMPOSITION,
                (IntPtr)i,
                position
            );
        }

        Debug.Log($"Shuffled {iconCount} desktop icons.");
#else
        Debug.Log("[Fallback] Shuffle desktop icons.");
#endif
    }

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
    private IntPtr GetDesktopListView()
    {
        IntPtr progman = FindWindow("Progman", "Program Manager");

        IntPtr shellView = FindWindowEx(
            progman,
            IntPtr.Zero,
            "SHELLDLL_DefView",
            null
        );

        if (shellView == IntPtr.Zero)
        {
            IntPtr workerW = IntPtr.Zero;

            do
            {
                workerW = FindWindowEx(
                    IntPtr.Zero,
                    workerW,
                    "WorkerW",
                    null
                );

                shellView = FindWindowEx(
                    workerW,
                    IntPtr.Zero,
                    "SHELLDLL_DefView",
                    null
                );

            } while (shellView == IntPtr.Zero && workerW != IntPtr.Zero);
        }

        if (shellView == IntPtr.Zero)
            return IntPtr.Zero;

        return FindWindowEx(
            shellView,
            IntPtr.Zero,
            "SysListView32",
            "FolderView"
        );
    }

    private IntPtr MakeLParam(int x, int y)
    {
        return (IntPtr)((y << 16) | (x & 0xFFFF));
    }
#endif
}