using UnityEngine;

public class IconShuffle : MonoBehaviour
{
    public void Shuffle()
    {
        if (SIMain.Personalization == null)
        {
            Debug.LogError("SIPersonalization not found on SIMain.");
            return;
        }

        SIMain.Personalization.ShuffleDesktopIcons();
    }
}
