using UnityEngine;

public class Interact : MonoBehaviour 
{

	public void Scan()
	{
		int itemID = GameManager.GM.activeArtifact.itemID;

		int type = (int)DataBase.artifacts [itemID].type;

		switch (type) 
		{
		case 0:		// case 0 or 1 :

		case 1:
			
			GameManager.GM.ShowScanPanel (itemID);

			GameManager.GM.CheckMissionScan (itemID);			// Check for mission and mark completed

			break;
		}
	}

	public void Collect()
	{
		GameManager.GM.ClickCollect ();
	}

    public void View()
    {
        GameManager.GM.BuildSiloMenu();
    }

    public void Save()
    {
        GameManager.GM.BuildInventorySiloMenu();
    }
}
