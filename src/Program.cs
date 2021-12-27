using BluetoothDevicePairing.Command;
using BluetoothDevicePairing.Command.Utils;
using CommandLine;
using System;

namespace BluetoothDevicePairing
{
    internal static class Program
    {
        private static bool WaitOnError;

        private static void ParseCommandLineAndExecuteActions(string[] args)
        {
            Parser.Default.ParseArguments<PairDeviceOptions, DiscoverDevicesOptions, UnpairDeviceOptions>(args)
                .WithParsed<CommonOptions>(opts => WaitOnError = opts.WaitOnError)
                .WithParsed<PairDeviceOptions>(PairDevice.Execute)
                .WithParsed<UnpairDeviceOptions>(UnPairDevice.Execute)
                .WithParsed<DiscoverDevicesOptions>(DiscoverDevices.Execute);
        }

        private static int Main(string[] args)
        {
            try
            {
                ParseCommandLineAndExecuteActions(args);
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed: {ex.Message}");
                if (WaitOnError)
                {
                    Console.ReadLine();
                }

                return 1;
            }
        }
    }
}
