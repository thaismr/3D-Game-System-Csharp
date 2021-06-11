using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Diary : MonoBehaviour 
{

	public GameObject _label;

	public GameObject _button;

	public Text dialogueTitle;

	public Transform scrollTransform;


	// Called once for each mission :

	public void BuildPanel (int id, string name)
	{
		GameObject button = Instantiate (_button);				// clone label for mission info

		button.transform.SetParent(scrollTransform, false);		// set this panel as parent (layout component should position it correctly)

		button.GetComponentInChildren<Text> ().text = name;		// write mission name

		button.GetComponent<DiarySlot> ().id = id;			// set mission id for this button

	}


	public void BuildMissionDetails (string name, string details)
	{
		GameObject label = Instantiate (_label);			// clone label for mission info

		label.transform.SetParent(scrollTransform, false);		// set this panel as parent (layout component should position it correctly)

		label.GetComponentInChildren<Text> ().text = name + ": " + details;	// write mission name and details

	}


	// Destroy previous panel:		(called once before rebuilding)

	public void DestroyPanel()
	{
		Button[] buttons = GetComponentsInChildren<Button> ();		// get buttons

		foreach (Button button in buttons)							// destroy each one
		{
			Destroy(button.gameObject);
		}

		Text[] labels = GetComponentsInChildren<Text> ();		// get text labels

		foreach (Text label in labels)							// destroy each one
		{
			if (label != dialogueTitle)							// except Title

				Destroy(label.gameObject);
		}
	}


	// No Mission to show:

	public void BuildNoMissions ()
	{
		GameObject label = Instantiate (_label);			// clone label

		label.GetComponent<Text> ().text = "No missions completed.";

		label.transform.SetParent(scrollTransform, false);			// set this panel as parent
	}
}
