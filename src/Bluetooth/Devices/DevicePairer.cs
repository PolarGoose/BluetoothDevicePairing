using BluetoothDevicePairing.Utils;
using System;
using System.Linq;

namespace BluetoothDevicePairing.Bluetooth.Devices;

internal static class DevicePairer
{
    public static void PairDevice(Device device, string pinCode)
    {
        Console.WriteLine($"Request to pair device [{device}]");

        if (device.ConnectionStatus == ConnectionStatus.Connected)
        {
            throw new AppException("Device is already connected, no need to pair");
        }

        if (device.ConnectionStatus == ConnectionStatus.Paired)
        {
            if (!device.AssociatedAudioDevices.Any())
            {
                throw new AppException("Device is already paired");
            }

            Console.WriteLine("Device is already paired. Connecting associated audio devices");
            foreach (var audioDevice in device.AssociatedAudioDevices)
            {
                audioDevice.Connect();
            }
            return;
        }

        Console.WriteLine("Start pairing");
        Pair(device.PairingInfo, pinCode);
        Console.WriteLine("Device has been successfully paired");
    }

    private static void Pair(Windows.Devices.Enumeration.DeviceInformationPairing pairingInfo, string pinCode)
    {
        pairingInfo.Custom.PairingRequested += (_, a) => PairingRequestedHandler(a, pinCode);

        // DeviceInformation.Pairing.PairAsync function doesn't work for non UWP applications. Thus, DeviceInformation.Pairing.Custom.PairAsync is used.
        // https://stackoverflow.com/questions/45191412/deviceinformation-pairasync-not-working-in-wpf

        // DevicePairingKinds.DisplayPin option conflicts with DevicePairingKinds.ProvidePin: I used "Bluetooth Module HC 05" to test pairing with PIN code.
        // This device requires pin code "1234" to be paired. When both DevicePairingKinds.DisplayPin and DevicePairingKinds.ProvidePin flags were used in PairAsync function,
        // the PairingRequestedHandler was called with PairingKind equal to DevicePairingKinds.DisplayPin instead of DevicePairingKinds.ProvidePin, which made pairing fail.
        // Therefore, I decided not to use DevicePairingKinds.DisplayPin flag.

        var res = pairingInfo.Custom.PairAsync(Windows.Devices.Enumeration.DevicePairingKinds.ConfirmOnly |
                                               Windows.Devices.Enumeration.DevicePairingKinds.ProvidePin |
                                               Windows.Devices.Enumeration.DevicePairingKinds.ConfirmPinMatch,
                                               Windows.Devices.Enumeration.DevicePairingProtectionLevel.None)
                                    .GetAwaiter().GetResult().Status;
        if (res != Windows.Devices.Enumeration.DevicePairingResultStatus.Paired)
        {
            throw new AppException($"Failed to pair device. Status = {res}");
        }
    }

    private static void PairingRequestedHandler(Windows.Devices.Enumeration.DevicePairingRequestedEventArgs args,
                                                string pin)
    {
        switch (args.PairingKind)
        {
            case Windows.Devices.Enumeration.DevicePairingKinds.ConfirmOnly:
                Console.WriteLine("Pairing mode: ConfirmOnly");
                args.Accept();
                return;

            case Windows.Devices.Enumeration.DevicePairingKinds.ProvidePin:
                Console.WriteLine("Pairing mode: ProvidePin");
                Console.WriteLine($"Pin is requested by the device. Using '{pin}' as a pin code");
                args.Accept(pin);
                return;

            case Windows.Devices.Enumeration.DevicePairingKinds.ConfirmPinMatch:
                Console.WriteLine("Pairing mode: ConfirmPinMatch");
                Console.WriteLine($"The device's pin code: '{args.Pin}'");
                Console.WriteLine("Waiting for the target device to accept the pairing (you probably need to follow the instructions on the target device's screen)");
                args.Accept();
                return;

            default:
                Console.WriteLine($"Unexpected pairing type: {args.PairingKind}");
                throw new AppException();
        }
    }
}
