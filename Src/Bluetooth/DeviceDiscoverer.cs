using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Windows.Devices.Enumeration;

namespace BluetoothDevicePairing.Bluetooth
{
    internal sealed class DeviceDiscoverer
    {
        private readonly List<Device> _discoveredDevices = new List<Device>();
        private readonly DeviceWatcher _watcher;
        private readonly AutoResetEvent _watcherStopped = new AutoResetEvent(false);

        public DeviceDiscoverer()
        {
            _watcher = DeviceInformation.CreateWatcher(GetAqsFilter(), null, DeviceInformationKind.AssociationEndpoint);
            _watcher.Added += OnNewDeviceDiscovered;
            _watcher.Removed += OnDeviceRemoved;
            _watcher.Updated += OnDeviceUpdated;
            _watcher.Stopped += OnStopped;
        }

        public List<Device> DiscoverDevices()
        {
            var timeToDiscoverInSec = 10;
            Console.WriteLine($"Start discovering devices for {timeToDiscoverInSec} seconds");
            _watcher.Start();
            Thread.Sleep(timeToDiscoverInSec * 1000);
            _watcher.Stop();
            _watcherStopped.WaitOne(5 * 1000);
            if (_discoveredDevices.Count == 0) throw new Exception("No devices were found");
            Console.WriteLine("Finished discovering");
            return _discoveredDevices;
        }

        private void OnNewDeviceDiscovered(DeviceWatcher sender, DeviceInformation info)
        {
            _discoveredDevices.Add(new Device(info));
        }

        private void OnDeviceRemoved(DeviceWatcher sender, DeviceInformationUpdate removedDevice)
        {
            foreach (var device in _discoveredDevices.Where(device => device.Info.Id == removedDevice.Id))
                _discoveredDevices.Remove(device);
        }

        private void OnDeviceUpdated(DeviceWatcher sender, DeviceInformationUpdate deviceInfoUpdate)
        {
            foreach (var device in _discoveredDevices.Where(device => device.Info.Id == deviceInfoUpdate.Id))
                device.Info.Update(deviceInfoUpdate);
        }

        private void OnStopped(DeviceWatcher sender, object obj)
        {
            _watcherStopped.Set();
        }

        private static string GetAqsFilter()
        {
            const string bluetooth = "System.Devices.Aep.ProtocolId:=\"{e0cbf06c-cd8b-4647-bb8a-263b43f0f974}\"";
            const string bluetoothLe = "System.Devices.Aep.ProtocolId:=\"{bb7bb05e-5972-42b5-94fc-76eaa7084d49}\"";
            const string pairable =
                "System.Devices.Aep.CanPair:=System.StructuredQueryType.Boolean#True OR System.Devices.Aep.IsPaired:=System.StructuredQueryType.Boolean#True";
            return $"(({bluetooth}) OR ({bluetoothLe})) AND ({pairable})";
        }
    }
}
