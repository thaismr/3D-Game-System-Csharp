using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 
/// BUILDING DATA : Structure for buildings' saved data
/// 
/// </summary>


[System.Serializable]		// Serializable so we can build a List of items
public class BuildingData
{
	public bool isEnabled;

	public float posX;

	public float posY;

	public float posZ;

	public float[] quaternion = new float[4];	// rotation
}
