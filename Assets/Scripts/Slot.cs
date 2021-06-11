using UnityEngine;

public class Slot : MonoBehaviour 
{
	public int itemid = -1;

	public int type = -1;		// vegetables are default, -1 type


	// When clicking slot button :

	public void ClickSlot()
	{
		if (type == -1)
			GameManager.GM.AddProduce (itemid);		// send vegetable id to Game Manager

		else
			GameManager.GM.ClickSlot (itemid);		// send artifact id to Game Manager
	}
}
