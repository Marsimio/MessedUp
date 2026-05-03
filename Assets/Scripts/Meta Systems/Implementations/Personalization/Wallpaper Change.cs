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

        SIMain.Personalization.ChangeBackground(imagePath);
    }
}