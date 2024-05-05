using BluetoothDevicePairing.Bluetooth.Devices;
using BluetoothDevicePairing.Commands.Utils;
using CommandLine;

namespace BluetoothDevicePairing.Commands;

[Verb("disconnect-bluetooth-audio-device-by-mac",
      HelpText = "Disconnect a Bluetooth audio device using its mac address")]
internal sealed class DisconnectBluetoothAudioDeviceByMacOptions : MacAndDeviceTypeOptions
{
}

internal static class DisconnectBluetoothAudioDeviceByMac
{
    public static void Execute(DisconnectBluetoothAudioDeviceByMacOptions opts)
    {
        var mac = new DeviceMacAddress(opts.Mac);
        var device = DeviceFinder.FindDevicesByMac(mac, opts.DeviceType);
        AudioDeviceDisconnector.DisconnectBluetoothAudioDevice(device);
    }
}
