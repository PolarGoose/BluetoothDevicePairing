using BluetoothDevicePairing.Bluetooth.Devices;
using BluetoothDevicePairing.Commands.Utils;
using CommandLine;

namespace BluetoothDevicePairing.Commands;

[Verb("connect-bluetooth-audio-device-by-mac",
      HelpText = "Connect a Bluetooth audio device using its mac address")]
internal sealed class ConnectBluetoothAudioDeviceByMacOptions : MacAndDeviceTypeOptions
{
}

internal static class ConnectBluetoothAudioDeviceByMac
{
    public static void Execute(ConnectBluetoothAudioDeviceByMacOptions opts)
    {
        var mac = new DeviceMacAddress(opts.Mac);
        var device = DeviceFinder.FindDevicesByMac(mac, opts.DeviceType);
        AudioDeviceConnector.ConnectBluetoothAudioDevice(device);
    }
}
