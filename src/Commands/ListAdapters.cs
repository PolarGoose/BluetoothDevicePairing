using BluetoothDevicePairing.Bluetooth.Adapters;
using CommandLine;
using Newtonsoft.Json;

namespace BluetoothDevicePairing.Commands;

[Verb("list-adapters",
      HelpText = "Lists bluetooth adapters. By default prints a table with the following fields:\n" +
                 "|Is default|Radio mac address|Name|State|\n" +
                 "Use --json for machine-readable JSON output.")]
internal sealed class ListAdaptersOptions
{
    [Option("json",
            Default = false,
            HelpText = "Output results in JSON format instead of a table")]
    public bool Json { get; set; }
}

internal static class ListAdapters
{
    public static void Execute(ListAdaptersOptions opts)
    {
        var adapters = AdapterFinder.FindBluetoothAdapters();

        if (opts.Json)
        {
            var jsonData = adapters.Select(a => new
            {
                isDefault = a.IsDefault,
                macAddress = a.MacAddress.ToString(),
                name = a.Name,
                state = a.State.ToString()
            });
            Console.WriteLine(JsonConvert.SerializeObject(jsonData, Formatting.Indented));
            return;
        }

        Console.WriteLine(new string('-', 71));
        foreach (var a in adapters)
        {
            PrintAdapter(a);
        }
        Console.WriteLine(new string('-', 71));
    }

    private static void PrintAdapter(Adapter a)
    {
        Console.WriteLine($"|{IsDefault(a),1}|{a.MacAddress}|{a.Name,-40}|{a.State,-8}|");
    }

    private static string IsDefault(Adapter a)
    {
        return a.IsDefault ? "*" : "";
    }
}
