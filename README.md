# BluetoothDevicePairing
Console utility to discover and pair/connect Bluetooth and Bluetooth LE devices.<br>
The main reason to create this utility was to be able to pair Bluetooth devices from console in the same way it is done by built in into Windows "Bluetooth & other devices" dialog.

# System requirements
Windows 10 x64.<br>
If you want to run a non self contained version of this application you need to have [.NET Core 3.1](https://dotnet.microsoft.com/download) or higher installed.

# How to use
* Download and unpack the latest release. The application has two versions:
    * `BluetoothDevicePairing.zip` - small executable which requires `.NET Core 3.1` or higher to be installed on the system.
    * `BluetoothDevicePairing_selfContained.zip` - self contained executable which doesn't requre `.Net Core` runtime to be installed.
* Run `BluetoothDevicePairing.exe --help` and `BluetoothDevicePairing.exe <command> --help` to get usage information and see the `Examples` section bellow.

# Examples
* Discover devices: `BluetoothDevicePairing.exe discover`
* Pair and connect to a device using its mac address: `BluetoothDevicePairing.exe pair --mac 12:34:56:78:9A:BC`
* Pair and connect to a device using its name: `BluetoothDevicePairing.exe pair --name "name of device"`
* Unpair a device using its mac address: `BluetoothDevicePairing.exe unpair --mac 12:34:56:78:9A:BC`
* Unpair a device using its name: `BluetoothDevicePairing.exe unpair --name "name of device"`

# Tips and tricks
* Bluetooth LE devices use mac address randomisation, therefore it is not reliable to pair them using mac address. Use pairing by name instead.

# Build
* Use `Visual Studio 2019` to open the solution file and work with the code
* Run `github/workflows/build.ps1` to build a release (to run this script `git.exe` should be in your PATH)
