using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.Serialization;

public class HandBehaviorScript : MonoBehaviour
{
    [Header("Serial Communication")]
    [Tooltip("Com port of the device to communicate to")]
    public int comPort;

    [Tooltip("Baud rate of communication")]
    public int baudRate;
    
    private SerialPort _serialPort;
    
    // Start is called before the first frame update
    void Start()
    {
        _serialPort = new SerialPort {PortName = $"COM{comPort}", BaudRate = baudRate};

        _serialPort.Open();
    }

    // Update is called once per frame
    void Update()
    {
        string data = _serialPort.ReadLine();
        if (data.Length > 0)
        {
            Debug.Log(data);
        }
    }
}
