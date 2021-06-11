using UnityEngine;

public class Gates : MonoBehaviour 
{
	Animator anim;

	void Start () 
	{
		anim = GetComponent<Animator> ();
	}
	
	void OnTriggerEnter (Collider other) 
	{
		if (other.GetComponent<Player> ())
			anim.SetTrigger ("OpenDoor");
	}

	void OnTriggerExit (Collider other) 
	{
		if (other.GetComponent<Player> ())
			anim.SetTrigger ("CloseDoor");
	}
}
