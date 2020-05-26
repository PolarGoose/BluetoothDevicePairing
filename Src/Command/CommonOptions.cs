using CommandLine;

namespace BluetoothDevicePairing.Command
{
    internal class PairUnpairDeviceOptions
    {
        [Option("mac", SetName = "mac",
            HelpText = "bluetooth mac address of the device. For example: 12:34:56:78:91:21")]
        public string Mac { get; set; }

        [Option("device-name", SetName = "device-name",
            HelpText = "bluetooth device name (case sensitive)")]
        public string DeviceName { get; set; }
    }
}
