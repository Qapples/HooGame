using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Global static class that holds data that other entities may need to use
/// </summary>
public static class GlobalVar
{
    /// <summary>
    /// The current level the game is on. 0 or less when the game is not active. When the player moves up a level,
    /// everything should reset and increase in difficulty
    /// </summary>
    public static int CurrentLevel { get; set; }
}
