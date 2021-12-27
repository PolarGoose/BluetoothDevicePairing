namespace BluetoothDevicePairing.Command.Utils
{
    internal enum DeviceType
    {
        Bluetooth,
        BluetoothLE,
        Any
    }

    internal static class DeviceTypeExtensions
    {
        public static bool Equals(DeviceType type1, Bluetooth.DeviceType type2)
        {
            if (type1 == DeviceType.Any)
            {
                return true;
            }

            if (type1 == DeviceType.Bluetooth && type2 == Bluetooth.DeviceType.Bluetooth
                || type1 == DeviceType.BluetoothLE && type2 == Bluetooth.DeviceType.BluetoothLE)
            {
                return true;
            }

            return false;
        }
    }
}
