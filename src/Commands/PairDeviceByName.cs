using BluetoothDevicePairing.Bluetooth.Devices;
using BluetoothDevicePairing.Commands.Utils;
using BluetoothDevicePairing.Utils;
using CommandLine;
using System;

namespace BluetoothDevicePairing.Commands;

[Verb("pair-by-name",
      HelpText = "Pair a device using its name. This command can also be used to connect to an already paired audio Bluetooth device.")]
internal sealed class PairDeviceByNameOptions : PairAndUnpairDeviceByNameOptions
{
    [Option("pin",
            Default = "0000",
            HelpText = "pin code to provide to a device if it requires it for pairing")]
    public string PinCode { get; set; }
}

internal static class PairDeviceByName
{
    public static void Execute(PairDeviceByNameOptions opts)
    {
        var devices = DeviceFinder.FindDevicesByName(DeviceDiscoverer.DiscoverBluetoothDevices(new DiscoveryTime(opts.DiscoveryTime)),
                                                     opts.DeviceName,
                                                     opts.DeviceType);

        switch (devices.Count)
        {
            case 1:
                DevicePairer.PairDevice(devices[0], opts.PinCode);
                return;
            case 2:
                throw new AppException(
                    $"2 devices with the name '{opts.DeviceName}' found \n 1 - \"{devices[0]}\" \n 2 - \"{devices[1]}\". Don't know which one to choose.");
            default:
                throw new AppException(
                    $"{devices.Count} devices with the name '{opts.DeviceName}' found. Don't know which one to choose");
        }
    }
}
