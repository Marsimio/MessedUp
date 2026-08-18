using System.Collections;
using UnityEngine;

public class Act2Manager : MonoBehaviour
{
    public static Act2Manager Instance;
    public int currentStep;

    [SerializeField] private FileAction portrait1;
    [SerializeField] private FileAction portrait2;

    [SerializeField] private FileAction MessageCreator;
    [SerializeField] private FileAction MessageEditor;
    [SerializeField] private FileAction MessageOpener;
    
    [SerializeField] private Transform player;
    [SerializeField] private Transform[] teleportTargets;
    [SerializeField] private WindowsAlert alert;
    [SerializeField] private GameObject[] removableDoors;
    [SerializeField] private WallpaperChange wallpaperChange;
    [SerializeField] private IconShuffle iconShuffle;

    private bool _waitingForAltTab;
    private bool _wasFocused = true;
    private bool _altTabTriggered;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartAct();
    }

    private void Update()
    {
        if (!_waitingForAltTab || _altTabTriggered) return;

        bool isAltTab = SIMain.Window.DetectAltTab();

        if (_wasFocused && isAltTab)
        {
            Debug.Log("Player tabbed out.");
            _wasFocused = false;
            return;
        }

        if (!_wasFocused && !isAltTab)
        {
            Debug.Log("Player returned to the game.");
            _altTabTriggered = true;
            StartCoroutine(HandleAltTabProgress());
        }
    }

    public void StartAct()
    {
        currentStep = 0;
        Debug.Log("Act 2 started");
        RunStep();
    }

    public void NextStep()
    {
        currentStep++;
        RunStep();
    }

    private void RunStep()
    {
        Debug.Log($"Running step: {currentStep}");

        switch (currentStep)
        {
            case 0:
                SIMain.Communication.SetWindowName("Keep moving forwards");
                alert.ShowMessage(0);
                break;

            case 1:
                portrait1.Execute();
                portrait2.Execute();
                SIMain.Communication.SetWindowName("One has to go");
                MessageCreator.Execute();
                MessageOpener.Execute();
                //SIMain.File.Open(/);
                //alert.ShowMessage(1);
                break;
            case 2:
                MessageEditor.Execute();
                MessageOpener.Execute();
                Destroy(removableDoors[0]);
                break;
            case 3:
                SIMain.Communication.SetWindowName("I can solve this for you if you just look away.");
                WaitForAltTab();
                break;
            case 4:
                StartCoroutine(FinalSequence());
                break;
            case 5:
                Destroy(removableDoors[1]);
                break;
            default:
                Debug.Log("Act 2 complete");
                break;
        }
    }

    private void WaitForAltTab()
    {
        _waitingForAltTab = true;
        _altTabTriggered = false;
        _wasFocused = Application.isFocused;

        Debug.Log("Waiting for Alt+Tab...");
    }

    private IEnumerator HandleAltTabProgress()
    {
        yield return new WaitForSecondsRealtime(0.5f);

        _waitingForAltTab = false;

        NextStep();
    }

    private IEnumerator FinalSequence()
    {
        Debug.Log("Final sequence started.");

        TeleportPlayer(0);

        yield return new WaitForSecondsRealtime(5f);

        alert.ShowMessage(2);

        yield return new WaitForSecondsRealtime(2f);
        
        TeleportPlayer(1);
        wallpaperChange.Change();
        iconShuffle.Shuffle();
        SIMain.Window.GameMinimize();

        Debug.Log("Sequence complete.");
    }

    private void TeleportPlayer(int targetIndex)
    {
        if (player == null || teleportTargets == null || teleportTargets.Length == 0)
        {
            Debug.LogError("Teleport failed: missing references.");
            return;
        }

        if (targetIndex < 0 || targetIndex >= teleportTargets.Length)
        {
            Debug.LogError($"Invalid teleport target index: {targetIndex}");
            return;
        }

        Transform target = teleportTargets[targetIndex];

        // player should reference PlayerRoot.
        CharacterController controller =
            player.GetComponentInChildren<CharacterController>();

        if (controller != null)
            controller.enabled = false;

        // Rotate only around the vertical axis.
        Vector3 targetEuler = target.eulerAngles;

        player.SetPositionAndRotation(
            target.position,
            Quaternion.Euler(0f, targetEuler.y, 0f)
        );

        if (controller != null)
            controller.enabled = true;

        Debug.Log($"Player teleported to {target.position}");
    }
}
