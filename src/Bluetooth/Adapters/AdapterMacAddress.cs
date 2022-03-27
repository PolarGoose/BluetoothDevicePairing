namespace BluetoothDevicePairing.Bluetooth.Adapters;

internal sealed class AdapterMacAddress : MacAddress
{
    public AdapterMacAddress(Windows.Devices.Bluetooth.BluetoothAdapter adapter) : base(adapter.BluetoothAddress)
    {
    }

    public AdapterMacAddress(string mac) : base(mac)
    {
    }
}
