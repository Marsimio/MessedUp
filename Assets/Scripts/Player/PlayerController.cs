using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    private PlayerControls _playerControls;
    private PlayerControls.OnFootActions _onFootActions;

    private PlayerMovement _playerMovement;
    private PlayerLook _playerLook;
    private PlayerInteract _playerInteract;

    private bool _inputLocked;

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
        if (_inputLocked) return;

        Vector2 moveInput = _onFootActions.Movement.ReadValue<Vector2>();
        Vector2 lookInput = _onFootActions.Look.ReadValue<Vector2>();

        _playerLook.ProcessLook(lookInput);
        _playerMovement.ProcessMove(moveInput);
    }

    public void LockInputUntilReconnect()
    {
        if (_inputLocked) return;

        _inputLocked = true;
        _playerControls.Disable();

        if (SIMain.PC != null)
            SIMain.PC.BeginInputResetCheck();

        Debug.Log("Input locked.");
    }

    private void HandleInputResetDetected(string deviceName)
    {
        if (!_inputLocked) return;

        Debug.Log($"Unlock triggered by: {deviceName}");
        UnlockInput();
    }

    private void UnlockInput()
    {
        _inputLocked = false;

        if (SIMain.PC != null)
            SIMain.PC.StopInputResetCheck();

        _playerControls.Enable();

        Debug.Log("Input unlocked.");
    }
    
    private void HandleInteract(InputAction.CallbackContext context)
    {
        if (_inputLocked) return;

        _playerInteract.TryInteract();
    }
}