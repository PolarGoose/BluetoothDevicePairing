using System;
using BluetoothDevicePairing.Command;
using CommandLine;

namespace BluetoothDevicePairing
{
    internal sealed class Program
    {
        private static void ParseCommandLineAndExecuteActions(string[] args)
        {
            Parser.Default.ParseArguments<PairDeviceOptions, DiscoverDevicesOptions, UnpairDeviceOptions>(args)
                .WithParsed<PairDeviceOptions>(PairDevice.Execute)
                .WithParsed<UnpairDeviceOptions>(UnPairDevice.Execute)
                .WithParsed<DiscoverDevicesOptions>(DiscoverDevices.Execute);
        }

        private static int Main(string[] args)
        {
            try
            {
                ParseCommandLineAndExecuteActions(args);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed: {ex.Message}");
                return 1;
            }

            return 0;
        }
    }
}
