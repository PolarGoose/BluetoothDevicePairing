namespace BluetoothDevicePairing.Bluetooth.Devices.Utils;

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
        var bPaired = Windows.Devices.Bluetooth.BluetoothDevice.GetDeviceSelectorFromPairingState(true);
        var blePaired = Windows.Devices.Bluetooth.BluetoothLEDevice.GetDeviceSelectorFromPairingState(true);
        return new AsqFilter($"({bPaired}) OR ({blePaired})");
    }

    private static AsqFilter NonPairedBluetoothDevicesFilter()
    {
        var bNonPaired = Windows.Devices.Bluetooth.BluetoothDevice.GetDeviceSelectorFromPairingState(false);
        var bleNonPaired = Windows.Devices.Bluetooth.BluetoothLEDevice.GetDeviceSelectorFromPairingState(false);
        return new AsqFilter($"({bNonPaired}) OR ({bleNonPaired})");
    }
}
