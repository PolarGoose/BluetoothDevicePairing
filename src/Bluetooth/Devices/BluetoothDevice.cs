using System;

namespace BluetoothDevicePairing.Bluetooth.Devices
{
    internal sealed class BluetoothDevice : Device
    {
        public readonly Windows.Devices.Bluetooth.BluetoothDevice device;
        protected override bool IsConnected => device.ConnectionStatus == Windows.Devices.Bluetooth.BluetoothConnectionStatus.Connected;

        private BluetoothDevice(Windows.Devices.Bluetooth.BluetoothDevice device) : base(device.DeviceInformation)
        {
            this.device = device;
        }

        public static BluetoothDevice FromDeviceInfo(Windows.Devices.Enumeration.DeviceInformation info)
        {
            return new BluetoothDevice(Windows.Devices.Bluetooth.BluetoothDevice.FromIdAsync(info.Id).GetAwaiter().GetResult());
        }

        public static BluetoothDevice FromMac(DeviceMacAddress mac)
        {
            return new BluetoothDevice(Windows.Devices.Bluetooth.BluetoothDevice.FromBluetoothAddressAsync(mac.RawAddess).GetAwaiter().GetResult());
        }
    }
}
