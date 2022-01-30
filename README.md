# BluetoothDevicePairing
Console utility to discover and pair Bluetooth and Bluetooth Low Energy devices.

# Note on connecting to Bluetooth devices
If you pair a device which is not already paired, the utility will also connect to it (this is default behaviour of Windows Bluetooth API)<br>
However, if device is paired but not connected, the pairing will fail.<br>
Unfortunately, currently it is not possible to simulate what "Connect" button from Windows `Bluetooth and Other devices` dialog does.
More details can be found here: [How to connect to a paired audio Bluetooth device](https://stackoverflow.com/questions/62502414/how-to-connect-to-a-paired-audio-bluetooth-device-using-windows-uwp-api). Specifically, [here](https://github.com/inthehand/32feet/issues/132#issuecomment-1019786324) I have described my failed attempts to implement this functionality.<br>

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
* Pair a device using its mac address:
```
BluetoothDevicePairing.exe pair-by-mac --mac 12:34:56:78:9A:BC --type Bluetooth
```
* Pair a device using its name:
```
BluetoothDevicePairing.exe pair-by-name --name "MX Ergo" --type BluetoothLE
```
* Pair a device using its name and pin code:
```
BluetoothDevicePairing.exe pair-by-name --name "Device name" --type BluetoothLE --pin 1234
```
* Pair a device using its mac and pin code:
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
* List all Bluetooth adapters available you your machine
```
BluetoothDevicePairing.exe list-adapters
```

# How it works
The program uses
* [Windows.Devices.Enumeration API](https://docs.microsoft.com/en-us/uwp/api/Windows.Devices.Enumeration?redirectedfrom=MSDN&view=winrt-22000) to work with Bluetooth.
* [Costura Fody](https://github.com/Fody/Costura) to create a single file executable.

## Device pairing by name
In order to pair a device by name, the utility starts with discovering all available devices and tries to find a device with the required name. After a device is found its mac address is used to request pairing. The command will fail if there are several devices with the same name.

# Return values
In case of failure the command returns value `-1`. In case of success the `0` is returned.

# Build
* Use `Visual Studio 2022` to open the solution file and work with the code
* Run `.github/workflows/build.ps1` to build a release (to run this script `git.exe` should be in your PATH)

# References
* [Windows.Devices.Enumeration API usage examples](https://github.com/microsoft/Windows-universal-samples/tree/master/Samples/DeviceEnumerationAndPairing)
* [Windows.Devices.Enumeration Namespace](https://docs.microsoft.com/en-us/uwp/api/Windows.Devices.Enumeration)
