using System.Collections;
using UnityEngine;

public class Act1Manager : MonoBehaviour
{
    public static Act1Manager Instance;
    [SerializeField] GameObject portrait;
    [SerializeField] WindowsAlert Alert;
    [SerializeField] AlarmClock Alarm;
    
    [SerializeField] private Transform playerRoot;
    [SerializeField] private Transform playerCamera;

    [SerializeField] private Vector3 standingPosition;
    [SerializeField] private Vector3 standingCameraLocalPosition = new Vector3(0f, 1.6f, 0f);
    [SerializeField] private float outOfBedDuration = 2f;
    
    private Color _originalAmbient;
    public int currentStep;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        StartAct();
    }

    public void StartAct()
    {
        currentStep = 0;
        Debug.Log("Act 1 started");

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
                Step_Intro();
                break;

            case 1:
                OutOfBed();
                break;
            case 2:
                ItemDeletion();
                break;

            default:
                Debug.Log("Act 1 complete");
                break;
        }
    }

    private void Step_Intro()
    {
        Alert.ShowMessage(0);
        PlayerController player = FindObjectOfType<PlayerController>();
        player.LockInputUntilReconnect();
        Alarm.ActivateAlarm();
    }

    private void OutOfBed()
    {
        SIMain.Communication.SetWindowName("Unplug your controller. You're still a bit hazy");

        StartCoroutine(OutOfBedRoutine());
    }
    
    private void ItemDeletion()
    {
        _originalAmbient = RenderSettings.ambientLight;

        RenderSettings.ambientLight = Color.black;

        if (portrait != null)
            Destroy(portrait);

        StartCoroutine(RestoreLight());
    }

    private IEnumerator RestoreLight()
    {
        yield return new WaitForSeconds(3f);

        RenderSettings.ambientLight = _originalAmbient;
    }
    
    private IEnumerator OutOfBedRoutine()
    {
        Vector3 startPlayerPos = playerRoot.position;
        Quaternion startPlayerRot = playerRoot.rotation;

        Vector3 startCameraPos = playerCamera.localPosition;
        Quaternion startCameraRot = playerCamera.localRotation;

        Quaternion endPlayerRot = Quaternion.Euler(0f, playerRoot.eulerAngles.y, 0f);
        Quaternion endCameraRot = Quaternion.identity;

        float t = 0f;

        while (t < outOfBedDuration)
        {
            t += Time.deltaTime;
            float lerp = t / outOfBedDuration;

            playerRoot.position = Vector3.Lerp(startPlayerPos, standingPosition, lerp);
            playerRoot.rotation = Quaternion.Slerp(startPlayerRot, endPlayerRot, lerp);

            playerCamera.localPosition = Vector3.Lerp(startCameraPos, standingCameraLocalPosition, lerp);
            playerCamera.localRotation = Quaternion.Slerp(startCameraRot, endCameraRot, lerp);

            yield return null;
        }

        playerRoot.position = standingPosition;
        playerRoot.rotation = endPlayerRot;

        playerCamera.localPosition = standingCameraLocalPosition;
        playerCamera.localRotation = endCameraRot;
    }
}