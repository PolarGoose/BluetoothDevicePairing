using CommandLine;

namespace BluetoothDevicePairing.Command.Utils
{
    internal class PairAndUnpairDeviceOptions
    {
        [Option("mac", SetName = "mac",
            HelpText = "bluetooth mac address of the device. For example: 12:34:56:78:9A:BC")]
        public string Mac { get; set; }

        [Option("device-name", SetName = "device-name",
            HelpText = "bluetooth device name (case sensitive)")]
        public string DeviceName { get; set; }
    }
}
