using System;
using System.Collections.Generic;
using BluetoothDevicePairing.Bluetooth;

namespace BluetoothDevicePairing.Command.Utils
{
    internal sealed class DeviceFinder
    {
        public static Device FindDeviceByMac(List<Device> devices, MacAddress mac)
        {
            var res = devices.Find(d => d.Mac.Address == mac.Address);
            if (res == null)
            {
                throw new Exception($"Couldn't find the device with '{mac}' mac address");
            }

            return res;
        }

        public static List<Device> FindDevicesByName(List<Device> devices, string name)
        {
            var res = devices.FindAll(d => d.Name == name);

            if (res.Count == 0)
            {
                throw new Exception($"Couldn't find any devices with '{name}' name");
            }

            return res;
        }
    }
}
