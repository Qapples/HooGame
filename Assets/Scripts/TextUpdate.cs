using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextUpdate : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GetComponent<Text>().text = GlobalVar.CurrentLevel <= 0 ? "Game over!" : $"Level: {GlobalVar.CurrentLevel}\nCoins: {GlobalVar.Coins}";
    }
}
