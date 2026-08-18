using System;
using System.IO;
using UnityEngine;

public class SIFile : MonoBehaviour
{
    public string GetDocumentsPath(string relativePath)
    {
        string documentsPath =
            Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        return Path.Combine(documentsPath, relativePath);
    }
    
    public void Open(string path)
    {
        Debug.Log($"Opening file: {path}");

        if (File.Exists(path))
        {
            Application.OpenURL(path);
        }
        else
        {
            Debug.LogWarning($"File not found: {path}");
        }
    }

    public void Delete(string path)
    {
        if (SIMain.SimulationMode)
        {
            Debug.Log($"[SIMULATION] Deleting file: {path}");
            return;
        }

        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log($"Deleted file: {path}");
        }
        else
        {
            Debug.LogWarning($"File not found: {path}");
        }
    }

    public void Edit(string path, string content)
    {
        if (SIMain.SimulationMode)
        {
            Debug.Log($"[SIMULATION] Editing file: {path}");
            return;
        }

        File.WriteAllText(path, content);
        Debug.Log($"Edited file: {path}");
    }

    public Texture2D LoadImage(string path)
    {
        if (!File.Exists(path))
        {
            Debug.LogWarning($"Image not found: {path}");
            return null;
        }

        byte[] fileData = File.ReadAllBytes(path);

        Texture2D texture = new Texture2D(2, 2);
        bool success = texture.LoadImage(fileData);

        if (success)
        {
            Debug.Log($"Loaded image: {path}");
            return texture;
        }
        else
        {
            Debug.LogError("Failed to load image.");
            return null;
        }
    }
}
