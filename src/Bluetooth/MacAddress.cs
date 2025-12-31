using BluetoothDevicePairing.Utils;
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
            throw new AppException($"MacAddress address '{mac}' is not a valid mac address");
        }
        Address = mac;
        RawAddress = Convert.ToUInt64(Address.Replace(":", ""), 16);
    }

    protected MacAddress(ulong mac)
    {
        var macBytes = BitConverter.GetBytes(mac);
        Array.Reverse(macBytes);
        Address = string.Join(":", macBytes.Skip(2).Select(b => b.ToString("x2")));
        RawAddress = mac;
    }

    public override string ToString()
    {
        return Address;
    }
}
