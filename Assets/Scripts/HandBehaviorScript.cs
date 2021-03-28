using System;
using System.IO.Ports;
using System.Threading;
using UnityEditor;
using UnityEngine;

public class HandBehaviorScript : MonoBehaviour
{
    /// <summary>
    /// Debug mode. Takes input from keyboard instead of serial ports.
    /// </summary>
    [Header("Debug Mode")] public bool debug;
    
    [Header("Serial Communication")] [Tooltip("Com port of the device to communicate to")]
    public int comPort;

    [Tooltip("Baud rate of communication")]
    public int baudRate;

    [Header("Hand Properties")] [Tooltip("How fast the hand goes up when it tries to recover")]
    public float recoverySpeed;

    [Tooltip("How fast the hand slams down")]
    public float slamSpeed;

    [Tooltip("How fast the hand moves side to side")]
    public float moveSpeed;
    
    private SerialPort _serialPort;
    private bool _isSlamReady;
    private Rigidbody _rigidBody;
    
    private Vector3 _originalPos;
    private Thread _serialThread;
    private Vector3 _velocity => _rigidBody.velocity;
    

    // Start is called before the first frame update
    void Start()
    {
        //Make sure that the time scale is 1. It could be zero if it the previous game was a game over.
        Time.timeScale = 1;
        _originalPos = transform.position;
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.velocity = new Vector3(moveSpeed, 0, 0);
        _isSlamReady = true;
        slamSpeed = -Math.Abs(slamSpeed); //ensure that the slam value is negative
        
        //Serial setup. Don't do if in debug mode
        if (debug) return;
        _serialPort = new SerialPort {PortName = $"COM{comPort}", BaudRate = baudRate};
        _serialPort.Open();

        //Start the thread
        _serialThread = new Thread(SerialThread);
        _serialThread.Start();
        
        //events
        Application.quitting += () =>
        {
            Debug.Log("Quitting...");
            _serialThread.Abort();
            _serialPort.Dispose();
        };
    }

    // Update is called once per frame
    void Update()
    {
        //If the hand reaches it's "recovery position" after slamming, get ready for another slam
        if (!_isSlamReady && transform.position.y >= _originalPos.y)
        {
            _isSlamReady = true;
            _rigidBody.velocity = new Vector3(_velocity.x, 0, 0);
        }
        
        //slam the hand if the button is pressed and the hand has recovered
        if (SlamHand && _isSlamReady)
        {
            _rigidBody.velocity = new Vector3(_velocity.x, slamSpeed, 0);
            _isSlamReady = false;
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Hit: " + other.gameObject.name);
        //if we collided with the bug_, level up. If collided with the floor, begin recovery. If collided
        //with anything else, we lose
        switch (other.gameObject.name)
        {
            case "Bug":
                //reset the scene
                GlobalVar.CurrentLevel++;
                
                _rigidBody.velocity = new Vector3(_velocity.x , 0, 0);
                _rigidBody.position = _originalPos;
                _isSlamReady = true;
                break;
            case "Floor":
                //send the hand back up
                _rigidBody.velocity = new Vector3(_velocity.x, recoverySpeed, 0);
                break;
            case "Bound":
                _rigidBody.velocity = new Vector3(-_velocity.x, _velocity.y, 0);
                break;
            default:
                //TODO: Add more game lose logic
                Debug.Log("Game Over");

                //set the time scale as zero for now. 
                Time.timeScale = 0;
                break;
        }
    }

    private bool _isReceivedOne;
    private void SerialThread()
    {
        _serialPort.DiscardInBuffer();
        while (true)
        {
            _isReceivedOne = _serialPort.ReadByte() == 1;
            Thread.Sleep(20);
        }
    }

    /// <summary>
    /// Determines if the hand should be slammed down. 
    /// </summary>
    /// <returns></returns>
    private bool SlamHand => debug ? Input.GetKey(KeyCode.Space) : _isReceivedOne;
}
