using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Artifact : MonoBehaviour 
{
	public int itemID;				// item ID as found in DataBase

	public bool isCollectable;		// if not collectable, player should take only samples


	// Item's game object data:

	public GameObject artifact;

	public Vector3 position;

	public Quaternion rotation;



	void Awake () 
	{
		artifact = gameObject;				// define gameobject to current gameobject
		position = transform.position;		// define position to current position
		rotation = transform.rotation;		// define rotation to current rotation
	}


	// Set position and rotation :

	public Artifact (Vector3 _position, Quaternion _rotation) 
	{
	//	artifact = gameObject;
		position = _position;
		rotation = _rotation;
	}


	// Collect this item :

	public void DoCollect () 
	{
		Destroy (gameObject);		// delete game object
	}

}
