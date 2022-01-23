using System;

namespace BluetoothDevicePairing.Bluetooth.Devices
{
    internal static class DeviceUnPairer
    {
        public static void UnpairDevice(Device device)
        {
            Console.WriteLine($"Request to unpair device \"{device}\"");

            if (device.ConnectionStatus == ConnectionStatus.NotPaired)
            {
                throw new Exception("Device is not paired");
            }

            var res = device.PairingInfo.UnpairAsync().GetAwaiter().GetResult().Status;
            if (res != Windows.Devices.Enumeration.DeviceUnpairingResultStatus.Unpaired)
            {
                throw new Exception($"Failed to unpair the device. Status = {res}");
            }

            Console.WriteLine("Device has been successfully unpaired");
        }
    }
}
