using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CoinBehavior : MonoBehaviour
{
    [Header("Properties")]
    public float appearanceOdds;

    [Header("Prefab")]
    public GameObject prefab;
        
    private int _prevLevel;
    // Start is called before the first frame update
    void Start()
    {
        _prevLevel = GlobalVar.CurrentLevel;
    }

    // Update is called once per frame
    void Update()
    {
        //Everytime there is a new level..
        if (_prevLevel != GlobalVar.CurrentLevel)
        {
            _prevLevel = GlobalVar.CurrentLevel;

            if (Random.Range(0, 1) <= appearanceOdds)
            {
                Vector3 newVector = new Vector3(Random.Range(-5f, 4f), 1.5f, -1); 
                GameObject obj = Instantiate(prefab, newVector, transform.rotation);
                obj.name = obj.name.Remove(4);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Destroy(gameObject);
    }
}
