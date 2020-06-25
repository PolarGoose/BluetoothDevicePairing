using Windows.Devices.Bluetooth;

namespace BluetoothDevicePairing.Bluetooth
{
    internal sealed class AsqFilter
    {
		public string Query { get; }
    
        private AsqFilter(string query)
        {
            Query = query;
        }

        public override string ToString()
        {
            return Query;
        }
        
        public static AsqFilter BluetoothDevicesFilter()
        {
            var paired = PairedBluetoothDevicesFilter();
            var nonPaired = NonPairedBluetoothDevicesFilter();
            return new AsqFilter($"({paired}) OR ({nonPaired})");
        }

        public static AsqFilter PairedBluetoothDevicesFilter()
        {
            var bPaired = BluetoothDevice.GetDeviceSelectorFromPairingState(true);
            var blePaired = BluetoothLEDevice.GetDeviceSelectorFromPairingState(true);
            return new AsqFilter($"({bPaired}) OR ({blePaired})");
        }

        public static AsqFilter NonPairedBluetoothDevicesFilter()
        {
            var bNonPaired = BluetoothDevice.GetDeviceSelectorFromPairingState(false);
            var bleNonPaired = BluetoothLEDevice.GetDeviceSelectorFromPairingState(false);
            return new AsqFilter($"({bNonPaired}) OR ({bleNonPaired})");
        }
    }
}
