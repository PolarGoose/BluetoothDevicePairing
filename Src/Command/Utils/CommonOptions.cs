using CommandLine;

namespace BluetoothDevicePairing.Command.Utils
{
    internal class PairAndUnpairDeviceOptions
    {
        [Option("mac", SetName = "mac",
            HelpText = "mac address of a bluetooth device. For example: 12:34:56:78:9A:BC")]
        public string Mac { get; set; }

        [Option("name", SetName = "name",
            HelpText = "name of a bluetooth device (case sensitive)")]
        public string DeviceName { get; set; }
    }
}
