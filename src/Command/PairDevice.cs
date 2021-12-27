using BluetoothDevicePairing.Bluetooth;
using BluetoothDevicePairing.Command.Utils;
using CommandLine;
using System;

namespace BluetoothDevicePairing.Command
{
    [Verb("pair",
        HelpText = "Pair and connect to a device. If device is paired but not connected, this command unpairs it first and then pairs and connects")]
    internal sealed class PairDeviceOptions : PairAndUnpairDeviceOptions
    {
        [Option("discovery-time", Default = 10,
            HelpText = "how long to search for devices. Units: seconds")]
        public int DiscoveryTime { get; set; }

        [Option("pin", Default = "0000",
            HelpText = "pin code to provide to a device if it requires it for pairing")]
        public string Pin { get; set; }
    }

    internal static class PairDevice
    {
        public static void Execute(PairDeviceOptions opts)
        {
            if (!string.IsNullOrEmpty(opts.Mac))
            {
                PairWithMac(new MacAddress(opts.Mac), opts.DiscoveryTime, opts.Type, opts.Pin);
            }
            else if (!string.IsNullOrEmpty(opts.DeviceName))
            {
                PairWithName(opts.DeviceName, opts.DiscoveryTime, opts.Type, opts.Pin);
            }
            else
            {
                throw new Exception("Mac or device name must be specified");
            }
        }

        private static void PairWithMac(MacAddress mac, int discoveryTime, Utils.DeviceType deviceType, string pin)
        {
            var devices = DeviceFinder.FindDevicesByMac(DeviceDiscoverer.DiscoverBluetoothDevices(discoveryTime), mac, deviceType);

            if (devices.Count == 1)
            {
                DevicePairer.PairDevice(devices[0], pin);
                return;
            }

            if (devices.Count == 2)
            {
                throw new Exception(
                    $"2 devices with the mac '{mac}' found \"{devices[0]}\" and \"{devices[1]}\". Don't know which one to choose.\n");
            }

            throw new Exception(
                $"{devices.Count} devices with the mac '{mac}' found. Don't know which one to choose");
        }

        private static void PairWithName(string name, int discoveryTime, Utils.DeviceType deviceType, string pin)
        {
            var devices = DeviceFinder.FindDevicesByName(DeviceDiscoverer.DiscoverBluetoothDevices(discoveryTime), name, deviceType);
            if (devices.Count == 1)
            {
                DevicePairer.PairDevice(devices[0], pin);
                return;
            }

            if (devices.Count == 2 && devices[0].Type == Bluetooth.DeviceType.BluetoothLE && devices[1].Type == Bluetooth.DeviceType.BluetoothLE)
            {
                HandleSituation_2_BluetoothLe_devices_with_the_same_name_found(devices[0], devices[1], pin);
                return;
            }

            throw new Exception($"{devices.Count} devices with the name '{name}' found. Don't know which one to choose");
        }

        private static void HandleSituation_2_BluetoothLe_devices_with_the_same_name_found(Device device1,
            Device device2, string pin)
        {
            // BLuetooth LE devices use mac randomization, which can lead to the situation when
            // the user already have the device paired but with different mac address.
            // In this situation we need to unpair the old device and pair the current one.
            // If we don't unpair the old device first, the pairing procedure will fail.
            if (device1.IsPaired || device2.IsPaired)
            {
                var oldDevice = device1.IsPaired ? device1 : device2;
                var newDevice = !device1.IsPaired ? device1 : device2;

                Console.WriteLine($"2 devices with the same name found: \"{oldDevice}\" (paired)  and \"{newDevice}\"");
                Console.WriteLine("Assume that the device changed its mac address");
                DevicePairer.UnpairDevice(oldDevice);
                DevicePairer.PairDevice(newDevice, pin);
                return;
            }

            throw new Exception($"2 unpaired devices with the same name found \"{device1}\" \"{device2}\". Don't know which one to choose.");
        }
    }
}
