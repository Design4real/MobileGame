/// <summary>
/// Timed lighting.
/// This listens for a message from the GameTime.cs to tell it weather it is time for it to be on or off.
/// </summary>
using UnityEngine;
using System.Collections;
using Holoville.HOTween;

public class TimedLighting : MonoBehaviour {

	public float mydelay=1;

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
			TweenParms parms = new TweenParms();
			parms.Prop("intensity", 0); // Position tween
			parms.Ease(EaseType.EaseInBounce);
			parms.Delay (mydelay);
			HOTween.To(GetComponent<Light>(), 1, parms );
		}
		
		else
		{
			//GetComponent<Light>().enabled = true;

			TweenParms parms = new TweenParms();
			parms.Prop("intensity", 4.67); // Position tween
			parms.Ease(EaseType.EaseInBounce);
			parms.Delay (mydelay);
			HOTween.To(GetComponent<Light>(), 1, parms );
			
		}
	}
}
