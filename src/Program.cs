using BluetoothDevicePairing.Commands;
using CommandLine;
using System;

static void ParseCommandLineAndExecuteActions(string[] args)
{
    _ = Parser.Default.ParseArguments<DiscoverDevicesOptions,
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
