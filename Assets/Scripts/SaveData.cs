using System;
using System.Collections.Generic;


[Serializable]
public class SaveData
{
	public string year = "3018";		// year the player is at

	public float energyLevel = 600.0f;	// Time Travel device energy level


	// Player Inventory:

	public int weightTotal = 0;			// quick check weight

	public Dictionary<int,int> vegetables = new Dictionary<int,int>();				// id, quantity

	public Dictionary<int,int> artifacts = new Dictionary<int,int>();				// id, quantity

	public Dictionary<string,bool> compounds = new Dictionary<string,bool>();       // name, hazardous?


    // Silo contents:

	public Dictionary<int, Silo.SiloContents> silos = new Dictionary<int, Silo.SiloContents>();		  	// id, Silo contents



	// Active or completed missions:

	public Dictionary<int,bool> missions = new Dictionary<int,bool>();		// id, completed?


	// Collected Hieroglyphs:

	public List<int> hieroglyphs = new List<int>();		// id


	// Scene buildings' state:

	public Dictionary<int,BuildingData> buildings = new Dictionary<int,BuildingData>();



//	public int[,,] inventory = {{-1,0,0},{-1,0,0},{-1,0,0},{-1,0,0},{-1,0,0},{-1,0,0}}; 	// {id, quantity, weight}

//	public int[,] vegetablesCollected = {{-1,0},{-1,0}};

//	public int[] itemsCount = {0,0,0,0,0};

}
