using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class Mission : MonoBehaviour 
{
	MissionSlot slot;

	public GameObject _label;

	public GameObject _button;

	public Text missionText;

	public Transform scrollTitles;

	public Transform scrollDescription;



	// Called once for each mission :

	public void BuildPanel (int id, string name)
	{
		GameObject button = Instantiate (_button);				// clone label for mission info

		button.transform.SetParent(scrollTitles, false);		// set this panel as parent (layout component should position it correctly)

		button.GetComponentInChildren<Text> ().text = name;		// write mission name

		slot = button.GetComponent<MissionSlot> ();

		slot.id = id;			// set mission id for this button
	}


	public void BuildMissionDetails (string name, string details)
	{
		_label.GetComponentInChildren<Text> ().text = name;	// write mission name and details

		missionText.text = details;	// write mission name and details
	}


	// Destroy previous panel:		(called once before rebuilding)

	public void DestroyPanel()
	{
		Button[] buttons = GetComponentsInChildren<Button> ();		// get buttons

		foreach (Button button in buttons)							// destroy each one
		{
			Destroy(button.gameObject);
		}

		_label.GetComponentInChildren<Text> ().text = "No mission selected...";

		missionText.text = " ";

		/*
		Text[] labels = GetComponentsInChildren<Text> ();		// get text labels

		foreach (Text label in labels)							// destroy each one
		{
			if (label != missionText)							// except Mission Description
			
				Destroy(label.gameObject);
		}
		*/
	}


	// No Mission to show:

	public void BuildNoMissions ()
	{
	//	GameObject label = Instantiate (_label);			// clone label

		_label.GetComponentInChildren<Text> ().text = "No active missions.";

	//	label.transform.SetParent(scrollDescription, false);			// set this panel as parent

		missionText.text = "No active missions.";
	}


}
