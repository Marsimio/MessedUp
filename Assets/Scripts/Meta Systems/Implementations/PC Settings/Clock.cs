using System;
using UnityEngine;

public class Clock : MonoBehaviour
{
    [SerializeField] private Transform hourHand;
    [SerializeField] private Transform minuteHand;

    private DateTime now;

    void Update()
    {
        now = SIMain.PC.GetSystemTime();

        UpdateClockHands();
    }

    private void UpdateClockHands()
    {
        float hours = now.Hour % 12;
        float minutes = now.Minute;

        float hourRotation = (hours + minutes / 60f) * 30f;

        float minuteRotation = minutes * 6f;

        hourHand.localRotation = Quaternion.Euler(0, hourRotation, 0);
        minuteHand.localRotation = Quaternion.Euler(0, minuteRotation, 0);
    }
}