using BluetoothDevicePairing.Bluetooth.Devices;
using CommandLine;
using System.Text.Json;

namespace BluetoothDevicePairing.Commands;

[Verb("discover",
      HelpText = "Discover devices. By default prints a table with the following fields:\n" +
                 "|Device type|Mac address|Pairing status|Device name|\n" +
                 "Use --json for machine-readable JSON output.")]
internal sealed class DiscoverDevicesOptions
{
    [Option("discovery-time",
            Default = 10,
            HelpText = "how long to search for devices. Units: seconds")]
    public int DiscoveryTime { get; set; }

    [Option("json",
            Default = false,
            HelpText = "Output results in JSON format instead of a table")]
    public bool Json { get; set; }
}

internal static class DiscoverDevices
{
    public static void Execute(DiscoverDevicesOptions opts)
    {
        var devices = DeviceDiscoverer.DiscoverBluetoothDevices(new DiscoveryTime(opts.DiscoveryTime)).OrderBy(d => d.Name);

        if (opts.Json)
        {
            var jsonData = devices.Select(d => new
            {
                deviceType = d.Id.DeviceType.ToString(),
                macAddress = d.Id.DeviceMac.ToString(),
                pairingStatus = d.ConnectionStatus.ToString(),
                name = d.Name,
                associatedAudioDevices = d.AssociatedAudioDevices.Select(a => new
                {
                    name = a.Name,
                    isConnected = a.IsConnected
                }).ToArray()
            });
            Console.WriteLine(JsonSerializer.Serialize(jsonData, new JsonSerializerOptions { WriteIndented = true }));
            return;
        }

        Console.WriteLine(new string('-', 73));
        foreach (var d in devices)
        {
            PrintDevice(d);
        }
        Console.WriteLine(new string('-', 73));
    }

    private static void PrintDevice(Device d)
    {
        Console.WriteLine($"|{GetType(d),2}|{d.Id.DeviceMac}|{GetConnectionStatus(d),-9}|{GetName(d),-40}|");

        if (!d.AssociatedAudioDevices.Any())
        {
            return;
        }

        Console.WriteLine($"|  |                 |         |  Associated audio devices:             |");
        foreach (var audioDevice in d.AssociatedAudioDevices)
        {
            Console.WriteLine($"|  |                 |         |  * {audioDevice.Name,-36}|");
        }
    }

    private static string GetType(Device d)
    {
        return d.Id.DeviceType == DeviceType.BluetoothLE ? "LE" : "";
    }

    private static string GetName(Device d)
    {
        return d.Name == "" ? "<Unknown>" : d.Name;
    }

    private static string GetConnectionStatus(Device d)
    {
        return d.ConnectionStatus == ConnectionStatus.NotPaired ? "" : d.ConnectionStatus.ToString();
    }
}
