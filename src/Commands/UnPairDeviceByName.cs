using BluetoothDevicePairing.Bluetooth.Devices;
using BluetoothDevicePairing.Commands.Utils;
using BluetoothDevicePairing.Utils;
using CommandLine;
using System;

namespace BluetoothDevicePairing.Commands;

[Verb("unpair-by-name", HelpText = "Unpair a device using its name")]
internal sealed class UnpairDeviceByNameOptions : PairAndUnpairDeviceByNameOptions
{
}

internal static class UnPairDeviceByName
{
    public static void Execute(UnpairDeviceByNameOptions opts)
    {
        var devices = DeviceFinder.FindDevicesByName(DeviceDiscoverer.DiscoverPairedBluetoothDevices(new DiscoveryTime(opts.DiscoveryTime)),
                                                     opts.DeviceName,
                                                     opts.DeviceType);
        if (devices.Count > 1)
        {
            throw new AppException($"{devices.Count} devices found, don't know which one to choose");
        }

        DeviceUnPairer.UnpairDevice(devices[0]);
    }
}
