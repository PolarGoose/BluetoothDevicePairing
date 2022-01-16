using System;
using System.Text.RegularExpressions;
using Windows.Devices.Enumeration;

namespace BluetoothDevicePairing.Bluetooth.Devices
{
    internal sealed class DeviceMacAddress : IEquatable<DeviceMacAddress>
    {
        public DeviceMacAddress(DeviceInformation device)
        {
            var match = Regex.Match(device.Id, @"(..:){5}(..)$");
            if (!match.Success)
            {
                throw new Exception($"Failed to extract mac address from the string '{device.Id}'");
            }
            Address = match.Value.ToUpper();
        }

        public DeviceMacAddress(string mac)
        {
            var match = Regex.Match(mac, @"^(..:){5}(..)$");
            if (!match.Success)
            {
                throw new Exception($"MacAddress address '{mac}' is not a valid mac address");
            }
            Address = mac;
        }

        public string Address { get; }

        public override string ToString()
        {
            return Address;
        }

        public bool Equals(DeviceMacAddress other)
        {
            if (other is null)
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return Address == other.Address;
        }

        public override bool Equals(object obj)
        {
            return ReferenceEquals(this, obj) || obj is DeviceMacAddress other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Address != null ? Address.GetHashCode() : 0;
        }
    }
}
