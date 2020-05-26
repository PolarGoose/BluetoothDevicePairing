using System;
using BluetoothDevicePairing.Bluetooth;
using CommandLine;

namespace BluetoothDevicePairing.Command
{
    [Verb("unpair", HelpText = "Unpair a device")]
    internal sealed class UnpairDeviceOptions : PairUnpairDeviceOptions
    {
    }

    internal sealed class UnPairDevice
    {
        private readonly DeviceDiscoverer _discoverer = new DeviceDiscoverer();
        private readonly DeviceFinder _finder = new DeviceFinder();
        private readonly DevicePairer _pairer = new DevicePairer();

        public void Execute(UnpairDeviceOptions opts)
        {
            if (!string.IsNullOrEmpty(opts.Mac))
                UnpairByMac(opts.Mac);
            else
                UnpairByName(opts.DeviceName);
        }

        public void UnpairByMac(string mac)
        {
            Utils.ValidateMac(mac);
            _pairer.UnpairDevice(_finder.FindDeviceByMac(_discoverer.DiscoverDevices(), mac));
        }

        public void UnpairByName(string name)
        {
            var devices = _finder.FindDevicesByName(_discoverer.DiscoverDevices(), name);
            if (devices.Count == 1)
            {
                _pairer.UnpairDevice(devices[0]);
                return;
            }

            throw new Exception(
                $"{devices.Count} devices with the name '{name}' found. Don't know which one to choose");
        }
    }
}
