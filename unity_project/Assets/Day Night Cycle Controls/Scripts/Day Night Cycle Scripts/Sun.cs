/// <summary>
/// Sun.
/// This allows us to set the brightness of our sun in the sky.
/// </summary>

using UnityEngine;
using System.Collections;
[AddComponentMenu("Environments/Sun")]
public class Sun : MonoBehaviour 
{
	public float maxLightBrightness;
	public float minLightBrightness;
	
	public float maxFlareBrightness;
	public float minFlareBrightness;
	
	public bool giveLight = false;
	
	void Start()
	{
		if(GetComponent<Light>() != null)
			giveLight = true;
	}
	
}
