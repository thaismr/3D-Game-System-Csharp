using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissionSlot : MonoBehaviour 
{

	public int id = -1;


	// When clicking mission button :

	public void ClickSlot()
	{
		GameManager.GM.BuildMissionDetails (id);		// Build mission details panel
	}

}
