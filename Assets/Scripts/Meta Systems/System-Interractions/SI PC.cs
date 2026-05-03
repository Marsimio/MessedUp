using System;
using System.Runtime.InteropServices;
using UnityEngine;

#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class SIPC : MonoBehaviour
{
    public event Action<string> OnInputResetDetected;

#if ENABLE_INPUT_SYSTEM
    private bool _checkingInputReset;
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

    public void BeginInputResetCheck()
    {
#if ENABLE_INPUT_SYSTEM
        _checkingInputReset = true;
        _controllerWasRemoved = false;

        Debug.Log("Started controller reset check.");
#endif
    }

    public void StopInputResetCheck()
    {
#if ENABLE_INPUT_SYSTEM
        _checkingInputReset = false;
        _controllerWasRemoved = false;
#endif
    }

#if ENABLE_INPUT_SYSTEM
    private void HandleDeviceChange(InputDevice device, InputDeviceChange change)
    {
        if (!_checkingInputReset) return;
        if (device is not Gamepad) return;

        if (change == InputDeviceChange.Disconnected ||
            change == InputDeviceChange.Removed)
        {
            _controllerWasRemoved = true;
            Debug.Log($"Controller removed: {device.displayName}");
            return;
        }

        if (_controllerWasRemoved &&
            (change == InputDeviceChange.Reconnected ||
             change == InputDeviceChange.Added))
        {
            TriggerInputReset($"{device.displayName} ({device.layout})");
        }
    }

    private void TriggerInputReset(string reason)
    {
        if (!_checkingInputReset) return;

        _checkingInputReset = false;
        _controllerWasRemoved = false;

        Debug.Log($"Input reset triggered by: {reason}");
        OnInputResetDetected?.Invoke(reason);
    }
#endif

    // -------- SYSTEM INFO --------

    public string GetComputerName()
    {
        return SystemInfo.deviceName;
    }

    public string GetUserName()
    {
        return Environment.UserName;
    }

    public DateTime GetSystemTime()
    {
        return DateTime.Now;
    }

    public float GetSoundVolume()
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN
        return WindowsAudio.GetMasterVolume();
#else
        Debug.LogWarning("System volume is only supported on Windows.");
        return -1f;
#endif
    }
}

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN

public static class WindowsAudio
{
    public static float GetMasterVolume()
    {
        IMMDeviceEnumerator enumerator = null;
        IMMDevice device = null;
        IAudioEndpointVolume volume = null;

        try
        {
            enumerator = new MMDeviceEnumerator() as IMMDeviceEnumerator;

            enumerator.GetDefaultAudioEndpoint(
                EDataFlow.eRender,
                ERole.eMultimedia,
                out device
            );

            Guid endpointVolumeGuid = typeof(IAudioEndpointVolume).GUID;

            device.Activate(
                ref endpointVolumeGuid,
                CLSCTX.ALL,
                IntPtr.Zero,
                out object endpointVolumeObject
            );

            volume = endpointVolumeObject as IAudioEndpointVolume;
            volume.GetMasterVolumeLevelScalar(out float level);

            Debug.Log($"System Volume: {level * 100f:0}%");
            return level;
        }
        catch (Exception ex)
        {
            Debug.LogWarning($"Failed to get system volume: {ex.Message}");
            return -1f;
        }
        finally
        {
            if (volume != null)
                Marshal.ReleaseComObject(volume);

            if (device != null)
                Marshal.ReleaseComObject(device);

            if (enumerator != null)
                Marshal.ReleaseComObject(enumerator);
        }
    }
}

[Flags]
public enum CLSCTX : uint
{
    INPROC_SERVER = 0x1,
    INPROC_HANDLER = 0x2,
    LOCAL_SERVER = 0x4,
    REMOTE_SERVER = 0x10,
    ALL = INPROC_SERVER | INPROC_HANDLER | LOCAL_SERVER | REMOTE_SERVER
}

[ComImport]
[Guid("BCDE0395-E52F-467C-8E3D-C4579291692E")]
public class MMDeviceEnumerator { }

[ComImport]
[Guid("A95664D2-9614-4F35-A746-DE8DB63617E6")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IMMDeviceEnumerator
{
    int NotImpl1();

    [PreserveSig]
    int GetDefaultAudioEndpoint(
        EDataFlow dataFlow,
        ERole role,
        out IMMDevice ppDevice
    );
}

[ComImport]
[Guid("D666063F-1587-4E43-81F1-B948E807363F")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IMMDevice
{
    [PreserveSig]
    int Activate(
        ref Guid iid,
        CLSCTX dwClsCtx,
        IntPtr pActivationParams,
        [MarshalAs(UnmanagedType.IUnknown)] out object ppInterface
    );
}

[ComImport]
[Guid("5CDF2C82-841E-4546-9722-0CF74078229A")]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IAudioEndpointVolume
{
    int RegisterControlChangeNotify(IntPtr pNotify);
    int UnregisterControlChangeNotify(IntPtr pNotify);
    int GetChannelCount(out uint channelCount);
    int SetMasterVolumeLevel(float levelDB, Guid eventContext);
    int SetMasterVolumeLevelScalar(float level, Guid eventContext);
    int GetMasterVolumeLevel(out float levelDB);

    [PreserveSig]
    int GetMasterVolumeLevelScalar(out float level);
}

public enum EDataFlow
{
    eRender,
    eCapture,
    eAll
}

public enum ERole
{
    eConsole,
    eMultimedia,
    eCommunications
}

#endif