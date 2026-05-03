using System;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class SIInputRecovery : MonoBehaviour
{
    public event Action<string> OnRecoveryComplete;

#if ENABLE_INPUT_SYSTEM
    private bool _active;
    private bool _controllerWasRemoved;
#endif

    private void OnEnable()
    {
#if ENABLE_INPUT_SYSTEM
        InputSystem.onDeviceChange += HandleDeviceChange;
#endif
    }

    private void OnDisable()
    {
#if ENABLE_INPUT_SYSTEM
        InputSystem.onDeviceChange -= HandleDeviceChange;
#endif
    }

    private void Update()
    {
#if ENABLE_INPUT_SYSTEM
        if (!_active) return;

        if (_controllerWasRemoved &&
            Gamepad.current != null &&
            Gamepad.current.buttonSouth.wasPressedThisFrame)
        {
            CompleteRecovery("Controller input confirmed");
        }
#endif
    }

    public void BeginRecovery()
    {
#if ENABLE_INPUT_SYSTEM
        _active = true;
        _controllerWasRemoved = false;

        Debug.Log("Input recovery started.");
#endif
    }

    public void StopRecovery()
    {
#if ENABLE_INPUT_SYSTEM
        _active = false;
        _controllerWasRemoved = false;
#endif
    }

#if ENABLE_INPUT_SYSTEM
    private void HandleDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (!_active) return;
        if (device is not Gamepad) return;

        if (change == InputDeviceChange.Disconnected ||
            change == InputDeviceChange.Removed)
        {
            _controllerWasRemoved = true;
            return;
        }

        if (_controllerWasRemoved &&
            (change == InputDeviceChange.Reconnected ||
             change == InputDeviceChange.Added))
        {
            CompleteRecovery("Controller reconnected");
        }
    }

    private void CompleteRecovery(string reason)
    {
        if (!_active) return;

        _active = false;
        _controllerWasRemoved = false;

        Debug.Log($"Input recovery complete: {reason}");
        OnRecoveryComplete?.Invoke(reason);
    }
#endif
}