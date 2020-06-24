using System;
using System.Text.RegularExpressions;
using Windows.Devices.Enumeration;

namespace BluetoothDevicePairing.Bluetooth
{
    internal sealed class MacAddress
    {
        public MacAddress(DeviceInformation device)
        {
            var match = Regex.Match(device.Id, @"(..:){5}(..)$");
            if (!match.Success) throw new Exception($"Failed to extract mac address from the string '{device.Id}'");
            Address = match.Value.ToUpper();
        }

        public MacAddress(string mac)
        {
            var match = Regex.Match(mac, @"^(..:){5}(..)$");
            if (!match.Success) throw new Exception($"MacAddress address '{mac}' is not a valid mac address");
            Address = mac;
        }

        public string Address { get; }

        public override string ToString()
        {
            return Address;
        }
    }
}
