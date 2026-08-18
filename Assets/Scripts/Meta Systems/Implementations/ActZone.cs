using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ActZone : MonoBehaviour
{
    [SerializeField] private int actNumber = 1;
    [SerializeField] private bool triggerOnce = true;

    private bool _triggered;

    private void Awake()
    {
        GetComponent<BoxCollider>().isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_triggered && triggerOnce) return;
        if (!other.CompareTag("Player")) return;

        Debug.Log("Act Zone Triggered!");
        
        if (Act1Manager.Instance != null && actNumber == 1)
        {
            Act1Manager.Instance.NextStep();
        }
        if (Act2Manager.Instance != null && actNumber == 2)
        {
            Act2Manager.Instance.NextStep();
        }
        if (Act3Manager.Instance != null && actNumber == 3)
        {
            Act3Manager.Instance.NextStep();
        }

        _triggered = true;
    }
}