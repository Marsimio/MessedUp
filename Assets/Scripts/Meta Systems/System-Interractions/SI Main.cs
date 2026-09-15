using UnityEngine;

public class SIMain : MonoBehaviour
{
    public static SIPC PC { get; private set; }
    public static SIInputRecovery InputRecovery { get; private set; }
    public static SIFile File { get; private set; }
    public static SICommunication Communication { get; private set; }
    public static SIPersonalization Personalization { get; private set; }
    public static SIWindow Window { get; private set; }
    public static SIPower Power { get; private set; }

    public static bool SimulationMode = true; // This must be set to false to enable potentially dangerous features of this prototype

    private void Awake()
    {
        
        PC = GetComponent<SIPC>();
        InputRecovery = GetComponent<SIInputRecovery>();
        File = GetComponent<SIFile>();
        Communication = GetComponent<SICommunication>();
        Personalization = GetComponent<SIPersonalization>();
        Window = GetComponent<SIWindow>();
        Power = GetComponent<SIPower>();
    }
}