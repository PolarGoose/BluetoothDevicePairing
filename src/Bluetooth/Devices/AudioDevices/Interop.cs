using System.Runtime.InteropServices;
using System.Security;
using System;
using Vanara.PInvoke;

namespace BluetoothDevicePairing.Bluetooth.Devices.AudioDevices;

public static class DeviceProperties
{
    // https://github.com/microsoft/win32metadata/blob/main/generation/WinSDK/RecompiledIdlHeaders/um/functiondiscoverykeys_devpkey.h#L62
    public static Ole32.PROPERTYKEY PKEY_Device_FriendlyName = new(new("{a45c254e-df1c-4efd-8020-67d146a850e0}"), 14u);
}

public enum KsPropertyKind : uint
{
    KSPROPERTY_TYPE_GET = 0x00000001,
    KSPROPERTY_TYPE_SET = 0x00000002,
    KSPROPERTY_TYPE_TOPOLOGY = 0x10000000
}

public enum KSPROPERTY_BTAUDIO : uint
{
    KSPROPERTY_ONESHOT_RECONNECT = 0,
    KSPROPERTY_ONESHOT_DISCONNECT = 1
}

public static class KsPropertyId
{
    public static readonly Guid KSPROPSETID_BtAudio = new("7fa06c40-b8f6-4c7e-8556-e8c33a12e54d");
}

[StructLayout(LayoutKind.Sequential)]
public readonly struct KsProperty(Guid set, KSPROPERTY_BTAUDIO id, KsPropertyKind flags)
{
    public Guid Set { get; } = set;
    public KSPROPERTY_BTAUDIO Id { get; } = id;
    public KsPropertyKind Flags { get; } = flags;
}

[ComImport, SuppressUnmanagedCodeSecurity, Guid("28F54685-06FD-11D2-B27A-00A0C9223196"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
public interface IKsControl
{
    [PreserveSig]
    int KsProperty(
      [In] ref KsProperty Property,
      [In] int PropertyLength,
      [In, Out] IntPtr PropertyData,
      [In] int DataLength,
      [In, Out] ref int BytesReturned);

    [PreserveSig]
    int KsMethod(
      [In] ref KsProperty Method,
      [In] int MethodLength,
      [In, Out] IntPtr MethodData,
      [In] int DataLength,
      [In, Out] ref int BytesReturned);

    [PreserveSig]
    int KsEvent(
      [In, Optional] ref KsProperty Event,
      [In] int EventLength,
      [In, Out] IntPtr EventData,
      [In] int DataLength,
      [In, Out] ref int BytesReturned);
}
