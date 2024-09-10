using System;

namespace BluetoothDevicePairing.Utils;

public sealed class AppException: ApplicationException
{
    public AppException() { }

    public AppException(string message): base(message) { }
}
