using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace BluetoothDevicePairing.Bluetooth
{
    internal class MacAddress
    {
        public string Address { get; }
        public ulong RawAddess { get; }

        public MacAddress(string mac)
        {
            var match = Regex.Match(mac, @"^(..:){5}(..)$");
            if (!match.Success)
            {
                throw new Exception($"MacAddress address '{mac}' is not a valid mac address");
            }
            Address = mac;
            RawAddess = Convert.ToUInt64(Address.Replace(":", ""), 16);
        }

        public MacAddress(ulong mac)
        {
            Address = string.Join(":", BitConverter.GetBytes(mac)
                                                   .Reverse()
                                                   .Skip(2)
                                                   .Select(b => b.ToString("x2")));
            RawAddess = mac;
        }

        public override string ToString()
        {
            return Address;
        }
    }
}
