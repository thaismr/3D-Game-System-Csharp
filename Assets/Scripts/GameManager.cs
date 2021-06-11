using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System.IO;

/// <summary>
/// 
/// GAME MANAGER
/// 
/// </summary>


public class GameManager : MonoBehaviour 
{

	public static GameManager GM;


	// Player data :

	const string SAVE_DATA_FILE = "save11.dat";

	private string saveDataPath;

	private SaveData saveData;


	// Scene data :

	public int sceneID;

	private SceneData sceneData;

    string sceneYear = "3018";

    public BoolVars isInPast;


    public bool canTravel = true;          // turn on/off time travel


    const string DO_ACTIVATE_TRIGGER = "DoActivateTrigger";		// Method to call from Trigger with BroadcastMessage();

	const int INVENTORY_MAX_WEIGHT = 6;

	const int INVENTORY_MAX_ITEMS = 6;

	const float ENERGY_MAX_LEVEL = 600.0f;

	private float energyLevel;

	float loadProgress;



	public const KeyCode

        KEY_CODE_LIFT = KeyCode.E,              // Key code to move lift

        KEY_CODE_INVENTORY = KeyCode.I,			// Key code to open inventory menu

		KEY_CODE_TIME_TRAVEL = KeyCode.X,		// Key code to travel back and forth in time

		KEY_CODE_INTERACT = KeyCode.Mouse0,	    // Key code to open interaction menu

		KEY_CODE_CLOSE = KeyCode.Mouse1,        // Key code to close interaction menu

        KEY_CODE_INTERACT_ALT = KeyCode.JoystickButton0,		// Alt Key code to open interaction menu

		KEY_CODE_CLOSE_ALT = KeyCode.JoystickButton1,	    	// Alt Key code to close interaction menu

		KEY_CODE_INVENTORY_ALT = KeyCode.JoystickButton5,		// Alt Key code to open inventory menu

		KEY_CODE_DELETE = KeyCode.R,			// Key code to delete saved data

		KEY_CODE_QUIT = KeyCode.Q;              // Key code to quit game



    const int UNLOCK_FOG_MISSION_ID = 5;        // Mission ID to unlock Fog

    const int UNLOCK_TRAVEL_MISSION_ID = 6;     // Mission ID to unlock Time-travel



    public GameObject[] gameMenus;				// List of menus to check if open

	public GameObject 

		fog,

		player,

		canvas,

		pastScene,

		presentScene,

		basePast,

		baseScene,

		collectables,

		buildingsList,

		sproutsMenu,

		dataMenu,
		
		alertMenu,

		harvestMenu,

		interactMenu,

		scanPanel,

        siloMenu,
		
		sallyPanel,

		dialogPanel,

		loadingScreen,
                
        lockedEscapeDoor;


    public GameObject[] sallyPanels;		// Panel tabs under SALLY AI

    private GameObject[] emergencyAlarmParents,
        
                        emergencyDoorParents;



    public Text yearNow;					// Text component to show current year

	public Slider energySlider;				// Slider component to show energy levels

	public Slider progressSlider;

	public Mission mission;

    public Activity[] actionAlarm, actionDoor;

	public string alertMessage = "";

	public Plot activePlot;

	public Artifact activeArtifact;				// artifact under raycast

    public Silo activeSilo;

	public static List<int> collectedArtifacts = new List<int>();		// player collected artifacts


    public bool isDialogueRunning = false;      // already running a dialogue?



    void Start() 
	{
		if (GM == null)
			GM = this;

		else if (GM != this)
			Destroy (gameObject);




		// Start with scene Canvas inactive:

		canvas.SetActive (false);


		// Player data:

		saveDataPath = Application.persistentDataPath + "/" + SAVE_DATA_FILE;


        /*
        if (File.Exists(saveDataPath))
            LoadSaveData();
        else
            CreateSaveData();
        */



        if (File.Exists(saveDataPath))
            DeleteSaveData();

        CreateSaveData();



        saveData.year = sceneYear;				// Scene loaded sets the year

		energyLevel = saveData.energyLevel;		// load energy level from save file

		collectedArtifacts = saveData.artifacts.Keys.ToList();



		Cursor.lockState = CursorLockMode.Locked;		// make sure cursors start hidden
		Cursor.visible = false;

		CloseMenus();			// start with all Menus closed

		Time.timeScale = 1;		// make sure game is not paused


		// Load scenes async and additive :

		sceneID = 0;

		loadProgress = 0;

		SceneManager.sceneLoaded += OnSceneLoaded;	    // Function to be called for each scene loaded

		StartCoroutine(LoadSceneAsync ("Base Past"));   // Load past scene

	}


