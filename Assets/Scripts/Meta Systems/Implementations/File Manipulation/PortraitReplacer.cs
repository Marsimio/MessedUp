using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class PortraitReplacer : MonoBehaviour
{
    [SerializeField] private string imagePath;

    private RawImage _rawImage;

    void Start()
    {
        _rawImage = GetComponent<RawImage>();

        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        string fullImagePath = Path.Combine(
            documentsPath,
            imagePath
        );

        Texture2D tex = SIMain.File.LoadImage(fullImagePath);

        if (tex != null)
        {
            _rawImage.texture = tex;
        }
        else
        {
            Debug.LogWarning($"Failed to load texture: {fullImagePath}");
        }
    }
}