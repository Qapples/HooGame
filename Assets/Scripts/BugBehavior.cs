using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Defines the behavior of a bug enemy.
/// </summary>
public class BugBehavior : MonoBehaviour
{
    [Header("Movement behavior")]
    [Tooltip("How fast the bug moves left to right")]
    public float movementSpeed;
    
    [Header("Level factors")]
    [Tooltip("How much of an impact the level change has on the speed of the bug.")]
    public float levelFactorSpeed;

    [Tooltip("How much of an impact the level change has on the size of the bug.")]
    public float levelFactorSize;

    private Rigidbody _rigidBody;
    
    /// <summary>
    /// Used to determine if the level has changed and a reset is necessary.
    /// </summary>
    private int _prevLevel;

    /// <summary>
    /// starting position of the bug_ when a level starts
    /// </summary>
    private Vector3 _startPos;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.velocity = new Vector3(movementSpeed, 0, 0);
        _prevLevel = GlobalVar.CurrentLevel;
        _startPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //If the level changed, then reset position increase the aspects of the "bug_" to make it harder to hit
        //(speed, size, etc.)
        if (_prevLevel != GlobalVar.CurrentLevel)
        {
            _prevLevel = GlobalVar.CurrentLevel;

            movementSpeed = _prevLevel * levelFactorSpeed;
            transform.position = _startPos;
            _rigidBody.velocity = new Vector3(movementSpeed, 0, 0);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //when a bound is hit, reverse the velocity.
        if (other.gameObject.name == "Bound")
        {
            _rigidBody.velocity *= -1;
        }
    }
}
