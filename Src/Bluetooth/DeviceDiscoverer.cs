using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Windows.Devices.Enumeration;

namespace BluetoothDevicePairing.Bluetooth
{
    internal sealed class DeviceDiscoverer
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
            if (discoveryTimeInSec < 0)
            {
                throw new Exception("Discovery time is less than 0");
            }

            var devices = new List<Device>();
            var watcherStopped = new AutoResetEvent(false);
            var watcher = CreateWatcher(filter, devices, watcherStopped);
            watcher.Start();
            Thread.Sleep(discoveryTimeInSec * 1000);
            watcher.Stop();
            watcherStopped.WaitOne(5 * 1000);
            return devices;
        }

        private static DeviceWatcher CreateWatcher(AsqFilter filter, List<Device> devices, AutoResetEvent stoppedEvent)
        {
            var watcher =
                DeviceInformation.CreateWatcher(filter.Query, null, DeviceInformationKind.AssociationEndpoint);

            watcher.Added += (s, info) => { devices.Add(new Device(info)); };
            watcher.Removed += (s, removedDevice) =>
            {
                foreach (var device in devices.Where(device => device.Info.Id == removedDevice.Id))
                    devices.Remove(device);
            };
            watcher.Updated += (s, updatedDevice) =>
            {
                foreach (var device in devices.Where(device => device.Info.Id == updatedDevice.Id))
                    device.Info.Update(updatedDevice);
            };
            watcher.Stopped += (s, o) => { stoppedEvent.Set(); };

            return watcher;
        }
    }
}
