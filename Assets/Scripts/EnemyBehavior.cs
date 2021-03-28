using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    [Header("Enemy Properties")] [Tooltip("How fast the enemy moves side to side")]
    public float moveSpeed;
    
    [Header("Level factors")]
    [Tooltip("How much of an impact the level change has on the speed of the bug.")]
    public float levelFactorSpeed;

    [Tooltip("How often a new enemy should be added after each level")]
    public int frequency;

    private Rigidbody _rigidbody;
    private Vector3 _originalPos;
    private int _prevLevel;
    
    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        moveSpeed = -Math.Abs(moveSpeed);   //ensure that move speed is negative 
        _rigidbody.velocity = new Vector3(moveSpeed, 0, 0);
        _prevLevel = GlobalVar.CurrentLevel;
        _originalPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //reset the position of the enemy and increase the speed
        if (_prevLevel != GlobalVar.CurrentLevel)
        {
            moveSpeed += GlobalVar.CurrentLevel * levelFactorSpeed;
            _rigidbody.position = _originalPos;
            _rigidbody.velocity = new Vector3(moveSpeed, 0, 0);

            //Add a new enemy every "frequency" levels
            if (GlobalVar.CurrentLevel % frequency == 0)
            {
                
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Bound")
        {
            _rigidbody.velocity *= -1;
        }
    }
}
