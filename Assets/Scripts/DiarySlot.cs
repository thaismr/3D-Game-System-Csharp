using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiarySlot : MonoBehaviour 
{

	public int id = -1;


	// When clicking diary button :

	public void ClickSlot()
	{
		GameManager.GM.BuildDiaryDetails (id);		// Build diary details panel
	}
}
