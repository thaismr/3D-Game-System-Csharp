using UnityEngine;
using UnityEngine.UI;

public class SiloUI : MonoBehaviour 
{
	
	public GameObject _label;


	// Called once for each produce :

	public void BuildPanel (string name, int quantity)
	{

		GameObject label = Instantiate (_label);			// clone label for product info

		label.GetComponentInChildren<Text> ().text = name + ": " + quantity;	// write product name and quantity

		label.transform.SetParent(transform, false);			// set this panel as parent (layout component should position it correctly)

	}


	// Destroy previous panel:		(called once before rebuilding)

	public void DestroyPanel()
	{
		Text[] labels = GetComponentsInChildren<Text> ();		// get text labels

		foreach (Text label in labels)							// destroy each one
		{
			if (label.text != "Silo")							// except header

				Destroy (label.gameObject);
		}
	}


	// No Produce to show:

	public void BuildNoProduce ()
	{
		GameObject label = Instantiate (_label);			// clone label

		label.GetComponentInChildren<Text> ().text = "This Silo is empty.";

		label.transform.SetParent(transform, false);			// set this panel as parent
	}


}
