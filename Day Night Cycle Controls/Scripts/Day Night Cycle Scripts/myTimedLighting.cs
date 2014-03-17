/// <summary>
/// Timed lighting.
/// This listens for a message from the GameTime.cs to tell it weather it is time for it to be on or off.
/// </summary>
using UnityEngine;
using System.Collections;


public class myTimedLighting : MonoBehaviour {
	
	public float startintensity = 0;
	public float targetintensity = 4.67f;
	public float ftime = 3;
//	private bool finished = true;
//	private bool a;
//
	void Update()
	{
		Debug.Log (GetComponent<Light>().intensity);
	}

	public void OnEnable()
	{
		Messenger<bool>.AddListener("Morning Time", OnToggleLight);
	}
	
	public void OnDisable()
	{
		
		Messenger<bool>.RemoveListener("Morning Time", OnToggleLight);
	}
	
	private void OnToggleLight(bool b)
	{
		if(b)
		{
			
			//GetComponent<Light>().enabled = false;
			GetComponent<Light>().intensity = Mathf.Lerp(targetintensity, startintensity, ftime*Time.deltaTime);
			Debug.Log (GetComponent<Light>().intensity);
//			if (GetComponent<Light>().intensity == startintensity) { finished = true;}
//			a = true;
		}
		
		else
		{
			//GetComponent<Light>().enabled = true;
			GetComponent<Light>().intensity = Mathf.Lerp(startintensity, targetintensity, ftime*Time.deltaTime);
//			if (GetComponent<Light>().intensity == targetintensity) { finished = true;}
//			a = false;


			
		}
	}
}
