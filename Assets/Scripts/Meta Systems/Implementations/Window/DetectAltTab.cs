using UnityEngine;

public class DetectAltTab : MonoBehaviour
{
    private bool _wasFocused = true;

    private void Update()
    {
        if (SIMain.Window == null) return;

        bool isAltTab = SIMain.Window.DetectAltTab();

        if (isAltTab && _wasFocused)
        {
            Debug.Log("Player Alt-Tabbed!");
            _wasFocused = false;
        }
        else if (!isAltTab)
        {
            _wasFocused = true;
        }
    }
}