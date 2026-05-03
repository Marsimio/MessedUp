using UnityEngine;
using UnityEngine.UI;

public class PortraitReplacer : MonoBehaviour
{
    [SerializeField] private string imagePath;

    private RawImage _rawImage;

    void Start()
    {
        _rawImage = GetComponent<RawImage>();

        Texture2D tex = SIMain.File.LoadImage(imagePath);

        if (tex != null)
        {
            _rawImage.texture = tex;
        }
        else
        {
            Debug.LogWarning("Failed to load texture.");
        }
    }
}
