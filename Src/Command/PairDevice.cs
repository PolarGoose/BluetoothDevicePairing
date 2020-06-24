using System;
using BluetoothDevicePairing.Bluetooth;
using BluetoothDevicePairing.Command.Utils;
using CommandLine;

namespace BluetoothDevicePairing.Command
{
    [Verb("pair", HelpText = "Pair and connect to a device")]
    internal sealed class PairDeviceOptions : PairAndUnpairDeviceOptions
    {
    }

    internal sealed class PairDevice
    {
        public void Execute(PairDeviceOptions opts)
        {
            if (!string.IsNullOrEmpty(opts.Mac))
            {
                PairWithMac(new MacAddress(opts.Mac));
            }
            else if (!string.IsNullOrEmpty(opts.DeviceName))
            {
                PairWithName(opts.DeviceName);
            }
            else
            {
                throw new Exception("Mac or device name must be specified");
            }
        }

        private static void PairWithMac(MacAddress mac)
        {
            DevicePairer.PairDevice(DeviceFinder.FindDeviceByMac(DeviceDiscoverer.DiscoverBluetoothDevices(10), mac));
        }

        private static void PairWithName(string name)
        {
            var devices = DeviceFinder.FindDevicesByName(DeviceDiscoverer.DiscoverBluetoothDevices(10), name);
            if (devices.Count == 1)
            {
                DevicePairer.PairDevice(devices[0]);
                return;
            }

            if (devices.Count == 2 && devices[0].Type == DeviceType.BluetoothLe &&
                devices[1].Type == DeviceType.BluetoothLe)
            {
                HandleSituation_2_BluetoothLe_devices_with_the_same_name_found(devices[0], devices[1]);
                return;
            }

            throw new Exception(
                $"{devices.Count} devices with the name '{name}' found. Don't know which one to choose");
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

            throw new Exception(
                $"2 unpaired devices with the same name found \"{device1}\" \"{device2}\". Don't know which one to choose");
        }
    }
}