	IEnumerator LoadSceneAsync(string sceneName)
	{
		AsyncOperation sceneOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);		// Load  scene

		while (!sceneOperation.isDone) 
		{
			float progress = (sceneOperation.progress + 0.1f) / 2;		// from 0 to 0.9, add the extra 0.1, and divide by 2 scenes

			progressSlider.value = loadProgress + progress;				// change slider value in screen

			yield return null;
		}
	}


	void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{

		if (scene.name == "Base Past")
		{
			UpdateSceneData ();					// Update objects & artifacts in scene

			basePast = GameObject.Find ("BasePast");

			basePast.SetActive (false);

			loadProgress = 0.5f;									// progress is now 1 of 2 scenes

			StartCoroutine(LoadSceneAsync ("Base"));
		}
		else if (scene.name == "Base")
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;

			UpdateSceneData ();					// Update objects & artifacts in scene

			baseScene = GameObject.Find ("Base");


            // Emergency alarms:

            emergencyAlarmParents = GameObject.FindGameObjectsWithTag("EmergencyAlarmParent");

            Debug.Log("emergency alarms: " + emergencyAlarmParents.Length);

            actionAlarm = new Activity[emergencyAlarmParents.Length];

            for (int i = 0; i < emergencyAlarmParents.Length; i++)
            {
               actionAlarm[i] = emergencyAlarmParents[i].GetComponent<Activity>();
            }


            // Emergency doors:

            emergencyDoorParents = GameObject.FindGameObjectsWithTag("EmergencyDoorParent");

            Debug.Log("emergency doors: " + emergencyDoorParents.Length);

            actionDoor = new Activity[emergencyDoorParents.Length];

            for (int i = 0; i < emergencyDoorParents.Length; i++)
            {
                actionDoor[i] = emergencyDoorParents[i].GetComponent<Activity>();
            }



            loadingScreen.SetActive (false);				// disable loading canvas

			canvas.SetActive (true);                        // Set scene canvas active


            // Init present scene functions:

            isInPast.value = false;

            canvas.transform.SetParent(baseScene.transform);

            player.transform.SetParent(baseScene.transform);

			player.SetActive (true);

			fog.SetActive (true);

			UpdateMissionList ();
		}
	}


	private void CreateSaveData()
	{
		saveData = new SaveData();
	}
		
	private void LoadSaveData()
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream      fileStream      = File.Open(saveDataPath, FileMode.Open);

		saveData = (SaveData)binaryFormatter.Deserialize(fileStream);

		fileStream.Close();
	}

	private void StoreSaveData()
	{
		BinaryFormatter binaryFormatter = new BinaryFormatter();
		FileStream      fileStream      = File.Create(saveDataPath);

		binaryFormatter.Serialize(fileStream, saveData);

		fileStream.Close();
	}

	private void DeleteSaveData()	// erase file to commit structural changes
	{
        if (File.Exists(saveDataPath))
            File.Delete (saveDataPath);
	}


	void UpdateSceneData()
	{
		collectables = GameObject.Find ("Collectables");

		buildingsList = GameObject.Find ("Buildings");


		Artifact[] artifacts = collectables.GetComponentsInChildren<Artifact> ();		// Get all Artifacts inside Collectables

		foreach (Artifact artifact in artifacts)
			if (collectedArtifacts.Contains (artifact.itemID))
				artifact.DoCollect ();

		Building[] buildings = buildingsList.GetComponentsInChildren<Building> ();			// Get all Buildings inside Buildings

		foreach (Building building in buildings) 
		{
			if (saveData.buildings.ContainsKey (building.itemID)) {
				if (saveData.buildings [building.itemID].isEnabled == true) {
					building.gameObject.SetActive (true);

					float posX = saveData.buildings [building.itemID].posX;
					float posY = saveData.buildings [building.itemID].posY;
					float posZ = saveData.buildings [building.itemID].posZ;

					building.gameObject.transform.position = new Vector3 (posX, posY, posZ);

					float qX = saveData.buildings [building.itemID].quaternion [0];
					float qY = saveData.buildings [building.itemID].quaternion [1];
					float qZ = saveData.buildings [building.itemID].quaternion [2];
					float qW = saveData.buildings [building.itemID].quaternion [3];

					building.gameObject.transform.rotation = new Quaternion (qX, qY, qZ, qW);

					Debug.Log ("is enabled - id: " + building.itemID);

				} 
				else 
				{
					// If found in database and not marked enabled, disable it :

					building.gameObject.SetActive (false);

					Debug.Log ("Building not enabled - id: " + building.itemID);
				}
			} 
			else if (! building.enabledAtStart)
			{
				// If not to be enabled at start, and not found in database, disable it:

				building.gameObject.SetActive (false);

				Debug.Log ("Building key not saved - id: " + building.itemID);
			}
		}
	}

	void Update()
	{
		

		if ( (sceneYear == "3018") && (energyLevel < ENERGY_MAX_LEVEL) )
		
			energyLevel += Time.fixedDeltaTime;		// soma 1 de energia a cada segundo

		else if ( (sceneYear == "-55.000") && (energyLevel > 0) )
			
			energyLevel -= Time.fixedDeltaTime;		// retira 1 de energia a cada segundo


		if ( (energyLevel < 10) && (sceneYear == "-55.000") )		// quando a energia chegar a 0 no passado :

			BackToTheFuture ();		// de volta ao futuro
				
		
		energySlider.value = energyLevel;			// Set energy level slider



		if (Input.GetKeyDown (KEY_CODE_CLOSE) || Input.GetKeyDown (KEY_CODE_CLOSE_ALT)) 
		{
			CloseMenus ();
		}
		else if (Input.GetKeyDown(KEY_CODE_DELETE))		// erase stored SaveData file
		{
			DeleteSaveData();
		}
		else if (Input.GetKeyDown(KEY_CODE_INVENTORY) || Input.GetKeyDown (KEY_CODE_INVENTORY_ALT))		// show AI panel
		{			
			ShowSALLYPanel();
		}
		else if (Input.GetKeyDown(KEY_CODE_TIME_TRAVEL))		// travel in time
		{
			TimeTravel ();
		}
		else if (Input.GetKeyDown(KEY_CODE_QUIT))
		{
			#if UNITY_EDITOR
			UnityEditor.EditorApplication.isPlaying = false;
			#else
			Application.Quit();
			#endif
		}
	}


	public void Restart()
	{
		saveData.energyLevel = energyLevel;

		StoreSaveData ();

		alertMessage = "You died!";

		ShowAlertMenu ();

		Time.timeScale = 0;

		SceneManager.LoadSceneAsync(0);		// Back to Main Menu scene
	}


	public void TravelToThePast()
	{
	//	Time.timeScale = 0;

		sceneYear = "-55.000";

		yearNow.text = "-55.000";

		saveData.year = sceneYear;

		saveData.energyLevel = energyLevel;

		StoreSaveData ();

		alertMessage = "Traveling to the past...";

		ShowAlertMenu ();

		fog.SetActive (false);


		Transform playerAt = player.GetComponent<Transform> ();
			
		playerAt.position = new Vector3(0,15,0);		// Place player back on terrain top


        isInPast.value = true;


		sceneID = 1;

		basePast.SetActive(true);

		baseScene.SetActive(false);

        canvas.transform.SetParent(basePast.transform);

        player.transform.SetParent(basePast.transform);


	//	Time.timeScale = 1;

		Invoke ("CloseMenus", 2);		// Wait 2s and close menus

	}


	public void BackToTheFuture()
	{
	//	Time.timeScale = 0;

		sceneYear = "3018";

		yearNow.text = "3018";

		saveData.energyLevel = energyLevel;

		saveData.year = sceneYear;

		StoreSaveData ();

		alertMessage = "Traveling back to the future...";

		ShowAlertMenu ();

		fog.SetActive (true);


		Transform playerAt = player.GetComponent<Transform> ();

		playerAt.position = new Vector3(0,3,0);		// Place player back on terrain top


        isInPast.value = false;


		sceneID = 0;

		basePast.SetActive(false);

		baseScene.SetActive(true);

        canvas.transform.SetParent(baseScene.transform);

        player.transform.SetParent(baseScene.transform);
        

	//	Time.timeScale = 1;

		Invoke ("CloseMenus", 2);		// Wait 2s and close menus

	}


	// Time Travel key presed:

	public void TimeTravel()
	{
		if ((sceneYear == "3018") && (energyLevel > 30) && canTravel) 
		{	
			// minimum 30s energy levels to travel to past
			TravelToThePast ();
		} 
		else if (sceneYear == "-55.000") 
		{		
			// always ok to go back to the future :
			BackToTheFuture ();
		}
	}


	public void CloseMenus()
	{
		if (gameMenus.Length > 0)
		{ 												// check if list of menus has items
			foreach (GameObject menu in gameMenus) 
			{
				menu.SetActive (false);
			}

            Cursor.lockState = CursorLockMode.Locked;

            Cursor.visible = false;
        }

	}

	public void ShowSproutsMenu(bool show) 
	{
		sproutsMenu.SetActive (show);
	}

	public void ShowInteractMenu(bool show) 
	{
		CloseMenus ();

		interactMenu.SetActive (show);
	}

    public void ShowSiloMenu(bool show)
    {
        CloseMenus();

        siloMenu.SetActive(show);
    }

    public void ShowDataMenu(string name, float time) 
	{
		CloseMenus ();

		dataMenu.SetActive (true);

		dataMenu.GetComponentInChildren<Timer> ().setName = name;
		dataMenu.GetComponentInChildren<Timer> ().setTimer = time;
	}

	public void ShowHarvestMenu(bool show) 
	{
		if (alertMenu.activeInHierarchy)
			
			return;
		
		CloseMenus ();

		harvestMenu.SetActive (show);
	}

	public void ShowScanPanel(int itemID)
	{
		CloseMenus ();					// make sure all other menus are closed

		scanPanel.GetComponent<ScanUI>().BuildScanPanel(itemID);			// build scan panel

		scanPanel.SetActive (true);
	}

    public void ShowSiloPanel()
    {
        CloseMenus();               // make sure all other menus are closed

        BuildSiloMenu();            // build scan panel

        siloMenu.SetActive(true);
    }

    public void ShowSALLYPanel()
	{
		CloseMenus ();					// make sure all other menus are closed

		sallyPanel.SetActive (true);


		foreach (GameObject panel in sallyPanels)		// make sure all tabs are closed

			panel.SetActive (false);
		

		sallyPanels [3].SetActive (true);	// leave Inventory as default active tab

		BuildInventoryMenu ();				// build inventory section
	}

    public void ShowSALLYSubPanel()
    {
        CloseMenus();                   // make sure all other menus are closed

        sallyPanel.SetActive(true);


        foreach (GameObject panel in sallyPanels)       // make sure all tabs are closed

            panel.SetActive(false);
        
    }


    // Build Inventory section of AI Menu:

    public void BuildInventoryMenu()
	{

		Inventory inventory = sallyPanel.GetComponentInChildren<Inventory>();

		if (!inventory)
			return;

        //		inventory.DestroySlots ();		// Destroy current menu before rebuilding

        int i = 0;

		// Rebuild each slot :

		if (saveData.artifacts.Count > 0) 
		{
			foreach (int key in saveData.artifacts.Keys) 
			{
				string name = DataBase.artifacts [key].name;			// get the name from the database

				int type = (int)DataBase.artifacts [key].type;			// get type from the database

				int weight = (int)DataBase.artifacts [key].weight;		// get weight from the database

				int quantity = saveData.artifacts[key];					// get quantity from inventory

				inventory.BuildSlot(key,type,weight,name,quantity,i);	// build a slot

                i++;
			}
		}

		if (saveData.vegetables.Count > 0) 
		{
			foreach (int key in saveData.vegetables.Keys) 
			{
				string name = DataBase.vegetables [key].name;		// get the name from the database

				int quantity = saveData.vegetables [key];			// get quantity from inventory

				inventory.BuildSlot(key,-1,1,name,quantity,i);		// build a slot

                i++;
			}
		}

        // Any empty inventory slot left?

        for (; i < INVENTORY_MAX_ITEMS; i++)
        {
            inventory.BuildSlot(-1, -3, 0, "Empty", 0, i);				// build a slot (id, type, weight, name, quantity)
        }


        /*
		if (saveData.compounds.Count > 0) 
		{
			foreach (string name in saveData.compounds.Keys) 
			{
				inventory.BuildSlot(-1,-2,1,name,1);				// build a slot (id, type, weight, name, quantity)
			}
		}

		if (saveData.artifacts.Count == 0 && saveData.vegetables.Count == 0 && saveData.compounds.Count == 0) 
		{
			inventory.BuildNoProduce ();		// Nothing to show inside Inventory
		}
                
         */

    }


    // Build Inventory section for SILO contents only:

    public void BuildInventorySiloMenu()
    {
        ShowSALLYSubPanel();

        sallyPanels[3].SetActive(true);

        Inventory inventory = sallyPanel.GetComponentInChildren<Inventory>();

        if (!inventory)
            return;

        //      inventory.DestroySlots();       // Destroy current menu before rebuilding


        int i = 0;

        // Rebuild each slot :

        if (saveData.vegetables.Count > 0)
        {
            foreach (int key in saveData.vegetables.Keys)
            {
                string name = DataBase.vegetables[key].name;        // get the name from the database

                int quantity = saveData.vegetables[key];            // get quantity from inventory

                inventory.BuildSlot(key, -1, 1, name, quantity, i); // build a slot

                i++;
            }
        }

        // Any empty inventory slot left?

        for (; i < INVENTORY_MAX_ITEMS; i++)
        {
            inventory.BuildSlot(-1, -3, 0, "Empty", 0, i);				// build a slot (id, type, weight, name, quantity)
        }


        if (saveData.vegetables.Count == 0)
        {
            //         inventory.BuildNoProduce();     // No produce to show inside Inventory

            alertMessage = "No produce inside inventory.";
            ShowAlertMenu();
        }

    }


    // Build Silo section of AI Menu:

    public void BuildSiloMenu()
	{
        ShowSALLYSubPanel();

        sallyPanels[0].SetActive(true);


        SiloUI silo = sallyPanel.GetComponentInChildren<SiloUI>();


		if (!silo)
			return;


		silo.DestroyPanel ();		// Destroy current menu before rebuilding


		// Rebuild each line :

		if (saveData.silos.ContainsKey(activeSilo.id)) 
		{
			foreach (int key in saveData.silos[activeSilo.id].items.Keys) 
			{
				string name = DataBase.vegetables [key].name;		        // get name from database

				int quantity = saveData.silos[activeSilo.id].items[key];	// get quantity from save file

				silo.BuildPanel(name,quantity);			        			// build a slot
			}
		}
		else
			silo.BuildNoProduce();									// build a single line stating silo is empty
		
	}


	// Build Missions section of AI Menu:

	public void BuildMissionsMenu()
	{

		Mission mission = sallyPanel.GetComponentInChildren<Mission>();

		if (!mission)
			return;

		mission.DestroyPanel ();		// Destroy current menu before rebuilding


		UpdateMissionList ();

		// Rebuild each line :

		if (DataBase.missions.Count > 0) 
		{
			for (int key=0; key < DataBase.missions.Count; key++) 
			{
				if (DataBase.missions[key].unlocked && !saveData.missions[DataBase.missions [key].id])			// unlocked and not completed
				{
					string name = DataBase.missions [key].title;						// get the name from the database

//					bool completed = saveData.missions.ContainsKey(key) ? true : false;		// completed?

//					bool completed = saveData.missions[DataBase.missions [key].id];			// completed?

					mission.BuildPanel(key, name);					// build a line
				}
			}
		}
		else
			mission.BuildNoMissions();								// build a single line stating no missions found

	}


	// Update SaveData with all active Missions

	public void UpdateMissionList()
	{
		if (DataBase.missions.Count > 0) 
		{
			for (int i=0; i < DataBase.missions.Count; i++) 
			{
				if (DataBase.missions [i].unlocked && !saveData.missions.ContainsKey (i)) 		// unlocked & not yet in saved data
				{
					saveData.missions.Add (i, false);	// Add to missions list in SaveData
				}
			}

			StoreSaveData ();
		}

	}

	public void AddMissionSave(int _id)
	{

		if (!saveData.missions.ContainsKey (_id)) 		// unlocked & not yet in saved data
				{
					saveData.missions.Add (_id, false);	// Add to missions list in SaveData
				}

	}

	// Build Missions details of AI Menu:

	public void BuildMissionDetails(int _id)
	{

		Mission mission = sallyPanel.GetComponentInChildren<Mission>();

		if (!mission)
			return;

	//	mission.DestroyPanel ();		// Destroy current menu before rebuilding

		mission.BuildMissionDetails (DataBase.missions[_id].title, DataBase.missions[_id].description);

	}


	// Build Diary section of AI Menu:

	public void BuildDiaryMenu()
	{

		Diary mission = sallyPanel.GetComponentInChildren<Diary>();

		if (!mission)
			return;

		mission.DestroyPanel ();		// Destroy current menu before rebuilding



		// Rebuild each line :

		if (saveData.missions.Count > 0) 
		{
			int[] diary = saveData.missions.Keys.ToArray ();

			for (int i=0; i < diary.Length; i++) 
			{
				if (saveData.missions[diary[i]] == true)						// if mission completed
				{
					string name = DataBase.missions [diary[i]].title;			// get the title from database

					mission.BuildPanel(diary[i], name);							// build a line in diary
				}
			}
		}
		else
			mission.BuildNoMissions();								// build a single line stating no missions found

	}

	// Build Diary details of AI Menu:

	public void BuildDiaryDetails(int _id)
	{

		Diary mission = sallyPanel.GetComponentInChildren<Diary>();

		if (!mission)
			return;

		mission.DestroyPanel ();		// Destroy current menu before rebuilding

		mission.BuildMissionDetails (DataBase.missions[_id].title, DataBase.missions[_id].logCompleted);

	}


	bool TryAddVegetables(int itemID)
	{
		if (saveData.vegetables.ContainsKey(itemID)) 						// if we have this item already
		{
			if (saveData.vegetables [itemID] < INVENTORY_MAX_ITEMS) 
			{																// and if we have less than the slot capacity
				saveData.vegetables [itemID]++;								// add quantity

				StoreSaveData ();											// save inventory

				return true;												// return from here
			} 

			alertMessage = "The maximum number of items in a slot is " + INVENTORY_MAX_ITEMS.ToString ();

			return false;			// only one slot for each item ID
		}


		// Check maximum weight :

		if (saveData.weightTotal < INVENTORY_MAX_WEIGHT)		// if weight is below capacity
		{
			saveData.vegetables.Add (itemID, 1);	// add item

			++saveData.weightTotal;					// add 1 to total weight

			StoreSaveData();						// save inventory

			return true;							// return from here
		}
		else
			alertMessage = "The maximum weight you can carry with you is " + INVENTORY_MAX_WEIGHT.ToString();


		return false;
	}


	// Try to Add a compound to Inventory :

	bool TryAddCompounds(string name, bool hazardous)
	{
		if (saveData.compounds.ContainsKey(name)) 		// if we have this item already
		{
			alertMessage = "You are already carrying a sample of " + name;

			return false;			// only one slot for each item
		}


		// Check maximum weight :

		if (saveData.weightTotal < INVENTORY_MAX_WEIGHT)	// if weight is below capacity
		{
			saveData.compounds.Add (name, hazardous);		// add item

			++saveData.weightTotal;							// add 1 to total weight

			StoreSaveData();								// save inventory

			return true;									// return from here
		}
		else
			alertMessage = "The maximum weight you can carry with you is " + INVENTORY_MAX_WEIGHT.ToString();


		return false;
	}



	// Try to Add an Artifact to Inventory :


	bool TryAddArtifact(int itemID)
	{
		if (saveData.artifacts.ContainsKey(itemID)) 						// if we have this item already
		{
			if (saveData.artifacts [itemID] < INVENTORY_MAX_ITEMS) 
			{																// and if we have less than the slot capacity
				saveData.artifacts [itemID]++;								// add quantity

				StoreSaveData ();											// save inventory

				return true;												// return from here
			} 

			alertMessage = "The maximum number of items in a slot is " + INVENTORY_MAX_ITEMS.ToString ();

			return false;			// only one slot for each item
		}


		// count total weight :

		int totalWeight = saveData.weightTotal + (int)DataBase.artifacts[itemID].weight;


		if (totalWeight < INVENTORY_MAX_WEIGHT)		// if weight is below capacity
		{
			saveData.artifacts.Add (itemID, 1);					// add item

			saveData.weightTotal = totalWeight;					// set new weight

			StoreSaveData();									// save inventory

			return true;										// return from here
		}
		else
			alertMessage = "The maximum weight you can carry with you is " + INVENTORY_MAX_WEIGHT.ToString();


		return false;
	}


	// Move vegetables from inventory to the Silo :

	public void AddProduce(int itemID)
	{
        //	CloseMenus ();

        if (saveData.vegetables.ContainsKey (itemID))   // item is in inventory
		{
            if (saveData.silos.ContainsKey(activeSilo.id))   // active silo is in SaveData
            {
                activeSilo.silo = saveData.silos[activeSilo.id];   // get silo's saved items

                if (activeSilo.silo.items.ContainsKey(itemID))
                {                                                               // and if we have this item in the Silo
                    activeSilo.silo.items[itemID] += saveData.vegetables[itemID];          // add up vegetables to the Silo
                    saveData.silos[activeSilo.id] = activeSilo.silo;                        // and Save
                }
                else
                {
                    activeSilo.silo.items.Add(itemID, saveData.vegetables[itemID]);    // add new vegetables to the silo
                    saveData.silos[activeSilo.id] = activeSilo.silo;                    // and Save
                }
            }
            else
            {
                activeSilo.silo.items.Add(itemID, saveData.vegetables[itemID]);  // Add new produce
                saveData.silos.Add(activeSilo.id, activeSilo.silo);              // and Save
            }


            saveData.vegetables.Remove (itemID);						// remove produce from inventory

			--saveData.weightTotal;										// subtract 1 weight

			StoreSaveData ();											// save data to file

			BuildSiloMenu ();											// rebuild AI panel

			BuildInventoryMenu ();										// rebuild AI panel


			CheckMissionSilo (itemID, activeSilo.silo.items[itemID]);		// Check for mission and mark completed (with quantity)

		} 

		else 			// we have not this item in inventory
		{
			alertMessage = "Item not found in Inventory.";

			ShowAlertMenu ();
		}

	}

	public void UpdateBuilding(int _id, bool _enabled, Vector3 _position, Quaternion _rotation)
	{
		if (saveData.buildings.ContainsKey (_id)) 
		{
			saveData.buildings [_id].isEnabled = _enabled;
			saveData.buildings [_id].posX = _position.x;
			saveData.buildings [_id].posY = _position.y;
			saveData.buildings [_id].posZ = _position.z;
			saveData.buildings [_id].quaternion [0] = _rotation.x;
			saveData.buildings [_id].quaternion [1] = _rotation.y;
			saveData.buildings [_id].quaternion [2] = _rotation.z;
			saveData.buildings [_id].quaternion [3] = _rotation.w;
		}
		else
		{
			BuildingData building = new BuildingData ();

			building.isEnabled = _enabled;
			building.posX = _position.x;
			building.posY = _position.y;
			building.posZ = _position.z;
			building.quaternion [0] = _rotation.x;
			building.quaternion [1] = _rotation.y;
			building.quaternion [2] = _rotation.z;
			building.quaternion [3] = _rotation.w;

			saveData.buildings.Add (_id, building);
		}

		StoreSaveData ();

		UpdateSceneData();
	}


	// Inventory Slot button clicked :

	public void ClickSlot(int itemID)
	{
	//	AddProduce (itemID);		// move vegetables to the silo
	}

	public void ClickSeed(int itemID)
	{
		if (activePlot)
			
			activePlot.DoSeed (itemID);

		CloseMenus ();
	}

	public void ClickHarvest()
	{
		CloseMenus();

		if (activePlot) 
		{
			if (TryAddVegetables (activePlot.itemID)) 
			{
				activePlot.DoHarvest ();

				CheckMissionHarvest (activePlot.itemID);			// Check for mission and mark completed
			}
			else
				
				ShowAlertMenu();
		}
	}

	public void ClickSample(string name, bool hazardous)
	{
		CloseMenus();

		Debug.Log (name);

		if ( ! TryAddCompounds(name, hazardous) )
			
			ShowAlertMenu();
		
	}

	public void ClickCollect()
	{
		CloseMenus();

		if (activeArtifact) 
		{
			if (TryAddArtifact (activeArtifact.itemID)) 
			{
				activeArtifact.DoCollect ();

				CheckMissionCollect (activeArtifact.itemID);			// Check for mission and mark completed
			}
			else

				ShowAlertMenu();
		}
	}

	public void ShowAlertMenu()
	{
		CloseMenus();

		alertMenu.SetActive(true);

		alertMenu.GetComponentInChildren<Text>().text = alertMessage;
	}


	// UNLOCK MISSION IN DATABASE :

	public void UnlockMission(int _id)
	{

		if (_id == UNLOCK_FOG_MISSION_ID)
			
			fog.GetComponent<Fog> ().Activate ();       // Mission to activate fog

        if (_id == UNLOCK_TRAVEL_MISSION_ID)

            canTravel = true;                            // Mission to activate Time-travel


        DataBase.missions [_id].unlocked = true;

        if (!saveData.missions.ContainsKey(_id))

            saveData.missions.Add (_id, false);


		StoreSaveData ();
	}


	// CHECK MISSION COMPLETED IN SAVEDATA :

	public bool CheckMissionCompleted(int _id)
	{
		if (saveData.missions.ContainsKey(_id))
			
			if (saveData.missions [_id] == true)		// if value is "true" for key "_id"

			return true;


		return false;
	}


	// CHECK IF THERE'S AN ACTIVE MISSION FOR A COLLISION :

	public void CheckMissionCollision(int _id)
	{
		int[] activeMissions = saveData.missions.Keys.ToArray();

		for (int i=0; i < activeMissions.Length; i++)
		{
			if ( saveData.missions [activeMissions [i]] == false

				&& DataBase.missions [activeMissions [i]].type == DataBase.MissionType.collisionEnter

				&& ( DataBase.missions[activeMissions [i]].itemID == _id || _id == -1) ) 
			{
				MissionMarkCompleted (activeMissions [i]);
			}
		}
	}

	public void CheckMissionCollect(int _id)
	{
		int[] activeMissions = saveData.missions.Keys.ToArray();

		for (int i=0; i < activeMissions.Length; i++)
		{
			if ( saveData.missions [activeMissions [i]] == false

				&& DataBase.missions [activeMissions [i]].type == DataBase.MissionType.artifactCollected

				&& ( DataBase.missions[activeMissions [i]].itemID == _id || _id == -1) ) 
			{
				MissionMarkCompleted (activeMissions [i]);
			}
		}
	}

	public void CheckMissionHarvest(int _id)
	{
		int[] activeMissions = saveData.missions.Keys.ToArray();

		for (int i=0; i < activeMissions.Length; i++)
		{
			if ( saveData.missions [activeMissions [i]] == false

				&& DataBase.missions [activeMissions [i]].type == DataBase.MissionType.harvestProduce

				&& ( DataBase.missions[activeMissions [i]].itemID == _id || _id == -1) ) 
			{
				MissionMarkCompleted (activeMissions [i]);
			}
		}
	}

	public void CheckMissionSilo(int _id, int _quantity)
	{
		int[] activeMissions = saveData.missions.Keys.ToArray();

		for (int i=0; i < activeMissions.Length; i++)
		{
			if ( saveData.missions [activeMissions [i]] == false

				&& DataBase.missions [activeMissions [i]].type == DataBase.MissionType.siloContents

				&& ( DataBase.missions[activeMissions [i]].itemID == _id || _id == -1)
			
				&& DataBase.missions[activeMissions [i]].quantity == _quantity ) 
			{
				MissionMarkCompleted (activeMissions [i]);
			}
		}
	}

	public void CheckMissionScan(int _id)
	{
		int[] activeMissions = saveData.missions.Keys.ToArray();

		for (int i=0; i < activeMissions.Length; i++)
		{
			if ( saveData.missions [activeMissions [i]] == false

				&& DataBase.missions [activeMissions [i]].type == DataBase.MissionType.artifactScan

				&& ( DataBase.missions[activeMissions [i]].itemID == _id || _id == -1) ) 
			{
				MissionMarkCompleted (activeMissions [i]);
			}
		}
	}

	public void MissionMarkCompleted(int _id)
	{
		saveData.missions [_id] = true;

		StoreSaveData ();

		// Add pop up dialog with Mission update
	}


    // MANAGE ACTION TRIGGERS :

    public void TriggerAction(Activity.actionTrigger _doAction)
    {
        switch (_doAction)
        {
            case Activity.actionTrigger.emergencyAlarmOn:        // Turn emergency alarm on:
                foreach (Activity action in actionAlarm)
                    action.EmergencyAlarmOn();
                break;

            case Activity.actionTrigger.emergencyAlarmOff:       // Turn emergency alarm off:
                foreach (Activity action in actionAlarm)
                    action.EmergencyAlarmOff();
                break;

            case Activity.actionTrigger.escapeDoorLock:           // Lock escape door:
                foreach (Activity action in actionDoor)
                    action.DoorLock();
                break;

            case Activity.actionTrigger.escapeDoorUnlock:         // Unlock escape door:
                foreach (Activity action in actionDoor)
                    action.DoorUnlock();
                break;
        }
    }


}
