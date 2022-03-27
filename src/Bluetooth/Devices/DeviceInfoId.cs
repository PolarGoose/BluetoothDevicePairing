using System;
using System.Text.RegularExpressions;
using BluetoothDevicePairing.Bluetooth.Adapters;

namespace BluetoothDevicePairing.Bluetooth.Devices;

internal sealed class DeviceInfoId
{
    public DeviceType DeviceType { get; }
    public DeviceMacAddress DeviceMac { get; }
    private AdapterMacAddress AdapterMac { get; }

    public DeviceInfoId(Windows.Devices.Enumeration.DeviceInformation info)
    {
        var match = Regex.Match(info.Id, @"(^\w+)#(?<Type>Bluetooth|BluetoothLE)(?<AdapterMac>(..:){5}(..))-(?<DeviceMac>(..:){5}(..))$");
        if (!match.Success)
        {
            throw new Exception($"Failed to parse DeviceInformation.Id '{info.Id}'");
        }

        DeviceType = match.Groups["Type"].Value == "Bluetooth" ? DeviceType.Bluetooth : DeviceType.BluetoothLE;
        AdapterMac = new AdapterMacAddress(match.Groups["AdapterMac"].Value);
        DeviceMac = new DeviceMacAddress(match.Groups["DeviceMac"].Value);
    }
}
