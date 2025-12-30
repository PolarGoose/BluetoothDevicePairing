using BluetoothDevicePairing.Bluetooth.Devices;
using System.Linq;
using System;
using BluetoothDevicePairing.Utils;

namespace BluetoothDevicePairing.Commands.Utils;

internal static class AudioDeviceConnector
{
    public static void ConnectBluetoothAudioDevice(Device device)
    {
        Console.WriteLine($"Request to connect Bluetooth audio devices associated with [{device}]");

        if (device.ConnectionStatus == ConnectionStatus.NotPaired)
        {
            throw new AppException($"The device '{device.Name}' is not paired");
        }

        if (device.ConnectionStatus == ConnectionStatus.Connected)
        {
            throw new AppException($"The device '{device.Name}' is already connected");
        }

        if (!device.AssociatedAudioDevices.Any())
        {
            throw new AppException($"The device '{device.Name}' is not an audio device");
        }

        Console.WriteLine($"Connecting audio devices associated with '{device.Name}'");

        foreach (var audioDevice in device.AssociatedAudioDevices)
        {
            audioDevice.Connect();
        }
    }
}
