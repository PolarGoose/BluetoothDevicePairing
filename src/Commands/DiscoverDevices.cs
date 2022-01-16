using BluetoothDevicePairing.Bluetooth.Devices;
using CommandLine;
using System;
using System.Linq;

namespace BluetoothDevicePairing.Commands
{
    [Verb("discover",
          HelpText = "Discover devices. Prints a table with the following fields:\n" +
                     "|Device type|Mac address|Pairing status|Device name|")]
    internal sealed class DiscoverDevicesOptions
    {
        [Option("discovery-time",
                Default = 10,
                HelpText = "how long to search for devices. Units: seconds")]
        public int DiscoveryTime { get; set; }
    }

    internal static class DiscoverDevices
    {
        public static void Execute(DiscoverDevicesOptions opts)
        {
            var devices = DeviceDiscoverer.DiscoverBluetoothDevices(opts.DiscoveryTime).OrderBy(d => d.Name);
            Console.WriteLine("----------------------------------------------------------");
            foreach (var d in devices)
            {
                PrintDevice(d);
            }
            Console.WriteLine("----------------------------------------------------------");
        }

        private static void PrintDevice(Device d)
        {
            Console.WriteLine($"{GetType(d),2}|{d.Mac}|{GetStatus(d),-9}|{GetName(d),-40}|");
        }

        private static string GetType(Device d)
        {
            return d.Type == DeviceType.BluetoothLE ? "LE" : "";
        }

        private static string GetStatus(Device d)
        {
            if (d.IsConnected)
            {
                return "Connected";
            }
            if (d.IsPaired)
            {
                return "Paired";
            }
            return "";
        }

        private static string GetName(Device d)
        {
            return d.Name == "" ? "<Unknown>" : d.Name;
        }

        private static string GetConnectionStatus(Device d)
        {
            return d.IsConnected ? "Connected" : "";
        }
    }
}
