using BluetoothDevicePairing.Bluetooth.Devices;
using BluetoothDevicePairing.Commands.Utils;
using CommandLine;

namespace BluetoothDevicePairing.Commands;

[Verb("unpair-by-mac", HelpText = "Unpair a device using its mac address")]
internal sealed class UnpairDeviceByMacOptions : MacAndDeviceTypeOptions
{
}

internal static class UnPairDeviceByMac
{
    public static void Execute(UnpairDeviceByMacOptions opts)
    {
        var mac = new DeviceMacAddress(opts.Mac);
        var device = DeviceFinder.FindDevicesByMac(mac, opts.DeviceType);
        DeviceUnPairer.UnpairDevice(device);
    }
}
