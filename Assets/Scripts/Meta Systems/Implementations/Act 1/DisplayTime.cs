using System;
using TMPro;
using UnityEngine;

public class DisplayTime : MonoBehaviour
{
    [SerializeField] private TMP_Text clockText;

    private DateTime now;

    private void Update()
    {
        now = SIMain.PC.GetSystemTime();

        clockText.text = now.ToString("HH:mm");
    }
}