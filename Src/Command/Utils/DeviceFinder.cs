using System;
using System.Collections.Generic;
using BluetoothDevicePairing.Bluetooth;

namespace BluetoothDevicePairing.Command.Utils
{
    internal sealed class DeviceFinder
    {
        public static List<Device> FindDevicesByMac(List<Device> devices, MacAddress mac)
        {
            var res = devices.FindAll(d => d.Mac.Equals(mac));
            if (res == null)
            {
                throw new Exception($"Couldn't find any devices with '{mac}' mac address");
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
