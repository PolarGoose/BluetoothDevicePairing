using System;
using BluetoothDevicePairing.Bluetooth;
using BluetoothDevicePairing.Command.Utils;
using CommandLine;

namespace BluetoothDevicePairing.Command
{
    [Verb("discover", HelpText = "Discover devices")]
    internal sealed class DiscoverDevicesOptions : CommonOptions
    {
        [Option("discovery-time", Default = 10,
            HelpText = "how long to search for devices. Units: seconds")]
        public int DiscoveryTime { get; set; }
    }

    internal sealed class DiscoverDevices
    {
        public static void Execute(DiscoverDevicesOptions opts)
        {
            var devices = DeviceDiscoverer.DiscoverBluetoothDevices(opts.DiscoveryTime);
            Console.WriteLine("----------------------------------------------------------");
            foreach (var d in devices) PrintDevice(d);
            Console.WriteLine("----------------------------------------------------------");
        }

        private static void PrintDevice(Device d)
        {
            Console.WriteLine($"{GetType(d),2}|{d.Mac}|{GetPairedStatus(d),6}|{GetConnectionStatus(d),9}|{GetName(d)}");
        }

        private static string GetType(Device d)
        {
            return d.Type == DeviceType.BluetoothLe ? "LE" : "";
        }

        private static string GetPairedStatus(Device d)
        {
            return d.IsPaired ? "Paired" : "";
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
