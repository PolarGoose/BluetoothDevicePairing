using System;
using Windows.Devices.Enumeration;

namespace BluetoothDevicePairing.Bluetooth
{
    internal sealed class DevicePairer
    {
        public static void PairDevice(Device device, string pin)
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
            Pair(device.Info, pin);
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

        private static void Pair(DeviceInformation device, string pin)
        {
            device.Pairing.Custom.PairingRequested += (s, a) => PairingRequestedHandler(s, a, pin);

            // DeviceInformation.Pairing.PairAsync function doesn't work for non UWP applications. Thus, DeviceInformation.Pairing.Custom.PairAsync is used.
            // https://stackoverflow.com/questions/45191412/deviceinformation-pairasync-not-working-in-wpf

            // DevicePairingKinds.DisplayPin option conflicts with DevicePairingKinds.ProvidePin: I used "Bluetooth Module HC 05" to test pairing with PIN code.
            // This device requires pin code "1234" to be paired. When both DevicePairingKinds.DisplayPin and DevicePairingKinds.ProvidePin flags were used in PairAsync function,
            // the PairingRequestedHandler was called with PairingKind equal to DevicePairingKinds.DisplayPin instead of DevicePairingKinds.ProvidePin, which made pairing fail.
            // Therefore, I decided not to use DevicePairingKinds.DisplayPin flag.

            var res = device.Pairing.Custom
                .PairAsync(DevicePairingKinds.ConfirmOnly | DevicePairingKinds.ProvidePin | DevicePairingKinds.ConfirmPinMatch, DevicePairingProtectionLevel.None)
                .GetAwaiter().GetResult().Status;
            if (res != DevicePairingResultStatus.Paired)
            {
                throw new Exception($"Failed to pair device. Status = {res}");
            }
        }

        private static void PairingRequestedHandler(DeviceInformationCustomPairing sender, DevicePairingRequestedEventArgs args, string pin)
        {
            switch (args.PairingKind)
            {
                case DevicePairingKinds.ConfirmOnly:
                    Console.WriteLine("Pairing mode: ConfirmOnly");
                    args.Accept();
                    return;

                case DevicePairingKinds.ProvidePin:
                    Console.WriteLine("Pairing mode: ProvidePin");
                    Console.WriteLine($"Pin is requested by the device. Using '{pin}' as a pin code");
                    args.Accept(pin);
                    return;

                case DevicePairingKinds.ConfirmPinMatch:
                    Console.WriteLine("Pairing mode: ConfirmPinMatch");
                    Console.WriteLine($"The device's pin code: '{args.Pin}'");
                    Console.WriteLine("Waiting for the target device to accept the pairing (you probably need to follow the instructions on the target device's screen)");
                    args.Accept();
                    return;
            }

            Console.WriteLine($"Unexpected pairing type: {args.PairingKind}");
            throw new Exception();
        }
    }
}
