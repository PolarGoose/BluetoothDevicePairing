namespace BluetoothDevicePairing.Bluetooth.Devices
{
    internal enum ConnectionStatus
    {
        NotPaired,
        Paired,
        Connected
    }

    internal enum DeviceType
    {
        Bluetooth,
        BluetoothLE
    }

    internal abstract class Device
    {
        private readonly Windows.Devices.Enumeration.DeviceInformation info;
        protected abstract bool IsConnected { get; }

        public ConnectionStatus ConnectionStatus =>
            IsConnected
                ? ConnectionStatus.Connected
                : PairingInfo.IsPaired
                    ? ConnectionStatus.Paired
                    : ConnectionStatus.NotPaired;
        public Windows.Devices.Enumeration.DeviceInformationPairing PairingInfo => info.Pairing;
        public DeviceInfoId Id { get; }
        public string Name => info.Name;

        protected Device(Windows.Devices.Enumeration.DeviceInformation info)
        {
            this.info = info;
            Id = new DeviceInfoId(info);
        }

        public override string ToString()
        {
            return $"name:'{Name}' mac:'{Id.DeviceMac}' type:'{Id.DeviceType}' ConnectionStatus:'{ConnectionStatus}'";
        }
    }
}
