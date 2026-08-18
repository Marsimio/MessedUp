using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlarmClock : MonoBehaviour
{
    [SerializeField] private AudioSource alarmSource;
    
    public void ActivateAlarm()
    {
        StartCoroutine(AlarmTimer());
    }

    private IEnumerator AlarmTimer()
    {
        Debug.Log("Alarm started");

        while (true)
        {
            float volume = WindowsAudio.GetMasterVolume();

            if (volume <= 0.001f)
            {
                Debug.Log("Alarm stopped (volume is 0)");
                if (Act1Manager.Instance != null)
                {
                    alarmSource.Stop();
                    Act1Manager.Instance.NextStep();
                }
                yield break;
            }

            Debug.Log($"Alarm running. Volume: {volume}");

            yield return new WaitForSeconds(0.5f);
        }
    }
}