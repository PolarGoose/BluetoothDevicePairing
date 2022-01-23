using BluetoothDevicePairing.Commands;
using CommandLine;
using System;

namespace BluetoothDevicePairing
{
    internal static class Program
    {
        private static void ParseCommandLineAndExecuteActions(string[] args)
        {
            Parser.Default.ParseArguments<DiscoverDevicesOptions,
                                          PairDeviceByMacOptions,
                                          PairDeviceByNameOptions,
                                          UnpairDeviceByMacOptions,
                                          UnpairDeviceByNameOptions,
                                          ListAdaptersOptions>(args)
                  .WithParsed<DiscoverDevicesOptions>(DiscoverDevices.Execute)
                  .WithParsed<PairDeviceByMacOptions>(PairDeviceByMac.Execute)
                  .WithParsed<PairDeviceByNameOptions>(PairDeviceByName.Execute)
                  .WithParsed<UnpairDeviceByMacOptions>(UnPairDeviceByMac.Execute)
                  .WithParsed<UnpairDeviceByNameOptions>(UnPairDeviceByName.Execute)
                  .WithParsed<ListAdaptersOptions>(ListAdapters.Execute);
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
                return -1;
            }
        }
    }
}
