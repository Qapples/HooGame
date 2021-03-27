using System;
using System.IO.Ports;
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
    
    private SerialPort _serialPort;
    private bool _isSlamReady;
    private Vector3 _originalPos;
    private Rigidbody _rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        //Make sure that the time scale is 1. It could be zero if it the previous game was a game over.
        Time.timeScale = 1;
        _originalPos = transform.position;
        _rigidBody = GetComponent<Rigidbody>();
        _isSlamReady = true;
        
        //ensure that the slam value is negative
        slamSpeed = -Math.Abs(slamSpeed);
        
        //Serial setup. Don't do if in debug mode
        if (debug) return;
        _serialPort = new SerialPort {PortName = $"COM{comPort}", BaudRate = baudRate};
        _serialPort.Open();
    }

    // Update is called once per frame
    void Update()
    {
        //If the hand reaches it's "recovery position" after slamming, get ready for another slam
        if (!_isSlamReady && transform.position.y >= _originalPos.y)
        {
            _isSlamReady = true;
            _rigidBody.velocity = Vector3.zero;
        }
        
        //slam the hand if the button is pressed and the hand has recovered
        if (SlamHand && _isSlamReady)
        {
            _rigidBody.velocity = new Vector3(0, slamSpeed, 0);
            _isSlamReady = false;
        }
        
    }
    
    private void OnTriggerEnter(Collider other)
    {
        //if we collided with the bug_, level up. If collided with the floor, begin recovery. If collided
        //with anything else, we lose
        if (other.gameObject.name == "Bug")
        {
            GlobalVar.CurrentLevel++;
            
            _rigidBody.velocity = Vector3.zero;
            _rigidBody.position = _originalPos;
            _isSlamReady = true;
        }
        else if (other.gameObject.name == "Floor")
        {
            //send the hand back up
            _rigidBody.velocity = new Vector3(0, recoverySpeed, 0);
        }
        else //Game over.
        { 
            //TODO: Add more game lose logic
            Debug.Log("Game Over");

            //set the time scale as zero for now. 
            Time.timeScale = 0;
        }
    }

    /// <summary>
    /// Determines if the hand should be slammed down. 
    /// </summary>
    /// <returns></returns>
    private bool SlamHand => debug ? Input.GetKey(KeyCode.Space) : _serialPort.ReadLine()[0] == '1';
}
