# BluetoothDevicePairing
Console utility to discover and pair or connect to Bluetooth and Bluetooth Low Energy devices.

# System requirements
Windows 10 1809 (10.0.17763) or higher

# How to use
* Download and unpack the latest [release](https://github.com/PolarGoose/BluetoothDevicePairing/releases).
* Run `BluetoothDevicePairing.exe --help` and `BluetoothDevicePairing.exe <command> --help` to get usage information and check the `Examples` section bellow.

# Examples
* Discover devices:
```
BluetoothDevicePairing.exe discover
```
* Pair and connect to a device using its mac address:
```
BluetoothDevicePairing.exe pair-by-mac --mac 12:34:56:78:9A:BC --type Bluetooth
```
* Pair and connect to a device using its name:
```
BluetoothDevicePairing.exe pair-by-name --name "MX Ergo" --type BluetoothLE
```
* Pair and connect to a device using its name and pin code:
```
BluetoothDevicePairing.exe pair-by-name --name "Device name" --type BluetoothLE --pin 1234
```
* Pair and connect to a device using its mac and pin code:
```
BluetoothDevicePairing.exe pair-by-mac --mac 12:34:56:78:9A:BC --type Bluetooth --pin 1234
```
* Unpair a device using its mac address:
```
BluetoothDevicePairing.exe unpair-by-mac --mac 12:34:56:78:9A:BC --type Bluetooth
```
* Unpair a device using its name:
```
BluetoothDevicePairing.exe unpair-by-name --name "MX Ergo" --type BluetoothLE
```

# How it works
The program uses [Windows.Devices.Enumeration API](https://docs.microsoft.com/en-us/uwp/api/Windows.Devices.Enumeration?redirectedfrom=MSDN&view=winrt-22000) to work with Bluetooth.

## Device pairing by mac
The utility gets the default bluetooth adapter and generates the bluetooth device id using combination of bluetooth type, adapter mac address and device's mac address. Using this id, it is possible to request to pair or unpair the device.

## Device pairing by name
The utility discovers all available devices (for unpairing only paired devices are checked) and tries to find a device with the required name. After that pairing or unpairing is requested for found device. The command will fail if there are several devices with the same name.

# Return values
In case of failure the command returns value `-1`. In case of success the `0` is returned.

# Build
* Use `Visual Studio 2022` to open the solution file and work with the code
* Run `.github/workflows/build.ps1` to build a release (to run this script `git.exe` should be in your PATH)

# References
* [Windows.Devices.Enumeration API usage examples](https://github.com/microsoft/Windows-universal-samples/tree/master/Samples/DeviceEnumerationAndPairing)
