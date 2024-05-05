using BluetoothDevicePairing.Bluetooth.Devices;
using BluetoothDevicePairing.Commands.Utils;
using CommandLine;
using System;

namespace BluetoothDevicePairing.Commands;

[Verb("disconnect-bluetooth-audio-device-by-name",
      HelpText = "Disconnect a Bluetooth audio device using its name")]
internal sealed class DisconnectBluetoothAudioDeviceByNameOptions : PairAndUnpairDeviceByNameOptions
{
}

internal static class DisconnectBluetoothAudioDeviceByName
{
    public static void Execute(DisconnectBluetoothAudioDeviceByNameOptions opts)
    {
        var devices = DeviceFinder.FindDevicesByName(DeviceDiscoverer.DiscoverPairedBluetoothDevices(new DiscoveryTime(opts.DiscoveryTime)),
                                                     opts.DeviceName,
                                                     opts.DeviceType);
        if (devices.Count > 1)
        {
            throw new Exception($"{devices.Count} devices found, don't know which one to choose");
        }

        AudioDeviceDisconnector.DisconnectBluetoothAudioDevice(devices[0]);
    }
}
