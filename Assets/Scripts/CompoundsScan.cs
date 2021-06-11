using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// COMPOUND SCAN : Located in the Compounds sub-panel, instantiates compound information UI elements
/// 
/// </summary>

public class CompoundsScan : MonoBehaviour 
{

	public GameObject _label;		// text labels

	public GameObject _button;		// sampling button

	Sample sample;					// Sample class component



	// Called once for each Compound :

	public void BuildPanel (string name, int percent, bool hazardous)
	{
		
		GameObject sampleLabel = Instantiate (_label);			// clone label for compound name

		sampleLabel.GetComponentInChildren<Text> ().text = name;

		sampleLabel.transform.SetParent(transform, false);			// set this panel as parent (layout component should position it correctly)



		GameObject sampleLabel_2 = Instantiate (_label);		// clone label for compound percentage

		sampleLabel_2.GetComponentInChildren<Text> ().text = percent.ToString("## '%'");

		sampleLabel_2.transform.SetParent(transform, false);			// set this panel as parent (layout component should position it correctly)



		GameObject sampleButton = Instantiate (_button);			// clone button for sampling

		sampleButton.GetComponentInChildren<Sample> ().itemName = name;

		sampleButton.GetComponentInChildren<Sample> ().hazardous = hazardous;

		sampleButton.transform.SetParent(transform, false);			// set this panel as parent (layout component should position it correctly)

	}


	// Destroy previous panel:		(called once before rebuilding)

	public void DestroyPanel()
	{
		Button[] samples = GetComponentsInChildren<Button> ();		// get sample buttons

		foreach (Button sample in samples)							// destroy each one

			Destroy (sample.gameObject);


		Text[] labels = GetComponentsInChildren<Text> ();			// get text labels

		foreach (Text label in labels)							// destroy each one

			Destroy (label.gameObject);
	}


	// No Compounds to show:

	public void BuildNoCompounds ()
	{
		GameObject sampleLabel = Instantiate (_label);			// clone label

		sampleLabel.GetComponentInChildren<Text> ().text = "No compound information found.";

		sampleLabel.transform.SetParent(transform, false);			// set this panel as parent (layout component should position it correctly)
	}


}
