using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BluetoothDevicePairing.Bluetooth.Devices
{
    internal sealed class DeviceWatcher
    {
        private readonly Windows.Devices.Enumeration.DeviceWatcher watcher;
        private readonly AutoResetEvent watcherStoppedEvent = new(false);
        private List<Windows.Devices.Enumeration.DeviceInformation> devices;

        public DeviceWatcher(AsqFilter filter)
        {
            watcher = Windows.Devices.Enumeration.DeviceInformation.CreateWatcher(filter.Query,
                                                                                  null,
                                                                                  Windows.Devices.Enumeration.DeviceInformationKind.AssociationEndpoint);

            watcher.Added += (s, info) =>
            {
                devices.Add(info);
            };

            watcher.Removed += (s, removedDevice) =>
            {
                foreach (var device in devices.Where(device => device.Id == removedDevice.Id))
                {
                    devices.Remove(device);
                }
            };

            watcher.Updated += (s, updatedDevice) =>
            {
                foreach (var device in devices.Where(device => device.Id == updatedDevice.Id))
                {
                    device.Update(updatedDevice);
                }
            };

            watcher.Stopped += (s, o) =>
            {
                watcherStoppedEvent.Set();
            };
        }

        public void Start()
        {
            devices = new();
            watcher.Start();
        }

        public List<Windows.Devices.Enumeration.DeviceInformation> Stop()
        {
            watcher.Stop();
            var receivedSignal = watcherStoppedEvent.WaitOne(5 * 1000);
            if (!receivedSignal)
            {
                Console.WriteLine("Warning: the watcher didn't stop after 5 seconds");
            }

            return devices;
        }
    }
}
