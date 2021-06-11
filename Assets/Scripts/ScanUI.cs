using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// SCAN UI : Located in the Scan Panel, builds scan information UI
/// 
/// </summary>


public class ScanUI : MonoBehaviour 
{
	
	public Text scanReading;


	// Build Scan panel information:

	public void BuildScanPanel(int itemID)
	{
		
		scanReading.text = DataBase.artifacts[itemID].scanReading;


		CompoundsScan panel = GetComponentInChildren<CompoundsScan>();		// get compounds panel, child to this panel

		panel.DestroyPanel ();		// Destroy elements before rebuilding


		if (DataBase.artifacts [itemID].composition.Count == 0)

			panel.BuildNoCompounds ();


		// Rebuild each slot :

		for (int i=0; i < DataBase.artifacts[itemID].composition.Count; i++) 
		{
			string name = DataBase.artifacts[itemID].composition[i].name;			// get the name from the database

			int percent = (int)DataBase.artifacts[itemID].composition[i].percent;			// get the percentage from the database

			bool hazardous = DataBase.artifacts[itemID].composition[i].biohazard;			// biohazard?

			panel.BuildPanel(name, percent, hazardous);									// build a compound line
		}

	}
}
