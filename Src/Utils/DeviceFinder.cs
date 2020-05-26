using System;
using System.Collections.Generic;
using System.Linq;

namespace BluetoothDevicePairing.Bluetooth
{
    internal class DeviceFinder
    {
        public Device FindDeviceByMac(List<Device> devices, string mac)
        {
            foreach (var d in devices.Where(d => d.Mac == mac))
                return d;
            throw new Exception($"Couldn't find the device with '{mac}' mac address");
        }

        public List<Device> FindDevicesByName(List<Device> devices, string name)
        {
            var res = new List<Device>();
            foreach (var d in devices.Where(d => d.Name == name))
                res.Add(d);
            if (res.Count == 0) throw new Exception($"Couldn't find any devices with '{name}' name");
            return res;
        }
    }
}
