# BluetoothDevicePairing
Console utility to discover and pair/connect Bluetooth and Bluetooth LE devices.<br>
The main reason to create this utility was to be able to pair Bluetooth devices from console in the same way it is done by built in into Windows "Bluetooth & other devices" dialog.

# System requirements
Windows 10 1809 (10.0.17763) 64bit or higher<br>

# How to use
* Download and unpack the latest [release](https://github.com/PolarGoose/BluetoothDevicePairing/releases).
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
* Run `.github/workflows/build.ps1` to build a release (to run this script `git.exe` should be in your PATH)
