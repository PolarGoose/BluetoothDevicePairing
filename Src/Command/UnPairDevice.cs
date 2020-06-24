using System;
using BluetoothDevicePairing.Bluetooth;
using BluetoothDevicePairing.Command.Utils;
using CommandLine;

namespace BluetoothDevicePairing.Command
{
    [Verb("unpair", HelpText = "Unpair a device")]
    internal sealed class UnpairDeviceOptions : PairAndUnpairDeviceOptions
    {
    }

    internal sealed class UnPairDevice
    {
        public void Execute(UnpairDeviceOptions opts)
        {
            if (!string.IsNullOrEmpty(opts.Mac))
            {
                UnpairByMac(new MacAddress(opts.Mac));
            }
            else if (!string.IsNullOrEmpty(opts.DeviceName))
            {
                UnpairByName(opts.DeviceName);
            }
            else
            {
                throw new Exception("Mac or device name must be specified");
            }
        }

        public void UnpairByMac(MacAddress mac)
        {
            DevicePairer.UnpairDevice(DeviceFinder.FindDeviceByMac(DeviceDiscoverer.DiscoverBluetoothDevices(10), mac));
        }

        public void UnpairByName(string name)
        {
            var devices = DeviceFinder.FindDevicesByName(DeviceDiscoverer.DiscoverBluetoothDevices(10), name);
            if (devices.Count == 1)
            {
                DevicePairer.UnpairDevice(devices[0]);
                return;
            }

            throw new Exception(
                $"{devices.Count} devices with the name '{name}' found. Don't know which one to choose");
        }
    }
}
