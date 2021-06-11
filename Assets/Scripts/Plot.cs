using UnityEngine;

public class Plot : MonoBehaviour {

	public Spawn[] sprouts;			// each sprout under this plot area

	public float secondsToGrow;		// timer for growing vegetables

	public bool isSeeded;			// is this plot already seeded?

	public bool isGrown;			// are the seeds grown? (to stop timer)

	public int itemID;				// register seed ID


	void Start() 
	{
		sprouts = GetComponentsInChildren<Spawn>();
	}

	void Update()
	{
		if (isSeeded && !isGrown) 
		{
			secondsToGrow -= Time.deltaTime;		// counts each 1 second

			if (secondsToGrow < 0) 
			{										// when timer gets to 0 :
				isGrown = true;

				ReadyToHarvest ();					// plant is ready to harvest
			}
		}
	}

	public void Interact() 
	{
		if (isSeeded && !isGrown)
			
			GameManager.GM.ShowDataMenu(DataBase.vegetables[itemID].name, secondsToGrow);
		
		else if (isSeeded)
		
			GameManager.GM.ShowHarvestMenu (true);

		else

			GameManager.GM.ShowSproutsMenu (true);
	}

	public void DoSeed(int _itemID) 
	{
		if (isSeeded)
			
			return;


		isSeeded = true;

		itemID = _itemID;

		secondsToGrow = DataBase.vegetables [itemID].minutesToGrow * 60;		// register time it should take to grow

		foreach (Spawn s in sprouts) 
		{
			s.SpawnItem (itemID);
		}
	}

	void ReadyToHarvest()
	{
		foreach (Spawn s in sprouts) 
		{
			s.GrowItem (itemID);
		}
	}

	public void DoHarvest()
	{
		if (!isGrown)
			return;

		isSeeded = false;

		isGrown = false;

		foreach (Spawn s in sprouts) 
		{
			s.HarvestItem ();
		}
	}
}
