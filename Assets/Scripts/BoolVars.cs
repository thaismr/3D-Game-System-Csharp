using UnityEngine;

/// <summary>
/// 
/// BoolVars : ScriptableObjects for game-wide bool variables
/// 
/// </summary>


[CreateAssetMenu(fileName = "bool_", menuName = "Game Wide Bools/New")]
public class BoolVars : ScriptableObject
{
    public bool value;
}