using BluetoothDevicePairing.Bluetooth.Devices;
using BluetoothDevicePairing.Commands.Utils;
using CommandLine;

namespace BluetoothDevicePairing.Commands
{
    [Verb("pair-by-mac",
          HelpText = "Pair and connect to a device using its mac address")]
    internal sealed class PairDeviceByMacOptions : PairAndUnpairDeviceByMacOptions
    {
        [Option("pin",
                Default = "0000",
                HelpText = "pin code to provide to a device if it requires it for pairing")]
        public string PinCode { get; set; }
    }

    internal static class PairDeviceByMac
    {
        public static void Execute(PairDeviceByMacOptions opts)
        {
            var mac = new DeviceMacAddress(opts.Mac);

            var device = DeviceFinder.FindDevicesByMac(mac, opts.DeviceType);
            DevicePairer.PairDevice(device, opts.PinCode);
        }
    }
}
