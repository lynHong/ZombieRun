using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class HeartRateMonitor : MonoBehaviour
{
    [System.Serializable]
    public class HeartRateEvent : UnityEvent<int> { }
    public HeartRateEvent onHeartRateUpdated;

    // 使用完整的128位UUID
    private string heartRateServiceUUID = "000056FF-0000-1000-8000-00805F9B34FB";
    private string heartRateCharacteristicUUID = "000034F2-0000-1000-8000-00805F9B34FB";
    private bool isScanning = false;

    public TextMeshProUGUI m_HeartrateText; // UI Text to display heart rate
    //public TextMeshProUGUI m_DebugText; // UI Text to display debug messages

    void Start()
    {
        if (onHeartRateUpdated == null)
            onHeartRateUpdated = new HeartRateEvent();

        InitializeBluetooth();
    }

    void InitializeBluetooth()
    {
        //Log("Initializing Bluetooth...");
        BluetoothLEHardwareInterface.Initialize(true, false, () =>
        {
            //Log("Bluetooth Initialized");
            StartScanning();
        }, (error) =>
        {
            //Log("Bluetooth Initialize Error: " + error, true);
        });
    }

    void StartScanning()
    {
        //Log("Starting Scan...");
        isScanning = true;
        Invoke("StopScanning", 20.0f);
        BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null, (address, name) =>
        {
            //Log($"Device Found: Name={name}, Address={address}");
            if (address == "78:02:B7:DE:92:26")  // Adjust based on your device
            {
                BluetoothLEHardwareInterface.StopScan();
                isScanning = false;
                //Log("Scan Stopped");
                ConnectToDevice(address);
            }

        }, null);
    }

    void StopScanning()
    {
        if (isScanning)
        {
            BluetoothLEHardwareInterface.StopScan();
            //Log("Scan Stopped after timeout");
            isScanning = false;
        }
    }

    void ConnectToDevice(string address)
    {
        //Log("Connecting to Device: " + address);
        BluetoothLEHardwareInterface.ConnectToPeripheral(address,
            (peripheralName) =>
            {
                //Log("Connected to: " + peripheralName);
            },
            (address, serviceUUID) =>
            {
                //Log("Service Discovered: " + serviceUUID);
                if (serviceUUID.ToUpperInvariant() == heartRateServiceUUID.ToUpperInvariant())
                {
                    //Log("Heart Rate Service Found, ready to subscribe.");
                    SubscribeToHeartRateService(address, serviceUUID);
                }
            },
            (address, serviceUUID, characteristicUUID) =>
            {
                //Log("Characteristic Discovered in service " + serviceUUID + ": " + characteristicUUID);
            },
            (disconnectedAddress) =>
            {
                //Log("Disconnected from " + disconnectedAddress);
                m_HeartrateText.text = "Disconnected";
            });
    }

    void SubscribeToHeartRateService(string address, string serviceUUID)
    {
        //Log("Subscribing to Heart Rate Service");
        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(address, serviceUUID, heartRateCharacteristicUUID, null,
            (deviceAddress, characteristicUUID, data) =>
            {
                if (data != null && data.Length > 1)
                {
                    int heartRate = data[8];
                    m_HeartrateText.text = $"Heart Rate: {heartRate} BPM";
                    onHeartRateUpdated.Invoke(heartRate);
                }
                else
                {
                    m_HeartrateText.text = "0";
                    //Log("Heart Rate Read Failed");
                }
            });
    }

    // Helper method for //logging messages
    /*private void //Log(string message, bool isError = false)
    {
        if (isError)
        {
            Debug.//LogError(message);
        }
        else
        {
            Debug.//Log(message);
        }
        // Update UI with the message, appending new messages to existing ones
        if (m_DebugText != null)
        {
            m_DebugText.text += message + "\n\n"; // Append new message with two newlines for separation
        }
    }*/
}
