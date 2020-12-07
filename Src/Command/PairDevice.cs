using System;
using BluetoothDevicePairing.Bluetooth;
using BluetoothDevicePairing.Command.Utils;
using CommandLine;

namespace BluetoothDevicePairing.Command
{
    [Verb("pair",
        HelpText = "Pair and connect to a device. If device is paired but not connected, this command unpairs it first and then pairs and connects")]
    internal sealed class PairDeviceOptions : PairAndUnpairDeviceOptions
    {
        [Option("discovery-time", Default = 10,
            HelpText = "how long to search for devices. Units: seconds")]
        public int DiscoveryTime { get; set; }
    }

    internal sealed class PairDevice
    {
        public static void Execute(PairDeviceOptions opts)
        {
            if (!string.IsNullOrEmpty(opts.Mac))
            {
                PairWithMac(new MacAddress(opts.Mac), opts.DiscoveryTime);
            }
            else if (!string.IsNullOrEmpty(opts.DeviceName))
            {
                PairWithName(opts.DeviceName, opts.DiscoveryTime);
            }
            else
            {
                throw new Exception("Mac or device name must be specified");
            }
        }

        private static void PairWithMac(MacAddress mac, int discoveryTime)
        {
            var devices = DeviceFinder.FindDevicesByMac(DeviceDiscoverer.DiscoverBluetoothDevices(discoveryTime), mac);

            if (devices.Count == 1)
            {
                DevicePairer.PairDevice(devices[0]);
                return;
            }

            if (devices.Count == 2 && devices[0].Type != devices[1].Type && devices[0].Name == devices[1].Name)
            {
                HandleSituation_2_Bluetooth_devices_with_the_same_mac_and_name_found(devices[0], devices[1]);
                return;
            }

            throw new Exception(
                $"{devices.Count} devices with the mac '{mac}' found. Don't know which one to choose");
        }

        private static void PairWithName(string name, int discoveryTime)
        {
            var devices = DeviceFinder.FindDevicesByName(DeviceDiscoverer.DiscoverBluetoothDevices(discoveryTime), name);
            if (devices.Count == 1)
            {
                DevicePairer.PairDevice(devices[0]);
                return;
            }

            if (devices.Count == 2 && devices[0].Type != devices[1].Type && devices[0].Mac.Equals(devices[1].Mac))
            {
                HandleSituation_2_Bluetooth_devices_with_the_same_mac_and_name_found(devices[0], devices[1]);
                return;
            }

            if (devices.Count == 2 && devices[0].Type == DeviceType.BluetoothLe &&
                devices[1].Type == DeviceType.BluetoothLe)
            {
                HandleSituation_2_BluetoothLe_devices_with_the_same_name_found(devices[0], devices[1]);
                return;
            }

            throw new Exception($"{devices.Count} devices with the name '{name}' found. Don't know which one to choose");
        }

        private static void HandleSituation_2_BluetoothLe_devices_with_the_same_name_found(Device device1,
            Device device2)
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
                DevicePairer.PairDevice(newDevice);
                return;
            }

            throw new Exception($"2 unpaired devices with the same name found \"{device1}\" \"{device2}\". Don't know which one to choose.");
        }

        private static void HandleSituation_2_Bluetooth_devices_with_the_same_mac_and_name_found(Device device1,
            Device device2)
        {
            Console.WriteLine(
                $"Two devices with the same mac address and name but different bluetooth types found: \"{device1}\"  and \"{device1}\". " +
                "It is possible that it is one device which advertises itself as Bluetooth and BluetoothLE simultaneously." +
                "Pair both of them.");

            // for unknown reasons we first need to pair BluetoothLE one. Otherwise non LE device pairing fail
            var leDevice = device1.Type == DeviceType.BluetoothLe ? device1 : device2;
            var nonLeDevice = device1.Type == DeviceType.Bluetooth ? device1 : device2;

            DevicePairer.PairDevice(leDevice);
            DevicePairer.PairDevice(nonLeDevice);
        }
    }
}
