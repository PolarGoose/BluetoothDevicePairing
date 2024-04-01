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
### Reconnect scripting example
Many devices (for example, Joy-Con game controllers) cannot be reconnected without unpairing and re-pairing them. With this utility you can emulate the connect button.

#### The setup
We're going to make a simple "Connect" script:
1. Make a folder somewhere. `Documents` is a good candidate. Let's name it `Bluetooth-Scripts`

![изображение](https://github.com/dkgitdev/BluetoothDevicePairing/assets/36101416/6ee821a5-3b4d-448a-b49e-9089eb521a7d)

2. Place `BluetoothDevicePairing.exe` there

![изображение](https://github.com/dkgitdev/BluetoothDevicePairing/assets/36101416/759fad67-16e8-4f00-bf18-ab584a3e74ff)

3. Shift + Right-Click in the empty space in the folder and select CMD / Powershell / Terminal. This will open up a black or blue window with text in it -- the console.

![изображение](https://github.com/dkgitdev/BluetoothDevicePairing/assets/36101416/ab019c99-23d6-489d-96f9-79b19b189aa6)

4. Set device into pairing mode.
5. Run following code in the console by copying it from browser, Right-Clicking in console and then hitting ENTER key:

`.\BluetoothDevicePairing.exe discover`

6. Wait until it prints all devices it found. There you can see **device name**, **MAC** (the symbols like `f3:b8:05:7f:b8:c9`) and if it's **LE device**.

![изображение](https://github.com/dkgitdev/BluetoothDevicePairing/assets/36101416/c68f0fd9-9d3f-41fc-83eb-3016713a0f19)

7. Open up a notepad and copy following code in it:

```
.\BluetoothDevicePairing.exe unpair-by-mac --type <TYPE> --mac <MAC>
.\BluetoothDevicePairing.exe pair-by-mac --type <TYPE> --mac <MAC>
```

8. Replace `<MAC>` with mac of your device and `<TYPE>` with `BluetoothLE` for LE-devices and `Bluetooth` for the rest.

![изображение](https://github.com/dkgitdev/BluetoothDevicePairing/assets/36101416/93beb6a5-a4a0-4aa2-b023-f43dc027f48e)

10. Save file with a name ending with `.bat` (like `connect left joy-con.bat`) in the folder with `BluetoothDevicePairing.exe` (`Bluetooth-Scripts` in `Documents in our example`). If it asks if you are sure to save file with such name, select yes.

![изображение](https://github.com/dkgitdev/BluetoothDevicePairing/assets/36101416/4537f473-a5b3-4f6e-b1bd-133beefeb3e2)

That's it. Now let's use it!

#### Usage

Use connect script like you would use a button:
1. Put device into pairing mode
2. Double click the script
3. Wait for pairing. Pairing can be done a bit after the script closes (10 secs max usually).

# How it works
The program uses
* [Windows.Devices.Enumeration API](https://docs.microsoft.com/en-us/uwp/api/Windows.Devices.Enumeration?redirectedfrom=MSDN&view=winrt-22000) to work with Bluetooth.
* [Costura Fody](https://github.com/Fody/Costura) to create a single file executable.

### Device pairing by name
In order to pair a device by name, the utility starts with discovering all available devices and tries to find a device with the required name. After a device is found its mac address is used to request pairing. The command will fail if there are several devices with the same name.

# Return values
In case of failure the command returns value `-1`. In case of success the `0` is returned.

# Build
* Use `Visual Studio 2022` to open the solution file and work with the code
* Run `.github/workflows/build.ps1` to build a release (to run this script `git.exe` should be in your PATH)

# References
* [Windows.Devices.Enumeration API usage examples](https://github.com/microsoft/Windows-universal-samples/tree/master/Samples/DeviceEnumerationAndPairing)
* [Windows.Devices.Enumeration Namespace](https://docs.microsoft.com/en-us/uwp/api/Windows.Devices.Enumeration)
