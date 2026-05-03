using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class ZoneAlert : MonoBehaviour
{
    [SerializeField] private string title;
    [SerializeField] private string description;

    [SerializeField] private SICommunication.AlertType alertType;
    [SerializeField] private SICommunication.AlertType iconType;

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

            string playerName = SIMain.PC.GetUserName();

            string finalTitle = title.Replace("{name}", playerName);
            string finalDescription = description.Replace("{name}", playerName);

            SIMain.Communication.ShowAlert(
                finalTitle,
                finalDescription,
                alertType | iconType
            );
        }
    }
}