using System;
using System.Collections.Generic;

/// <summary>
/// 
/// SCENE DATA : One for each scene; saves scene's state (replaced and removed objects)
/// 
/// </summary>

[Serializable]
public class SceneData
{
	public Dictionary<int,bool> buildings = new Dictionary<int,bool>();		// id, data
}
