using System;
using System.Runtime.InteropServices;
using Vanara.PInvoke;
using static Vanara.PInvoke.CoreAudio;

namespace BluetoothDevicePairing.Bluetooth.Devices.AudioDevices;

internal sealed class AudioDevice
{
    private readonly IKsControl ksControl;

    public string Name { get; }
    private string Id { get; }
    public Guid ContainerId { get; }
    public bool IsConnected { get; }

    public AudioDevice(IMMDevice device, IKsControl ksControl)
    {
        this.ksControl = ksControl;

        Id = device.GetId();
        IsConnected = device.GetState() == DEVICE_STATE.DEVICE_STATE_ACTIVE;

        var propertyStore = device.OpenPropertyStore(STGM.STGM_READ);
        Name = (string)propertyStore.GetValue(DeviceProperties.PKEY_Device_FriendlyName);
        ContainerId = (Guid)propertyStore.GetValue(Ole32.PROPERTYKEY.System.Devices.ContainerId);
    }

    public void Connect()
    {
        Console.WriteLine($"Request to connect audio device '{Name}'");
        GetKsProperty(KSPROPERTY_BTAUDIO.KSPROPERTY_ONESHOT_RECONNECT);
    }

    public void Disconnect()
    {
        Console.WriteLine($"Request to disconnect audio device '{Name}'");
        GetKsProperty(KSPROPERTY_BTAUDIO.KSPROPERTY_ONESHOT_DISCONNECT);
    }

    private void GetKsProperty(KSPROPERTY_BTAUDIO btAudioProperty)
    {
        var ksProperty = new KsProperty(KsPropertyId.KSPROPSETID_BtAudio, btAudioProperty, KsPropertyKind.KSPROPERTY_TYPE_GET);
        var dwReturned = 0;
        ksControl.KsProperty(ksProperty, Marshal.SizeOf(ksProperty), IntPtr.Zero, 0, ref dwReturned);
    }
}
