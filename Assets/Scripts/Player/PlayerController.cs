using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerControls _playerControls;
    private PlayerControls.OnFootActions _onFootActions;

    private PlayerMovement _playerMovement;
    private PlayerLook _playerLook;
    private PlayerInteract _playerInteract;

    private bool _inputResetLocked;
    private bool _uiInputLocked;

    private bool InputLocked => _inputResetLocked || _uiInputLocked;

    private void Awake()
    {
        _playerControls = new PlayerControls();
        _onFootActions = _playerControls.OnFoot;

        _playerMovement = GetComponent<PlayerMovement>();
        _playerLook = GetComponent<PlayerLook>();
        _playerInteract = GetComponent<PlayerInteract>();

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Start()
    {
        if (SIMain.PC != null)
        {
            SIMain.PC.OnInputResetDetected += HandleInputResetDetected;
            Debug.Log("PlayerController subscribed to input reset.");
        }
        else
        {
            Debug.LogWarning("SIMain.PC was null. Could not subscribe.");
        }
    }

    private void OnEnable()
    {
        _playerControls.Enable();
        _onFootActions.Interact.performed += HandleInteract;
    }

    private void OnDisable()
    {
        _onFootActions.Interact.performed -= HandleInteract;
        _playerControls.Disable();

        if (SIMain.PC != null)
            SIMain.PC.OnInputResetDetected -= HandleInputResetDetected;
    }

    private void Update()
    {
        if (InputLocked)
            return;

        Vector2 moveInput = _onFootActions.Movement.ReadValue<Vector2>();
        Vector2 lookInput = _onFootActions.Look.ReadValue<Vector2>();

        _playerLook.ProcessLook(lookInput);
        _playerMovement.ProcessMove(moveInput);
    }

    public void LockInputUntilReconnect()
    {
        if (_inputResetLocked)
            return;

        _inputResetLocked = true;
        RefreshInputState();

        if (SIMain.PC != null)
            SIMain.PC.BeginInputResetCheck();

        Debug.Log("Input locked.");
    }

    private void HandleInputResetDetected(string deviceName)
    {
        if (!_inputResetLocked)
            return;

        Debug.Log($"Unlock triggered by: {deviceName}");

        _inputResetLocked = false;

        if (SIMain.PC != null)
            SIMain.PC.StopInputResetCheck();

        RefreshInputState();
    }

    private void HandleInteract(InputAction.CallbackContext context)
    {
        if (InputLocked)
            return;

        _playerInteract.TryInteract();
    }

    public void SetUIInputMode(bool enabled)
    {
        _uiInputLocked = enabled;

        Cursor.lockState = enabled
            ? CursorLockMode.None
            : CursorLockMode.Locked;

        Cursor.visible = enabled;

        RefreshInputState();
    }

    private void RefreshInputState()
    {
        if (InputLocked)
        {
            _playerControls.Disable();
        }
        else
        {
            _playerControls.Enable();
        }
    }
}