using BluetoothDevicePairing.Commands;
using BluetoothDevicePairing.Utils;
using CommandLine;
using CommandLine.Text;
using System.Reflection;

[assembly: AssemblyCopyright("")]
[assembly: AssemblyUsage("Console utility to discover and pair Bluetooth and Bluetooth Low Energy devices.")]

static int ParseCommandLineAndExecuteActions(string[] args)
{
    if (args.Length == 0)
    {
        args = ["--help"];
    }

    return Parser.Default.ParseArguments<DiscoverDevicesOptions,
                                         PairDeviceByMacOptions,
                                         PairDeviceByNameOptions,
                                         UnpairDeviceByMacOptions,
                                         UnpairDeviceByNameOptions,
                                         DisconnectBluetoothAudioDeviceByNameOptions,
                                         DisconnectBluetoothAudioDeviceByMacOptions,
                                         ListAdaptersOptions>(args)
          .WithParsed<DiscoverDevicesOptions>(DiscoverDevices.Execute)
          .WithParsed<PairDeviceByMacOptions>(PairDeviceByMac.Execute)
          .WithParsed<PairDeviceByNameOptions>(PairDeviceByName.Execute)
          .WithParsed<UnpairDeviceByMacOptions>(UnPairDeviceByMac.Execute)
          .WithParsed<UnpairDeviceByNameOptions>(UnPairDeviceByName.Execute)
          .WithParsed<DisconnectBluetoothAudioDeviceByNameOptions>(DisconnectBluetoothAudioDeviceByName.Execute)
          .WithParsed<DisconnectBluetoothAudioDeviceByMacOptions>(DisconnectBluetoothAudioDeviceByMac.Execute)
          .WithParsed<ListAdaptersOptions>(ListAdapters.Execute)
          .MapResult(
              _ => 0,
              errors => errors.All(error => error is HelpRequestedError
                  or HelpVerbRequestedError
                  or VersionRequestedError) ? 0 : -1);
}

try
{
    return ParseCommandLineAndExecuteActions(args);
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
