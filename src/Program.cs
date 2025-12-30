using System;
using BluetoothDevicePairing.Commands;
using BluetoothDevicePairing.Utils;
using CommandLine;

static void ParseCommandLineAndExecuteActions(string[] args)
{
    _ = Parser.Default.ParseArguments<DiscoverDevicesOptions,
                                      PairDeviceByMacOptions,
                                      PairDeviceByNameOptions,
                                      UnpairDeviceByMacOptions,
                                      UnpairDeviceByNameOptions,
                                      ConnectBluetoothAudioDeviceByNameOptions,
                                      ConnectBluetoothAudioDeviceByMacOptions,
                                      DisconnectBluetoothAudioDeviceByNameOptions,
                                      DisconnectBluetoothAudioDeviceByMacOptions,
                                      ListAdaptersOptions>(args)
          .WithParsed<DiscoverDevicesOptions>(DiscoverDevices.Execute)
          .WithParsed<PairDeviceByMacOptions>(PairDeviceByMac.Execute)
          .WithParsed<PairDeviceByNameOptions>(PairDeviceByName.Execute)
          .WithParsed<UnpairDeviceByMacOptions>(UnPairDeviceByMac.Execute)
          .WithParsed<UnpairDeviceByNameOptions>(UnPairDeviceByName.Execute)
          .WithParsed<ConnectBluetoothAudioDeviceByNameOptions>(ConnectBluetoothAudioDeviceByName.Execute)
          .WithParsed<ConnectBluetoothAudioDeviceByMacOptions>(ConnectBluetoothAudioDeviceByMac.Execute)
          .WithParsed<DisconnectBluetoothAudioDeviceByNameOptions>(DisconnectBluetoothAudioDeviceByName.Execute)
          .WithParsed<DisconnectBluetoothAudioDeviceByMacOptions>(DisconnectBluetoothAudioDeviceByMac.Execute)
          .WithParsed<ListAdaptersOptions>(ListAdapters.Execute);
}

try
{
    ParseCommandLineAndExecuteActions(args);
    return 0;
}
catch (AppException ex)
{
    Console.WriteLine($"Failed: {ex.Message}");
    return -1;
}
catch (Exception ex)
{
    Console.WriteLine($"Unexpected failure: {ex}");
    return -1;
}
