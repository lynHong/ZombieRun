using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class HeartRateMonitor : MonoBehaviour
{
    [SerializeField]
    private IntEventSO m_heartRateUpdatedEvent; // Event to be called when heart rate is updated
    // use 128bits UUID for heart rate service and characteristic 
    private string m_heartRateServiceUUID = "000056FF-0000-1000-8000-00805F9B34FB";
    private string m_heartRateCharacteristicUUID = "000034F2-0000-1000-8000-00805F9B34FB";
    private bool m_isScanning = false;

    public TextMeshProUGUI m_HeartrateText; // UI Text to display heart rate

    void Start()
    {
        InitializeBluetooth();
    }

    void InitializeBluetooth()
    {

        BluetoothLEHardwareInterface.Initialize(true, false, () =>
        {

            StartScanning();
        }, (error) =>
        {
            //Log("Bluetooth Initialize Error: " + error, true);
        });
    }

    void StartScanning()
    {

        m_isScanning = true;
        Invoke("StopScanning", 20.0f);
        BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null, (address, name) =>
        {

            if (address == "78:02:B7:DE:92:26")  // Adjust based on your device
            {
                BluetoothLEHardwareInterface.StopScan();
                m_isScanning = false;
                ConnectToDevice(address);
            }

        }, null);
    }

    void StopScanning()
    {
        if (m_isScanning)
        {
            BluetoothLEHardwareInterface.StopScan();
            m_isScanning = false;
        }
    }

    void ConnectToDevice(string address)
    {
        BluetoothLEHardwareInterface.ConnectToPeripheral(address,
            (peripheralName) =>
            {
                //Log("Connected to: " + peripheralName);
            },
            (address, serviceUUID) =>
            {
                //Log("Service Discovered: " + serviceUUID);
                if (serviceUUID.ToUpperInvariant() == m_heartRateServiceUUID.ToUpperInvariant())
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
                m_HeartrateText.text = "Disconnected";
            });
    }

    void SubscribeToHeartRateService(string address, string serviceUUID)
    {
        //Log("Subscribing to Heart Rate Service");
        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(address, serviceUUID, m_heartRateCharacteristicUUID, null,
            (deviceAddress, characteristicUUID, data) =>
            {
                if (data != null && data.Length > 1)
                {
                    int heartRate = data[8];
                    m_heartRateUpdatedEvent?.RaiseEvent(heartRate, this);
                }
                else
                {
                    m_HeartrateText.text = "0";
                }
            });
    }

}
