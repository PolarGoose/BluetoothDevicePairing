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
                .WithParsed<PairDeviceOptions>(opts => new PairDevice().Execute(opts))
                .WithParsed<UnpairDeviceOptions>(opts => new UnPairDevice().Execute(opts))
                .WithParsed<DiscoverDevicesOptions>(opts => new DiscoverDevices().Execute(opts));
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
