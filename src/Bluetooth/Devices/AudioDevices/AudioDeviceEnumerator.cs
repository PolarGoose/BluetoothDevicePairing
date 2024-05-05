using static Vanara.PInvoke.CoreAudio;
using System;
using Vanara.PInvoke;
using System.Collections.Generic;

namespace BluetoothDevicePairing.Bluetooth.Devices.AudioDevices;

internal static class AudioDeviceEnumerator
{
    static readonly Dictionary<Guid, List<AudioDevice>> audioDevices = [];

    static AudioDeviceEnumerator()
    {
        var audioEndpointsEnumerator = new IMMDeviceEnumerator();
        foreach (var audioEndPoint in EnumerateAudioEndpoints(audioEndpointsEnumerator))
        {
            foreach (var connector in EnumerateConnectors(audioEndPoint))
            {
                var connectedToPart = connector.TryGetConnectedToPart();
                if (connectedToPart is null)
                {
                    continue;
                }

                var connectedToDeviceId = (string)connectedToPart.GetTopologyObject().GetDeviceId();
                if (!connectedToDeviceId.StartsWith(@"{2}.\\?\bth"))
                {
                    continue;
                }

                var connectedToDevice = audioEndpointsEnumerator.GetDevice(connectedToDeviceId);
                var ksControl = Activate<IKsControl>(connectedToDevice);
                AddToDevicesDictionary(new AudioDevice(audioEndPoint, ksControl));
            }
        }
    }

    public static IEnumerable<AudioDevice> GetAudioDevices(Guid containerId)
    {
        if (!audioDevices.TryGetValue(containerId, out var value))
        {
            return [];
        }

        return value;
    }

    private static void AddToDevicesDictionary(AudioDevice audioDevice)
    {
        if (!audioDevices.ContainsKey(audioDevice.ContainerId))
        {
            audioDevices[audioDevice.ContainerId] = [];
        }

        audioDevices[audioDevice.ContainerId].Add(audioDevice);
    }

    private static IEnumerable<IMMDevice> EnumerateAudioEndpoints(IMMDeviceEnumerator enumerator)
    {
        var deviceCollection = enumerator.EnumAudioEndpoints(EDataFlow.eAll, DEVICE_STATE.DEVICE_STATEMASK_ALL);
        for (uint i = 0; i < deviceCollection.GetCount(); i++)
        {
            deviceCollection.Item(i, out var device);
            yield return device;
        }
    }

    private static IEnumerable<IConnector> EnumerateConnectors(IMMDevice audioEndPoint)
    {
        var topology = Activate<IDeviceTopology>(audioEndPoint);
        for (uint i = 0; i < topology.GetConnectorCount(); i++)
        {
            yield return topology.GetConnector(i);
        }
    }

    private static IPart TryGetConnectedToPart(this IConnector connector)
    {
        try
        {
            return (IPart)connector.GetConnectedTo();
        }
        catch (Exception)
        {
            return null;
        }
    }

    private static T Activate<T>(IMMDevice device)
    {
        device.Activate(typeof(T).GUID, Ole32.CLSCTX.CLSCTX_ALL, null, out var itf);
        return (T)itf;
    }
}
