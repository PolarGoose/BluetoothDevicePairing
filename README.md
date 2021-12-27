# BluetoothDevicePairing
Console utility to discover and pair/connect Bluetooth and Bluetooth LE devices.

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
BluetoothDevicePairing.exe pair --mac 12:34:56:78:9A:BC
```
* Pair and connect to a device using its name:
```
BluetoothDevicePairing.exe pair --name "name of device"
```
* Pair and connect to a device using its name/mac and device type:
```
BluetoothDevicePairing.exe pair --name "name of device" --type BluetoothLE
```
* Pair and connect to a device using its name/mac and pin code:
```
BluetoothDevicePairing.exe pair --mac 12:34:56:78:9A:BC --pin 1234
```
* Unpair a device using its mac address:
```
BluetoothDevicePairing.exe unpair --mac 12:34:56:78:9A:BC
```
* Unpair a device using its name:
```
BluetoothDevicePairing.exe unpair --name "name of device"
```
* Unpair a device using its name/mac and device type:
```
BluetoothDevicePairing.exe unpair --mac 12:34:56:78:9A:BC --type Bluetooth
```

# How it works
The program uses [Windows.Devices.Enumeration API](https://docs.microsoft.com/en-us/uwp/api/Windows.Devices.Enumeration?redirectedfrom=MSDN&view=winrt-22000) to work with Bluetooth.

# Tips and tricks
* Bluetooth LE devices use mac address randomisation, therefore it is not reliable to pair them using mac address. Use pairing by name instead.
* Some devices advertize itself as Bluetooth and BluetoothLE simultaneously while having the same mac and name. To work with such devices explicitly specify to which type of device you want to connect using `--type` parameter.
* Some device require pin code to be paired, use `--pin` parameter to provide PIN code. By default this programm will try to use `0000` as a pin code.

# Build
* Use `Visual Studio 2022` to open the solution file and work with the code
* Run `.github/workflows/build.ps1` to build a release (to run this script `git.exe` should be in your PATH)

# References
* [Windows.Devices.Enumeration API usage examples](https://github.com/microsoft/Windows-universal-samples/tree/master/Samples/DeviceEnumerationAndPairing)
