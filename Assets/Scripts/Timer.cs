using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour 
{
	
	public float setTimer = 0f;

	public string setName;

	
	void FixedUpdate () 
	{
		if (setTimer > 0)
			
			setTimer -= Time.fixedDeltaTime;		// minus 1 per second

		else 
		{
			setTimer = 0;

			GetComponentInParent<RectTransform> ().gameObject.SetActive (false);
		}

		GetComponentInChildren<Text> ().text = "Time left to harvest " + setName + ": " + setTimer.ToString ("####") + " seconds.";
		
	}
}
