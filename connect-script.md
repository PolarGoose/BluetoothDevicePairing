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
