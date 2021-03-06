using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyBehavior : MonoBehaviour
{
    [Header("Enemy Properties")] [Tooltip("How fast the enemy moves side to side")]
    public float moveSpeed;
    
    [Header("Level factors")]
    [Tooltip("How much of an impact the level change has on the speed of the bug.")]
    public float levelFactorSpeed;

    [Tooltip("How often a new enemy should be added after each level")]
    public int frequency;

    [Header("Prefab")] [Tooltip("This is the prefab that is spawned whenever there is a difficulty increase")]
    public GameObject prefab;

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
            _prevLevel = GlobalVar.CurrentLevel;
            
            moveSpeed += GlobalVar.CurrentLevel * levelFactorSpeed;
            _rigidbody.position = _originalPos;
            _rigidbody.velocity = new Vector3(moveSpeed, 0, 0);

            //Add a new enemy every "frequency" levels
            if (GlobalVar.CurrentLevel % frequency == 0)
            {
                float randomX = Random.Range(-5, 4);
                GameObject obj = Instantiate(prefab, new Vector3(randomX, _originalPos.y, _originalPos.z),
                    new Quaternion(0, 0.7f, 0, 0.7f));
                obj.GetComponent<Rigidbody>().velocity = _rigidbody.velocity;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.name == "Bound")
        {
            _rigidbody.velocity *= -1;
            
            transform.Rotate(0, 180, 0);
        }
    }
}
