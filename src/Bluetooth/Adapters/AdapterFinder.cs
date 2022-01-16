using System;
using System.Collections.Generic;
using System.Linq;

namespace BluetoothDevicePairing.Bluetooth.Adapter
{
    internal static class AdapterFinder
    {
        public static Adapter FindDefaultAdapter()
        {
            return FindBluetoothAdapters().Where(adapter => adapter.IsDefault).Single();
        }

        public static IEnumerable<Adapter> FindBluetoothAdapters()
        {
            var macOfDefaultAdapter = GetMacAddressOfDefaultAdapter();
            return Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(Windows.Devices.Bluetooth.BluetoothAdapter.GetDeviceSelector()).GetAwaiter().GetResult()
                                                                .Select(info => new Adapter(info, macOfDefaultAdapter))
                                                                .ToList();
        }

        private static AdapterMacAddress GetMacAddressOfDefaultAdapter()
        {
            var defaultAdapter = GetDefaultAdapter();
            return new(defaultAdapter);
        }

        private static Windows.Devices.Bluetooth.BluetoothAdapter GetDefaultAdapter()
        {
            return Windows.Devices.Bluetooth.BluetoothAdapter.GetDefaultAsync().GetAwaiter().GetResult();
        }
    }
}
