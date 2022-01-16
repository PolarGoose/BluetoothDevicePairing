using System;
using System.Text.RegularExpressions;

namespace BluetoothDevicePairing.Bluetooth.Devices
{
    internal enum DeviceType
    {
        Bluetooth,
        BluetoothLE
    }

    internal sealed class Device
    {
        private readonly Windows.Devices.Enumeration.DeviceInformation info;
        private readonly Windows.Devices.Bluetooth.BluetoothDevice bluetoothDevice;
        private readonly Windows.Devices.Bluetooth.BluetoothLEDevice bluetoothLeDevice;

        public string Id => info.Id;
        public Windows.Devices.Enumeration.DeviceInformationPairing PairingInfo => info.Pairing;
        public bool IsPaired => info.Pairing.IsPaired;
        public DeviceMacAddress Mac { get; }
        public DeviceType Type { get; }
        public string Name => info.Name;
        public bool IsConnected
        {
            get
            {
                if (bluetoothDevice != null)
                {
                    return bluetoothDevice.ConnectionStatus == Windows.Devices.Bluetooth.BluetoothConnectionStatus.Connected;
                }
                else
                {
                    return bluetoothLeDevice.ConnectionStatus == Windows.Devices.Bluetooth.BluetoothConnectionStatus.Connected;
                }
            }
        }

        public Device(Windows.Devices.Enumeration.DeviceInformation info)
        {
            this.info = info;
            Mac = new DeviceMacAddress(info);
            Type = GetDeviceType(info);
            if (Type == DeviceType.Bluetooth)
            {
                bluetoothDevice = Windows.Devices.Bluetooth.BluetoothDevice.FromIdAsync(info.Id).GetAwaiter().GetResult();
            }
            else
            {
                bluetoothLeDevice = Windows.Devices.Bluetooth.BluetoothLEDevice.FromIdAsync(info.Id).GetAwaiter().GetResult();
            }
        }

        public override string ToString()
        {
            return $"name:'{Name}' mac:'{Mac}' type:'{Type}' Connected:'{IsConnected}' Paired:'{IsPaired}'";
        }

        private static DeviceType GetDeviceType(Windows.Devices.Enumeration.DeviceInformation device)
        {
            var match = Regex.Match(device.Id, @"(^\w*)(#)");
            if (!match.Success)
            {
                throw new Exception($"Failed to extract the device type from the string '{device.Id}'");
            }

            var type = match.Groups[1].Value;
            switch (type)
            {
                case "Bluetooth":
                    return DeviceType.Bluetooth;
                case "BluetoothLE":
                    return DeviceType.BluetoothLE;
                default:
                    throw new Exception($"Wrong device type '{type}' extracted from '{device.Id}'");
            }
        }
    }
}
