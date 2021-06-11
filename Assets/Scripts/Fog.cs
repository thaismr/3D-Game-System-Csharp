using UnityEngine;

public class Fog : MonoBehaviour 
{
	public const string TAG = "Fog";

	public GameObject player;

	Transform target; 				// the player's transform
	float moveSpeed = 1.5f; 		// move speed
	float rotationSpeed = 1.5f; 	// speed of turning

	Transform myTransform; 			// fog's transform

	bool isActive = false;			// begin game inactive

	void Awake()
	{
		myTransform = transform; 	// cache transform data for easy access/preformance
	}

	void OnEnable()
	{
		target = player.transform; 	// target the player
	}

	void Update () 
	{
		if (isActive) 
		{
			// rotate to look at the player
			myTransform.rotation = Quaternion.Slerp (myTransform.rotation, Quaternion.LookRotation (target.position - myTransform.position), rotationSpeed * Time.deltaTime);

			// move towards the player
			myTransform.position += myTransform.forward * moveSpeed * Time.deltaTime;
		}
	}

	public void Activate()
	{
		isActive = !isActive;
	}
}
