using BluetoothDevicePairing.Bluetooth.Adapter;
using CommandLine;
using System;

namespace BluetoothDevicePairing.Commands
{
    [Verb("list-adapters",
          HelpText = "Lists bluetooth adapters. Prints a table with the following fields:\n" +
                     "|Is default|Radio mac address|Name|State|")]
    internal sealed class ListAdaptersOptions
    {
    }

    internal static class ListAdapters
    {
        public static void Execute(ListAdaptersOptions opts)
        {
            var adapters = AdapterFinder.FindBluetoothAdapters();
            Console.WriteLine(new string('-', 71));
            foreach (var a in adapters)
            {
                PrintAdapter(a);
            }
            Console.WriteLine(new string('-', 71));
        }

        private static void PrintAdapter(Adapter a)
        {
            Console.WriteLine($"|{GetIsDefailt(a),1}|{a.MacAddress}|{a.Name,-40}|{a.State,-8}|");
        }

        private static string GetIsDefailt(Adapter a)
        {
            return a.IsDefault ? "*" : "";
        }
    }
}
