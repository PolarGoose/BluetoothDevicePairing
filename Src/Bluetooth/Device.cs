using System;
using System.Text.RegularExpressions;
using Windows.Devices.Bluetooth;
using Windows.Devices.Enumeration;

namespace BluetoothDevicePairing.Bluetooth
{
    internal enum DeviceType
    {
        Bluetooth,
        BluetoothLe
    }

    internal sealed class Device
    {
        public Device(DeviceInformation info)
        {
            Info = info;
            Mac = new MacAddress(info);
            Type = GetDeviceType(info);
        }

        public DeviceInformation Info { get; }
        public bool IsPaired => Info.Pairing.IsPaired;
        public MacAddress Mac { get; }
        public DeviceType Type { get; }
        public string Name => Info.Name;

        public bool IsConnected
        {
            get
            {
                switch (Type)
                {
                    case DeviceType.Bluetooth:
                        var b = BluetoothDevice.FromIdAsync(Info.Id).GetAwaiter().GetResult();
                        return b.ConnectionStatus == BluetoothConnectionStatus.Connected;
                    case DeviceType.BluetoothLe:
                        var ble = BluetoothLEDevice.FromIdAsync(Info.Id).GetAwaiter().GetResult();
                        return ble.ConnectionStatus == BluetoothConnectionStatus.Connected;
                }

                throw new Exception($"Unknown device type '{Type}'");
            }
        }

        public override string ToString()
        {
            return $"name:'{Name}' mac:'{Mac}' type:'{Type}'";
        }

        private static DeviceType GetDeviceType(DeviceInformation device)
        {
            var match = Regex.Match(device.Id, @"(^\w*)(#)");
            if (!match.Success) throw new Exception($"Failed to extract the device type from the string '{device.Id}'");

            var type = match.Groups[1].Value;
            switch (type)
            {
                case "Bluetooth":
                    return DeviceType.Bluetooth;
                case "BluetoothLE":
                    return DeviceType.BluetoothLe;
                default:
                    throw new Exception($"Wrong device type '{type}' extracted from '{device.Id}'");
            }
        }
    }
}
