using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// BUILDINGS : Parent controller for Buildings (objects subject to be moved or removed)
/// 
/// </summary>

public class Buildings : MonoBehaviour 
{
	
	/*
	public Building buildings;			// all buildings


	void Start () 
	{
		buildings = GetComponentsInChildren<Building> ();	// get all child buildings

		UpdateBuilding ();		// Update scene's buildings at start (call function outside Start() so GameManager will be loaded first)
	}


	void UpdateBuilding()
	{
		// Call GameManager to update each scene's buildings :

		foreach (Building building in buildings) 
		{
			Transform build = building.gameObject.transform;

			GameManager.GM.UpdateBuilding (building.itemID, building.enabledAtStart, build.position, build.rotation);
		}
	}
	*/

}
