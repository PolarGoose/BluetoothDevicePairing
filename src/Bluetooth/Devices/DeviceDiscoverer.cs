using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BluetoothDevicePairing.Bluetooth.Devices
{
    internal static class DeviceDiscoverer
    {
        public static List<Device> DiscoverBluetoothDevices(int timeoutInSec)
        {
            return Discover(AsqFilter.BluetoothDevicesFilter(), timeoutInSec);
        }

        public static List<Device> DiscoverPairedBluetoothDevices(int timeoutInSec)
        {
            return Discover(AsqFilter.PairedBluetoothDevicesFilter(), timeoutInSec);
        }

        private static List<Device> Discover(AsqFilter filter, int discoveryTimeInSec)
        {
            Console.WriteLine($"Start discovering devices for {discoveryTimeInSec} seconds");

            if (discoveryTimeInSec < 1 || discoveryTimeInSec > 30)
            {
                throw new Exception($"discovery time should be in range [1; 30] but was {discoveryTimeInSec}");
            }

            var watcher = new DeviceWatcher(filter);
            watcher.Start();
            Thread.Sleep(discoveryTimeInSec * 1000);
            var devices = watcher.Stop();
            return devices.Select(d => new Device(d)).ToList();
        }
    }
}
