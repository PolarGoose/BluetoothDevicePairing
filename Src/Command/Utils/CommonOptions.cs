using CommandLine;

namespace BluetoothDevicePairing.Command.Utils
{
    internal class CommonOptions
    {
        [Option("wait-on-error", Default = false,
            HelpText = "wait for an user input in case of an error. This flag can be useful if you want a console to not disappear to see an error message in case a command failed")]
        public bool WaitOnError { get; set; }
    }

    internal class PairAndUnpairDeviceOptions : CommonOptions
    {
        [Option("mac", SetName = "mac",
            HelpText = "mac address of a bluetooth device. For example: 12:34:56:78:9A:BC")]
        public string Mac { get; set; }

        [Option("name", SetName = "name",
            HelpText = "name of a bluetooth device (case sensitive)")]
        public string DeviceName { get; set; }
    }
}
