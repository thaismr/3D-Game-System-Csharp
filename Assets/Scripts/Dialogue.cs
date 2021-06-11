using UnityEngine;

/// <summary>
/// 
/// DIALOGUE : Structure for AI dialogues
/// 
/// </summary>
///
    

// Scriptable object for creating individual dialogue objects

[CreateAssetMenu(fileName ="_dialogue", menuName = "Dialogues/New")]
public class Dialogue : ScriptableObject
{
	public string title;

	public Sentences[] sentences;
}


[System.Serializable]		// Serializable so we can build a List of items
public class Sentences
{
    public string characterName;

    [TextArea(3, 5)]
    public string message;

    public Activity.actionTrigger[] actions;

    public AudioClip voiceSound;
}


