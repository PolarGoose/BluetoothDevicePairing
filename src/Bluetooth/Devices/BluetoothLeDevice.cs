using BluetoothDevicePairing.Utils;

namespace BluetoothDevicePairing.Bluetooth.Devices;

internal class BluetoothLeDevice : Device
{
    private readonly Windows.Devices.Bluetooth.BluetoothLEDevice device;
    protected override bool IsConnected => device.ConnectionStatus == Windows.Devices.Bluetooth.BluetoothConnectionStatus.Connected;

    private BluetoothLeDevice(Windows.Devices.Bluetooth.BluetoothLEDevice device) : base(device.DeviceInformation)
    {
        this.device = device;
    }

    public static BluetoothLeDevice FromDeviceInfo(Windows.Devices.Enumeration.DeviceInformation info)
    {
        return new BluetoothLeDevice(Windows.Devices.Bluetooth.BluetoothLEDevice.FromIdAsync(info.Id).GetAwaiter().GetResult());
    }

    public static BluetoothLeDevice FromMac(DeviceMacAddress mac)
    {
        var device = Windows.Devices.Bluetooth.BluetoothLEDevice.FromBluetoothAddressAsync(mac.RawAddress).GetAwaiter().GetResult();
        return device == null
            ? throw new AppException($"Can't create a BluetoothLE device from the provided mac address '{mac}'. Device with this mac address doesn't exist")
            : new BluetoothLeDevice(device);
    }
}
