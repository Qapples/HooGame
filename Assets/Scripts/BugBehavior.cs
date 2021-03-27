using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BugBehavior : MonoBehaviour
{
    [Header("Movement behavior")]
    [Tooltip("How fast the bug moves left to right")]
    public float movementSpeed;

    private Rigidbody _rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
        _rigidBody.velocity = new Vector3(movementSpeed, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        
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
