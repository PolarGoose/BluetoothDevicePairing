using Windows.Devices.Bluetooth;

namespace BluetoothDevicePairing.Bluetooth
{
    internal sealed class AsqFilter
    {
        private AsqFilter(string query)
        {
            Query = query;
        }

        public string Query { get; }

        public static AsqFilter BluetoothDevicesFilter()
        {
            var bPaired = BluetoothDevice.GetDeviceSelectorFromPairingState(true);
            var bNonPaired = BluetoothDevice.GetDeviceSelectorFromPairingState(false);
            var blePaired = BluetoothLEDevice.GetDeviceSelectorFromPairingState(true);
            var bleNonPaired = BluetoothLEDevice.GetDeviceSelectorFromPairingState(false);
            return new AsqFilter($"({bPaired}) OR ({bNonPaired}) OR ({blePaired}) OR ({bleNonPaired})");
        }
    }
}
