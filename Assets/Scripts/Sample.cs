using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// SAMPLE BUTTON : Located in the Scan panel. Button for sampling a compound.
/// 
/// </summary>

public class Sample : MonoBehaviour 
{
	public string itemName;

	public bool hazardous;


	public void TakeSample () 
	{
		Debug.Log ("Take Sample " + itemName);

		GameManager.GM.ClickSample (itemName, hazardous);
	}
}
