using System;
using System.Text.RegularExpressions;

namespace BluetoothDevicePairing.Command
{
    internal sealed class Utils
    {
        public static void ValidateMac(string mac)
        {
            var match = Regex.Match(mac, @"^(..:){5}(..)$");
            if (!match.Success) throw new Exception($"Mac address {mac} is not a valid mac address");
        }
    }
}
