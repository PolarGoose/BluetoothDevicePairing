using BluetoothDevicePairing.Bluetooth.Devices;
using System;
using System.Collections.Generic;

namespace BluetoothDevicePairing.Commands.Utils
{
    internal static class DeviceFinder
    {
        public static List<Device> FindDevicesByName(DiscoveryTime time, string name, DeviceType deviceType)
        {
            var devices = DeviceDiscoverer.DiscoverBluetoothDevices(time);
            var res = devices.FindAll(d => d.Name == name && deviceType == d.Id.DeviceType);

            if (res.Count == 0)
            {
                throw new Exception($"Couldn't find any devices with '{name}' name and device type '{deviceType}'");
            }

            return res;
        }

        public static Device FindDevicesByMac(DeviceMacAddress mac, DeviceType deviceType)
        {
            if (deviceType == DeviceType.Bluetooth)
            {
                Console.WriteLine($"Create Bluetooth device using Mac:'{mac}'");
                return BluetoothDevice.FromMac(mac);
            }
            Console.WriteLine($"Create BluetoothLE device using Mac:'{mac}'");
            return BluetoothLeDevice.FromMac(mac);
        }
    }
}
