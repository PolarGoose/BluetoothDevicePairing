using System;
using System.Linq;
using Windows.Devices.Bluetooth;

namespace BluetoothDevicePairing.Bluetooth.Adapter
{
    internal sealed class AdapterMacAddress : IEquatable<AdapterMacAddress>
    {
        public string Address { get; }

        public AdapterMacAddress(BluetoothAdapter adapter)
        {
            Address = string.Join(":", BitConverter.GetBytes(adapter.BluetoothAddress)
                                                   .Reverse()
                                                   .Skip(2)
                                                   .Select(b => b.ToString("x2")));
        }

        public override string ToString()
        {
            return Address;
        }

        public bool Equals(AdapterMacAddress other)
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
            return ReferenceEquals(this, obj) || obj is AdapterMacAddress other && Equals(other);
        }

        public override int GetHashCode()
        {
            return Address != null ? Address.GetHashCode() : 0;
        }
    }
}
