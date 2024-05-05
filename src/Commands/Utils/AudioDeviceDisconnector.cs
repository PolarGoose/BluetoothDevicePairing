using BluetoothDevicePairing.Bluetooth.Devices;
using System.Linq;
using System;

namespace BluetoothDevicePairing.Commands.Utils;

internal static class AudioDeviceDisconnector
{
    public static void DisconnectBluetoothAudioDevice(Device device)
    {
        Console.WriteLine($"Request to disconnect Bluetooth audio devices associated with [{device}]");

        if (device.ConnectionStatus != ConnectionStatus.Connected)
        {
            throw new Exception($"The device '{device.Name}' is not connected");
        }

        if (!device.AssociatedAudioDevices.Any())
        {
            throw new Exception($"The device '{device.Name}' is not an audio device");
        }

        Console.WriteLine($"Disconnecting audio devices associated with '{device.Name}'");

        foreach (var audioDevice in device.AssociatedAudioDevices)
        {
            audioDevice.Disconnect();
        }
    }
}
