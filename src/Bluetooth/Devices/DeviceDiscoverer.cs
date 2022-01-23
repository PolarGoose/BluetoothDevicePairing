using BluetoothDevicePairing.Bluetooth.Devices.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace BluetoothDevicePairing.Bluetooth.Devices
{
    internal class DiscoveryTime
    {
        public int Seconds { get; }

        public DiscoveryTime(int timeInSeconds)
        {
            if (timeInSeconds < 1 || timeInSeconds > 30)
            {
                throw new Exception($"discovery time should be in range [1; 30] but was {timeInSeconds}");
            }

            Seconds = timeInSeconds;
        }
    }

    internal static class DeviceDiscoverer
    {
        public static List<Device> DiscoverBluetoothDevices(DiscoveryTime time)
        {
            return Discover(AsqFilter.BluetoothDevicesFilter(), time);
        }

        public static List<Device> DiscoverPairedBluetoothDevices(DiscoveryTime time)
        {
            return Discover(AsqFilter.PairedBluetoothDevicesFilter(), time);
        }

        private static List<Device> Discover(AsqFilter filter, DiscoveryTime time)
        {
            Console.WriteLine($"Start discovering devices for {time.Seconds} seconds");

            var watcher = new DeviceWatcher(filter);
            watcher.Start();
            Thread.Sleep(time.Seconds * 1000);
            var devices = watcher.Stop();
            return devices.Select(info => CreateDevice(info)).ToList();
        }

        private static Device CreateDevice(Windows.Devices.Enumeration.DeviceInformation info)
        {
            return new DeviceInfoId(info).DeviceType == DeviceType.Bluetooth
                ? BluetoothDevice.FromDeviceInfo(info)
                : BluetoothLeDevice.FromDeviceInfo(info);
        }
    }
}
