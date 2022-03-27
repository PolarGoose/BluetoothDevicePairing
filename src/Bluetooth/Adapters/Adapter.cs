using System;

namespace BluetoothDevicePairing.Bluetooth.Adapters;

internal sealed class Adapter
{
    private readonly Windows.Devices.Enumeration.DeviceInformation adapterInfo;
    private readonly Windows.Devices.Radios.Radio radio;

    public Windows.Devices.Radios.RadioState State => radio.State;
    public string Name => adapterInfo.Name;
    public AdapterMacAddress MacAddress { get; }
    public bool IsDefault { get; }

    public Adapter(Windows.Devices.Enumeration.DeviceInformation bluetoothAdapterInfo,
                   AdapterMacAddress defaultAdapterMacAddress)
    {
        adapterInfo = bluetoothAdapterInfo;
        var adapterDevice = Windows.Devices.Bluetooth.BluetoothAdapter.FromIdAsync(bluetoothAdapterInfo.Id).GetAwaiter().GetResult();
        radio = adapterDevice.GetRadioAsync().GetAwaiter().GetResult();
        MacAddress = new AdapterMacAddress(adapterDevice);
        IsDefault = MacAddress.RawAddress == defaultAdapterMacAddress.RawAddress;
    }

    public override string ToString()
    {
        return $"name:'{Name}' mac:'{MacAddress}' state:'{State}'";
    }
}
