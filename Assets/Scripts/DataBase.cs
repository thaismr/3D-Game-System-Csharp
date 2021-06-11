using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 
/// DATABASE
/// 
/// </summary>

public class DataBase : MonoBehaviour {


	/*
	 * 	Database Elements :
	 * 
	 * */


	// Weight drop down for Unity Inspector:

	public enum Weight
	{
		light = 1, 
		moderate = 2, 
		heavy = 3, 
		superHeavy = 4
	};


	// Type drop down for Unity Inspector:

	public enum ArtifactType
	{
		hieroglyph = 0, 
		relic = 1,
	};


	// Mission Type drop down for Unity Inspector:

	public enum MissionType
	{
		dialogCompleted		= 0, 
		artifactCollected	= 1,
		siloContents		= 2,
		collisionEnter		= 3,
		triggerActivated	= 4,
		harvestProduce		= 5,
		artifactScan		= 6
	};


	// Percentage drop down for Unity Inspector:

	public enum Percent
	{
		_25percent = 25,
		_50percent = 50, 
		_75percent = 75, 
		_100percent = 100
	};


	// Elements for compounds:

	[Serializable]		// Serializable so we can build a List of Elements
	public class Compound
	{
		public bool biohazard;		// is it hazardous?

		public string name;

		public Percent percent;
	}




	/*
	 * 	Database Structure :
	 * 
	 * */


	// Vegetables "DataTable" :


	[Serializable]		// Serializable so we can build a List of items
	public class Vegetable
	{
		public int id;

		public string name;

		public float minutesToGrow;		// applies to crops only

		public float offsetYInit;		// applies to crops only

		public GameObject prefab;
	}



	// Artifacts "DataTable" :


	[Serializable]		// Serializable so we can build a List of items
	public class Artifact
	{
		public int id;

		public string name;

		public bool collectable;			// can player carry this item?

		public ArtifactType type;			// artifact type selection

		public Weight weight;				// weight drop down

		public int dateReading;				// chronometric dating (in years)

		public Sprite image;				// sprite image for UI

		public GameObject prefab;			// 3d game object

		[Multiline]
		public string scanReading;			// what it reads in scan

		[Tooltip("Must sum 100.")]
		public List<Compound> composition;	// chemical composition. Should sum 100.
	}


	/*
	// Hieroglyphs "DataTable" :


	[Serializable]		// Serializable so we can build a List of items
	public class Hieroglyph
	{
		public int id;

		public Sprite image;				// sprite image for UI

		[Multiline]
		public string scanReading;			// what it translates in scan
	}
	*/


	// Missions "DataTable" :


	[Serializable]		// Serializable so we can build a List of items
	public class Missions
	{
		public int id;

		public string title;

		public bool unlocked;				// active at start ?

		public MissionType type;			// mission type from drop down menu

		[Multiline]
		public string description;			// mission details (AI dialog)

		[Multiline]
		public string logCompleted;			// Mission log after completed;

		[Tooltip("IDs for missions required completed before this one.")]
		public List<int> requiresID = new List<int> ();	// required completed missions (id)

		[Tooltip("ID of item to check for mission completion (dialog, vegetable, artifact, etc).")]
		public int	itemID = -1;

		public int quantity = 1;		// how much(/many) of this item is needed?
	}






	/*
	 * 	Database Init :
	 * 
	 * */

	public static DataBase DB;

//	public List<Compound> compounds = new List<Compound>();			// initialize the compounds list


	public List<Vegetable> _vegetables = new List<Vegetable>();		// "non-static" so we can edit from Unity's Inspector

	public static List<Vegetable> vegetables;						// "static" copy to access items from any script


	public List<Artifact> _artifacts = new List<Artifact>();		// "non-static" so we can edit from Unity's Inspector

	public static List<Artifact> artifacts;							// "static" copy to access items from any script


	public List<Missions> _missions = new List<Missions>();		// "non-static" so we can edit from Unity's Inspector

	public static List<Missions> missions;							// "static" copy to access items from any script


	public List<Dialogue> _dialogues = new List<Dialogue>();

	public static List<Dialogue> dialogues;


	void Start()
	{
		
		if (DB == null)
		{
			DB = this;
		}
		else if (DB != this)
		{
			Destroy (gameObject);
		}


		// Copies items filled from the "DataBase" prefab :

		vegetables = new List<Vegetable>(DB._vegetables);		

		artifacts = new List<Artifact>(DB._artifacts);

		missions = new List<Missions>(DB._missions);

		dialogues = new List<Dialogue>(DB._dialogues);

	}
}
