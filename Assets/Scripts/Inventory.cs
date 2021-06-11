using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 
/// INVENTORY MENU : Located in the inventory panel, instantiates a button slot for each item saved in inventory
/// 
/// </summary>

public class Inventory : MonoBehaviour 
{
	public GameObject _label;			// Label in case of no products to show
	
	public GameObject _slotButton;      // slot (button) to spawn

    public Sprite[] slotImage;

    public GameObject[] slotButton;

    public Text[] slotText;

	Slot slot;							// Slot class component

	Text text;                          // slot button Text component

    

    // Called once for each slot to build :

    public void BuildSlot(int id, int type, int weight, string name, int quantity, int i)
    {

        slotText[i].text = name;

        if (quantity != 0)
        {
            slotText[i].text += ": " + quantity.ToString();
        }

        if (type == -1)
        {
            slotText[i].text += "Kg";        // vegetables
        }


        // Set slot attributes :

        slot = slotButton[i].GetComponent<Slot>();

        slot.itemid = id;

        slot.type = type;


        // Image according to weight :

        switch (weight)
        {
            case 0:

            case 1:

                slotButton[i].GetComponent<Image>().sprite = slotImage[0];
                break;

            case 2:

                slotButton[i].GetComponent<Image>().sprite = slotImage[1];
                break;

            case 3:

                slotButton[i].GetComponent<Image>().sprite = slotImage[2];
                break;

            case 4:

                slotButton[i].GetComponent<Image>().sprite = slotImage[3];
                break;
        }
        

    }

    public void BuildSlotOld (int id, int type, int weight, string name, int quantity)
	{
		GameObject slotButton = Instantiate (_slotButton);			// clone slot button

		slotButton.transform.SetParent(transform, false);			// set this panel as parent (layout component should position it correctly)

		slotButton.GetComponentInChildren<Text> ().text = name + ": " + quantity.ToString();


		// Set slot attributes :

		slot = slotButton.GetComponent<Slot> ();

		slot.itemid = id;

		slot.type = type;


		// Color according to weight :

		switch (weight) 
		{
		case 0:

		case 1:

			slotButton.GetComponent<Image> ().color = Color.green;

			break;

		case 2:

			slotButton.GetComponent<Image> ().color = Color.yellow;

			break;

		case 3:

			slotButton.GetComponent<Image> ().color = Color.magenta;

			break;

		case 4:

			slotButton.GetComponent<Image> ().color = Color.red;

			break;
		}



	}


	// Destroy slots:		(called once before rebuilding)

	public void DestroySlots()
	{
		Slot[] slots = GetComponentsInChildren<Slot> ();		// get all Slots

		foreach (Slot slot in slots)							// destroy each one
		
			Destroy (slot.gameObject);


		// Destroy all text except Title :

		Text[] emptyLabel = GetComponentsInChildren<Text> ();

		foreach (Text empty in emptyLabel)
			if (empty.text != "Inventory")
				Destroy (empty.gameObject);
	}


	// No Produce to show:

	public void BuildNoProduce ()
	{
		GameObject label = Instantiate (_label);			// clone label

		label.GetComponentInChildren<Text> ().text = "No produce in inventory.";

		label.transform.SetParent(transform, false);			// set this panel as parent
	}

}
