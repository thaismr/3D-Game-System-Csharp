using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// DIALOGUES : Dialogues panel & controller
/// 
/// </summary>

public class Dialogues : MonoBehaviour 
{
	
	KeyCode NEXT_DIALOGUE = KeyCode.Tab;			// key to fast forward dialogue


    AudioSource sally;

	public Text dialogueText;

	Queue<Sentences> sentences;

    Sentences sentence;

    char[] letter;

    int index = 0;

    WaitForSeconds wait = new WaitForSeconds(3);

    bool isTyping;


    void Update()
	{
        if (Input.GetKeyDown(NEXT_DIALOGUE))
        {
            if (!isTyping)
                DisplayNextSentence();
            else
                DisplayFullSentence();
        }

        if (isTyping)
        {
            if (index < letter.Length)
            {
                dialogueText.text += letter[index];
                index++;
            }
            else
            {
                isTyping = false;
                index = 0;
                DisplayNextSentence();
            }
        }

    }


	public void StartDialogue(Dialogue dialogue)
	{
        if (!sally)
        {
            sally = GameObject.FindGameObjectWithTag("SALLY").GetComponent<AudioSource>();
        }

        sentences = new Queue<Sentences>();
        
		foreach(Sentences oneSentence in dialogue.sentences)
		{
			sentences.Enqueue(oneSentence);
		}       

		DisplayNextSentence();
	}


	// Display next sentence in Mission's dialogue:

	public void DisplayNextSentence()
	{
        StopAllCoroutines();

        dialogueText.text = "";


        if (sentences.Count == 0)
		{
			EndDialogue();
			return;
		}

		sentence = sentences.Dequeue();

        if (sentence.actions.Length > 0)    // if this sentence has Actions to trigger :
        {
            for (int i = 0; i < sentence.actions.Length; i++)
            {
                GameManager.GM.TriggerAction(sentence.actions[i]);      // call Game Manager to trigger each Action
            }
        }

        if (sentence.voiceSound)
        {
            sally.clip = sentence.voiceSound;
            sally.Play();
        }

        letter = sentence.message.ToCharArray();

        index = 0;

        isTyping = true;

    }




    // Display full sentence in Mission's dialogue:

    void DisplayFullSentence()
    {
       StopAllCoroutines();

       StartCoroutine(TypeFullSentence());
    }


    // Display full sentence:

    IEnumerator TypeFullSentence()
    {
        isTyping = false;

        dialogueText.text = sentence.message;

        yield return wait;

        DisplayNextSentence();
    }



    // Display one notification sentence:

    public void DisplayOneSentence(string _sentence)
    {
        StopAllCoroutines();

        sentence.message = _sentence;

        StartCoroutine(TypeOneSentence());
    }


    // Display notice dialog:

    IEnumerator TypeOneSentence()
    {
        dialogueText.text = sentence.message;

        yield return wait;

        EndDialogue();
    }

    void EndDialogue ()
	{
        GameManager.GM.isDialogueRunning = false;               // tell GM this dialogue ended

        Debug.Log("isDialogRunning = " + GameManager.GM.isDialogueRunning);

        gameObject.SetActive (false);
	}

}