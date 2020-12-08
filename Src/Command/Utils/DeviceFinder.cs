using System;
using System.Collections.Generic;
using BluetoothDevicePairing.Bluetooth;

namespace BluetoothDevicePairing.Command.Utils
{
    internal sealed class DeviceFinder
    {
        public static List<Device> FindDevicesByMac(List<Device> devices, MacAddress mac, DeviceType deviceType)
        {
            var res = devices.FindAll(d => d.Mac.Equals(mac) && DeviceTypeExtensions.Equals(deviceType, d.Type));
            if (res == null)
            {
                throw new Exception($"Couldn't find any devices with '{mac}' mac address and device type '{deviceType}'");
            }

            return res;
        }

        public static List<Device> FindDevicesByName(List<Device> devices, string name, DeviceType deviceType)
        {
            var res = devices.FindAll(d => d.Name == name && DeviceTypeExtensions.Equals(deviceType, d.Type));

            if (res.Count == 0)
            {
                throw new Exception($"Couldn't find any devices with '{name}' name and device type '{deviceType}'");
            }

            return res;
        }
    }
}
