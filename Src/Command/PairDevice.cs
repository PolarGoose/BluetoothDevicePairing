using System;
using BluetoothDevicePairing.Bluetooth;
using CommandLine;

namespace BluetoothDevicePairing.Command
{
    [Verb("pair", HelpText = "Pair and connect to a device")]
    internal sealed class PairDeviceOptions : PairUnpairDeviceOptions
    {
    }

    internal sealed class PairDevice
    {
        private readonly DeviceDiscoverer _discoverer = new DeviceDiscoverer();
        private readonly DeviceFinder _finder = new DeviceFinder();
        private readonly DevicePairer _pairer = new DevicePairer();

        public void Execute(PairDeviceOptions opts)
        {
            if (!string.IsNullOrEmpty(opts.Mac))
                PairWithMac(opts.Mac);
            else
                PairWithName(opts.DeviceName);
        }

        private void PairWithMac(string mac)
        {
            Utils.ValidateMac(mac);
            _pairer.PairDevice(_finder.FindDeviceByMac(_discoverer.DiscoverDevices(), mac));
        }

        private void PairWithName(string name)
        {
            var devices = _finder.FindDevicesByName(_discoverer.DiscoverDevices(), name);
            if (devices.Count == 1)
            {
                _pairer.PairDevice(devices[0]);
                return;
            }

            if (devices.Count == 2 && devices[0].Type == DeviceType.BluetoothLe &&
                devices[1].Type == DeviceType.BluetoothLe)
            {
                HandleSituation_2BluetoothLeDevicesWithTheSameNameFound(devices[0], devices[1]);
                return;
            }

            throw new Exception(
                $"{devices.Count} devices with the name '{name}' found. Don't know which one to choose");
        }

        private void HandleSituation_2BluetoothLeDevicesWithTheSameNameFound(Device device1, Device device2)
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
                _pairer.UnpairDevice(oldDevice);
                _pairer.PairDevice(newDevice);
                return;
            }

            throw new Exception(
                $"2 unpaired devices with the same name found \"{device1}\" \"{device2}\". Don't know which one to choose");
        }
    }
}
