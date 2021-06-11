using UnityEngine;

/// <summary>
/// 
/// FloatVars : ScriptableObjects for game-wide Float variables
/// 
/// </summary>


[CreateAssetMenu(fileName = "float_", menuName = "Game Wide Floats/New")]
public class FloatVars : ScriptableObject
{
    public float value;
}