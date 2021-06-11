using UnityEngine;

/// <summary>
/// 
/// SPAWN : Spawns a game object
/// 
/// </summary>

public class Spawn : MonoBehaviour 
{

	public GameObject[] spawnItem;	// item (gameobject) to spawn


	public void SpawnItem (int itemID)
	{
		
		if (itemID != -1)				// If item to spawn is set from function call :
		{	
			GameObject spawnItem = Instantiate (DataBase.vegetables[itemID].prefab);		// instantiate item from database
			spawnItem.transform.parent = gameObject.transform;						// position item inside this gameobject
			spawnItem.transform.localPosition = new Vector3(0, DataBase.vegetables[itemID].offsetYInit, 0);						// offset from this gameobject
		}

	}

	public void GrowItem(int itemID)
	{
		SphereCollider sprout = GetComponentInChildren<SphereCollider> ();

		sprout.gameObject.transform.localPosition += new Vector3(0, 0.25f, 0);		// grow from the soil
	}

	public void HarvestItem()
	{
        SphereCollider sprout = GetComponentInChildren<SphereCollider> ();

		Destroy (sprout.gameObject);
	}
}
