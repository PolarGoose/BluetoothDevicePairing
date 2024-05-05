using BluetoothDevicePairing.Bluetooth.Devices;
using BluetoothDevicePairing.Commands.Utils;
using CommandLine;

namespace BluetoothDevicePairing.Commands;

[Verb("pair-by-mac",
      HelpText = "Pair a device using its mac address. This command can also be used to connect to an already paired audio Bluetooth device.")]
internal sealed class PairDeviceByMacOptions : MacAndDeviceTypeOptions
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
