using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour 
{
	GameManager GM;

	public const string TAG = "Trigger";

	public bool active;

	public bool completed;

	public bool repeat;


	public int[] missionRequired;		// required missions before activating dialog ?

	public int[] missionTrigger;		// Trigger (unlock) missions ?

    public int[] missionComplete;       // Mark missions complete ?


    int dialogIndex = 0;		    	// saves dialog order (for multiple collisons)

	Dialogues dialogues;

	GameObject dialogPanel;


	public List<Dialogue> dialogue = new List<Dialogue>();          // List of sentences to fill from inspector

    public string tipText;                                          // TIP after dialogue end

	public AudioClip voice;                                         // A.I. voice


	void Start () 
	{
		GM = GameManager.GM;

		dialogPanel = GM.dialogPanel;

		dialogues = dialogPanel.GetComponent<Dialogues> ();
	}

	public void Dialogue () 
	{

        if (GM.isDialogueRunning)        // some dialogue is already running

            return;

        if (completed && !repeat)        // if completed and not to repeat

            return;

        if ( !CheckMissionsRequired () || !active )		// Check if active & requirements met
			
			return;

        if (dialogue.Count <= dialogIndex)              // end of dialogues list

            return;


        GM.isDialogueRunning = true;                // tell GM a dialogue is running

        Debug.Log("isDialogRunning = " + GM.isDialogueRunning);


        dialogPanel.SetActive (true);

		dialogues.StartDialogue (dialogue[dialogIndex]);	// run current dialog

		++dialogIndex;										// add to dialogue index


		if ( dialogue.Count == dialogIndex )				//  this was the last dialogue

			LastDialogue();
		
	}

	void LastDialogue () 
	{
        if (repeat)
            dialogIndex = 0;

		completed = true;							// mark this trigger dialog as completed

        GM.isDialogueRunning = false;               // tell GM this dialogue ended

        Debug.Log("isDialogRunning = " + GM.isDialogueRunning);


		if (missionTrigger.Length > 0)				// if there's a Mission to trigger active :
			
			for (int i=0; i < missionTrigger.Length; i++)
			
				GM.UnlockMission (missionTrigger[i]);		// call Game Manager to unlock Missions


        if (missionComplete.Length > 0)              // if there's a Mission to trigger complete :

            for (int i = 0; i < missionComplete.Length; i++)

                GM.MissionMarkCompleted(missionComplete[i]);        // call Game Manager to mark Missions complete


        if (tipText != "")      // if there's a TIP to show after dialogue end
        {
            dialogPanel.SetActive(true);

            dialogues.DisplayOneSentence(tipText);	// call dialog panel to display this TIP
        }

    }

	bool CheckMissionsRequired () 
	{
		bool okay = true;

        if (missionRequired.Length > 0)             // if there's a Mission attached to it :
        {
            for (int i = 0; i < missionRequired.Length; i++)
            {
                if (!GM.CheckMissionCompleted(missionRequired[i]))  // Ask GM if missions were completed (if NOT:)

                    okay = false;
            }
        }

		return okay;
	}

	void Activate (bool _active) 
	{
		active = _active;
	}

	void Complete (bool _completed) 
	{
		completed = _completed;
	}
}
