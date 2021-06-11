using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 
/// COLLECTABLES : Located in an empty game object containing all scene's collectables
/// 
/// </summary>

public class Collectables : MonoBehaviour 
{
	void Start () 
	{
//		Invoke ("UpdateCollectibles", 0.1f);
	}

	void UpdateCollectibles () 
	{
		Artifact[] artifacts = GetComponentsInChildren<Artifact> ();		// Get all Artifacts inside Collectables

		// Delete all collected objects from the scene :

		foreach (Artifact artifact in artifacts)
			if (GameManager.collectedArtifacts.Contains (artifact.itemID))
				artifact.DoCollect ();
	}
}
