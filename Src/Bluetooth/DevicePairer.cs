using System;
using Windows.Devices.Enumeration;

namespace BluetoothDevicePairing.Bluetooth
{
    internal sealed class DevicePairer
    {
        public static void PairDevice(Device device)
        {
            Console.WriteLine($"Request to pair device \"{device}\"");

            if (device.IsConnected)
            {
                throw new Exception("Device is already connected, no need to pair");
            }

            if (device.IsPaired)
            {
                Console.WriteLine("Device is already paired, unpair it first");
                Unpair(device.Info);
            }

            Console.WriteLine("Start pairing");
            Pair(device.Info);
            Console.WriteLine("Device has been successfully paired");
        }

        public static void UnpairDevice(Device device)
        {
            Console.WriteLine($"Request to unpair device \"{device}\"");

            if (!device.IsPaired)
            {
                throw new Exception("Device is not paired, no need to unpair");
            }

            Unpair(device.Info);
            Console.WriteLine("Device has been successfully unpaired");
        }

        private static void Unpair(DeviceInformation device)
        {
            var res = device.Pairing.UnpairAsync().GetAwaiter().GetResult().Status;
            if (res != DeviceUnpairingResultStatus.Unpaired)
            {
                throw new Exception($"Failed to unpair the device. Status = {res}");
            }
        }

        private static void Pair(DeviceInformation device)
        {
            device.Pairing.Custom.PairingRequested += (sender, args) => { args.Accept(); };

            var res = device.Pairing.Custom.PairAsync(DevicePairingKinds.ConfirmOnly, DevicePairingProtectionLevel.None)
                .GetAwaiter().GetResult().Status;
            if (res != DevicePairingResultStatus.Paired)
            {
                throw new Exception($"Failed to pair device. Status = {res}");
            }
        }
    }
}
