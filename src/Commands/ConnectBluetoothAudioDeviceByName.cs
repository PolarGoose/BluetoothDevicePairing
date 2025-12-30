using BluetoothDevicePairing.Bluetooth.Devices;
using BluetoothDevicePairing.Commands.Utils;
using BluetoothDevicePairing.Utils;
using CommandLine;

namespace BluetoothDevicePairing.Commands;

[Verb("connect-bluetooth-audio-device-by-name",
      HelpText = "Connect a Bluetooth audio device using its name")]
internal sealed class ConnectBluetoothAudioDeviceByNameOptions : PairAndUnpairDeviceByNameOptions
{
}

internal static class ConnectBluetoothAudioDeviceByName
{
    public static void Execute(ConnectBluetoothAudioDeviceByNameOptions opts)
    {
        var devices = DeviceFinder.FindDevicesByName(DeviceDiscoverer.DiscoverPairedBluetoothDevices(new DiscoveryTime(opts.DiscoveryTime)),
                                                     opts.DeviceName,
                                                     opts.DeviceType);
        if (devices.Count > 1)
        {
            throw new AppException($"{devices.Count} devices found, don't know which one to choose");
        }

        AudioDeviceConnector.ConnectBluetoothAudioDevice(devices[0]);
    }
}
