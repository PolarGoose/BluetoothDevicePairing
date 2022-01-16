using CommandLine;

namespace BluetoothDevicePairing.Commands.Utils
{
    internal class PairAndUnpairDeviceOptions
    {
        [Option("type",
                Required = true,
                HelpText = "(case sensitive) type of a bluetooth device. Possible values: \"Bluetooth\", \"BluetoothLE\"")]
        public Bluetooth.Devices.DeviceType DeviceType { get; set; }
    }

    internal class PairAndUnpairDeviceByMacOptions : PairAndUnpairDeviceOptions
    {
        [Option("mac",
                Required = true,
                HelpText = "mac address of a bluetooth device. For example: 12:34:56:78:9A:BC")]
        public string Mac { get; set; }
    }

    internal class PairAndUnpairDeviceByNameOptions : PairAndUnpairDeviceOptions
    {
        [Option("discovery-time",
                Default = 10,
                HelpText = "how long to search for devices. Units: seconds")]
        public int DiscoveryTime { get; set; }

        [Option("name",
                Required = true,
                HelpText = "(case sensitive) name of a bluetooth device")]
        public string DeviceName { get; set; }
    }
}
