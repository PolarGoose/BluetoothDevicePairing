using BluetoothDevicePairing.Bluetooth.Devices;
using BluetoothDevicePairing.Commands.Utils;
using CommandLine;
using System;

namespace BluetoothDevicePairing.Commands
{
    [Verb("pair-by-name",
          HelpText = "Pair and connect to a device using its name.")]
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
            var devices = DeviceFinder.FindDevicesByName(new DiscoveryTime(opts.DiscoveryTime), opts.DeviceName, opts.DeviceType);

            if (devices.Count == 1)
            {
                DevicePairer.PairDevice(devices[0], opts.PinCode);
                return;
            }

            if (devices.Count == 2)
            {
                throw new Exception(
                    $"2 devices with the name '{opts.DeviceName}' found \n 1 - \"{devices[0]}\" \n 2 - \"{devices[1]}\". Don't know which one to choose.");
            }

            throw new Exception(
                $"{devices.Count} devices with the name '{opts.DeviceName}' found. Don't know which one to choose");
        }
    }
}
