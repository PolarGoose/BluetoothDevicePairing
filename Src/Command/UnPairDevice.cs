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
                UnpairByMac(new MacAddress(opts.Mac), opts.DiscoveryTime);
            }
            else if (!string.IsNullOrEmpty(opts.DeviceName))
            {
                UnpairByName(opts.DeviceName, opts.DiscoveryTime);
            }
            else
            {
                throw new Exception("Mac or device name must be specified");
            }
        }

        public static void UnpairByMac(MacAddress mac, int discoveryTime)
        {
            var devices = DeviceFinder.FindDevicesByMac(DeviceDiscoverer.DiscoverPairedBluetoothDevices(discoveryTime), mac);

            if (devices.Count == 1)
            {
                DevicePairer.UnpairDevice(devices[0]);
                return;
            }

            if (devices.Count == 2 && devices[0].Type != devices[1].Type && devices[0].Name == devices[1].Name)
            {
                HandleSituation_2_Bluetooth_devices_with_the_same_mac_and_name_found(devices[0], devices[1]);
                return;
            }

            throw new Exception($"{devices.Count} devices found, don't know which one to choose");
        }

        public static void UnpairByName(string name, int discoveryTime)
        {
            var devices = DeviceFinder.FindDevicesByName(DeviceDiscoverer.DiscoverPairedBluetoothDevices(discoveryTime), name);

            if (devices.Count == 1)
            {
                DevicePairer.UnpairDevice(devices[0]);
                return;
            }

            if (devices.Count == 2 && devices[0].Type != devices[1].Type && devices[0].Mac.Equals(devices[1].Mac))
            {
                HandleSituation_2_Bluetooth_devices_with_the_same_mac_and_name_found(devices[0], devices[1]);
                return;
            }

            throw new Exception($"{devices.Count} devices found, don't know which one to choose");
        }

        private static void HandleSituation_2_Bluetooth_devices_with_the_same_mac_and_name_found(Device device1,
            Device device2)
        {
            Console.WriteLine(
                $"Two devices with the same mac address and name but different bluetooth types found: \"{device1}\"  and \"{device1}\"." +
                "It is possible that it is one device which advertises itself as Bluetooth and BluetoothLE simultaneously." +
                "Unpair both of them.");

            DevicePairer.UnpairDevice(device1);
            DevicePairer.UnpairDevice(device2);
        }
    }
}
