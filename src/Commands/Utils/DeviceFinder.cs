using BluetoothDevicePairing.Bluetooth.Adapter;
using BluetoothDevicePairing.Bluetooth.Devices;
using System;
using System.Collections.Generic;

namespace BluetoothDevicePairing.Commands.Utils
{
    internal static class DeviceFinder
    {
        public static List<Device> FindDevicesByName(int discoveryTimeInSeconds, string name, DeviceType deviceType)
        {
            var devices = DeviceDiscoverer.DiscoverBluetoothDevices(discoveryTimeInSeconds);
            var res = devices.FindAll(d => d.Name == name && deviceType == d.Type);

            if (res.Count == 0)
            {
                throw new Exception($"Couldn't find any devices with '{name}' name and device type '{deviceType}'");
            }

            return res;
        }

        public static Device FindDevicesByMac(DeviceMacAddress mac, DeviceType deviceType)
        {
            Console.WriteLine("Find default bluetooth adapter");
            var defaultAdapter = AdapterFinder.FindDefaultAdapter();
            Console.WriteLine($"Default bluetooth adapter found: \"{defaultAdapter}\"");

            var deviceId = $"{deviceType}#{deviceType}{defaultAdapter.MacAddress}-{mac}";

            Console.WriteLine($"Create device information using ID '{deviceId}'");
            var deviceInfo = Windows.Devices.Enumeration.DeviceInformation.CreateFromIdAsync(deviceId).GetAwaiter().GetResult();

            return new(deviceInfo);
        }
    }
}
