using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace BluetoothDevicePairing.Bluetooth;

internal class MacAddress
{
    private string Address { get; }
    public ulong RawAddress { get; }

    protected MacAddress(string mac)
    {
        var match = Regex.Match(mac, @"^(..:){5}(..)$");
        if (!match.Success)
        {
            throw new Exception($"MacAddress address '{mac}' is not a valid mac address");
        }
        Address = mac;
        RawAddress = Convert.ToUInt64(Address.Replace(":", ""), 16);
    }

    protected MacAddress(ulong mac)
    {
        Address = string.Join(":", BitConverter.GetBytes(mac)
                                               .Reverse()
                                               .Skip(2)
                                               .Select(b => b.ToString("x2")));
        RawAddress = mac;
    }

    public override string ToString()
    {
        return Address;
    }
}
