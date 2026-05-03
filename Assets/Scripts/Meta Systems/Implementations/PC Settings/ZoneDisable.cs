using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ZoneDisable : MonoBehaviour
{
    private bool _triggered = false;

    private void Awake()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_triggered) return;

        if (other.CompareTag("Player"))
        {
            _triggered = true;

            PlayerController player = other.GetComponentInParent<PlayerController>();

            if (player != null)
            {
                player.LockInputUntilReconnect();
            }
        }
    }
}