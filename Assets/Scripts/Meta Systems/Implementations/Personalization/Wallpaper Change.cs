using System;
using System.IO;
using UnityEngine;

public class WallpaperChange : MonoBehaviour
{
    [SerializeField] private string imagePath;

    public void Change()
    {
        if (SIMain.Personalization == null)
        {
            Debug.LogError("SIPersonalization not found on SIMain.");
            return;
        }

        string documentsPath =
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        string fullPath = Path.Combine(
            documentsPath,
            imagePath
        );

        SIMain.Personalization.ChangeBackground(fullPath);
    }
}