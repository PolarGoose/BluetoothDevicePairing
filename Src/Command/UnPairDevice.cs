using System;
using BluetoothDevicePairing.Bluetooth;
using BluetoothDevicePairing.Command.Utils;
using CommandLine;

namespace BluetoothDevicePairing.Command
{
    [Verb("unpair", HelpText = "Unpair a device")]
    internal sealed class UnpairDeviceOptions : PairAndUnpairDeviceOptions
    {
        [Option("discovery-time", Default = 2,
            HelpText = "how long to search for devices. Units: seconds")]
        public int DiscoveryTime { get; set; }
    }

    internal sealed class UnPairDevice
    {
        public static void Execute(UnpairDeviceOptions opts)
        {
            if (!string.IsNullOrEmpty(opts.Mac))
            {
                UnpairByMac(new MacAddress(opts.Mac), opts.DiscoveryTime, opts.Type);
            }
            else if (!string.IsNullOrEmpty(opts.DeviceName))
            {
                UnpairByName(opts.DeviceName, opts.DiscoveryTime, opts.Type);
            }
            else
            {
                throw new Exception("Mac or device name must be specified");
            }
        }

        public static void UnpairByMac(MacAddress mac, int discoveryTime, Utils.DeviceType deviceType)
        {
            var devices = DeviceFinder.FindDevicesByMac(DeviceDiscoverer.DiscoverPairedBluetoothDevices(discoveryTime), mac, deviceType);
            if (devices.Count > 1)
            {
                throw new Exception($"{devices.Count} devices found, don't know which one to choose");
            }

            DevicePairer.UnpairDevice(devices[0]);
        }

        public static void UnpairByName(string name, int discoveryTime, Utils.DeviceType deviceType)
        {
            var devices = DeviceFinder.FindDevicesByName(DeviceDiscoverer.DiscoverPairedBluetoothDevices(discoveryTime), name, deviceType);
            if (devices.Count > 1)
            {
                throw new Exception($"{devices.Count} devices found, don't know which one to choose");
            }

            DevicePairer.UnpairDevice(devices[0]);
        }
    }
}
