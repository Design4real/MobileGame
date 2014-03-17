/// <summary>
/// Game time.cs
/// 9/28/2012
/// David Flynn
/// 
/// This is responsible for keeping track of the time in game.  It will rotate the suns and moons in the sky according to the current  time in game.
/// This class will also change the sky from day to night as time passes in game.
/// </summary>


using UnityEngine;
using System.Collections;

public class GameTime : MonoBehaviour 
{
	public enum TimeOfDay
	{
		Idle,
		SunRise,
		SunSet
		
	}
	public Transform[] sun;  //an array to hold all your suns
	
	private Sun[] _sunScript;  // an array to hold all the Sun.cs scripts attached to our sun
	
	public float dayCycleInMinutes = 1;  // how many real time minutes an in game day will last
	
	public float sunRise;// the time of day that the sun rise begins.
	public float sunSet;// the time of day that it starts the sunset.
	public float skyboxBlendModifier;  //speed at which the skybox textures blend
	
	public Color ambLightMax;
	public Color ambLightMin;
	
	public float morningLight;
	public float nightLight;
	private bool _isMorning = false;
	
	//do fog the same way
	
	private TimeOfDay _tod;
	private float _noonTime;  //this is the time of day when it is noon
	
	private const float Second = 1;  //constant for for 1 second.  1 second is always 1 second.
	private const float Minute = 60 * Second;// constant for how many seconds in a Minute.
	private const float Hour = 60 * Minute;// constant for how many minutes in an hour.
	private const float Day = 24 * Hour; // constant for how many hours in a day.
	
	private float _dayCycleInSeconds; // the number of real time seconds in an game day.
	
	private const float Degrees_Per_Second = 360 / Day;//constant for how many degress we have to rotate per second a day to do 360 degrees.
	
	private float _degreeRoatiation;  // how many degress the sun/moon rotate each unit of time.
	
	private float _timeOfDay; // track the passage of time through out the day.
	
	private float _morningLength;
	private float _eveningLength; //time from noon to sunset
	

	// Use this for initialization
	void Start () 
	{
		
		_tod = TimeOfDay.Idle;
		_dayCycleInSeconds = dayCycleInMinutes * Minute;
		
		RenderSettings.skybox.SetFloat("_Blend",0);
		_sunScript = new Sun[sun.Length];
		
		for(int cnt = 0; cnt < sun.Length; cnt++)
		{
			Sun temp = sun[cnt].GetComponent<Sun>();
			
			if(temp == null)
			{
				Debug.LogWarning("Sun script not found.  Adding it.");
				sun[cnt].gameObject.AddComponent<Sun>();
				temp = sun[cnt].GetComponent<Sun>();
				
			}
			_sunScript[cnt] = temp;
		}
		_timeOfDay = 0;
		_degreeRoatiation = Degrees_Per_Second * Day / (_dayCycleInSeconds);
		
		sunRise *= _dayCycleInSeconds;
		sunSet *= _dayCycleInSeconds;
		_noonTime = _dayCycleInSeconds/2;
		
		_morningLength = _noonTime - sunRise;  // length of morning in seconds
		_eveningLength = sunSet - _noonTime;// length of evening in seconds
		morningLight *= _dayCycleInSeconds;
		nightLight *= _dayCycleInSeconds;
		
		
		
		SetupLighting(); // setup lighting to min light valuse to start with
		
		
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		//control the outside lights
		if(!_isMorning && _timeOfDay > morningLight && _timeOfDay < nightLight)
			
		{
			_isMorning = true;
			Messenger<bool>.Broadcast("Morning Time",true);
			
		}
		else if(_isMorning && _timeOfDay > nightLight)
		{
			_isMorning = false;
			Messenger<bool>.Broadcast("Morning Time",false);
			
			
		}
		
		for(int cnt = 0; cnt < sun.Length; cnt++)
		sun[cnt].Rotate (new Vector3(_degreeRoatiation, 0 ,0 ) * Time.deltaTime);
		
		if(_timeOfDay > sunRise && _timeOfDay < _noonTime)
			
		{
			AdjustLighting(true);
		}
		else if (_timeOfDay > _noonTime && _timeOfDay < sunSet)
			
		{
			AdjustLighting(false);
		}
		
		_timeOfDay += Time.deltaTime;
		
		if(_timeOfDay > _dayCycleInSeconds)
			_timeOfDay -= _dayCycleInSeconds;
		//Debug.Log (_timeOfDay);
		
		if(_timeOfDay > sunRise && _timeOfDay< sunSet  && RenderSettings.skybox.GetFloat("_Blend") < 1)
		{
			_tod = GameTime.TimeOfDay.SunRise;
			BlendSkybox();
		}
		
		else if(_timeOfDay > sunSet && RenderSettings.skybox.GetFloat("_Blend") > 0)
		{
			_tod = GameTime.TimeOfDay.SunSet;
			BlendSkybox();
		}
		
		else
		{
			_tod = GameTime.TimeOfDay.Idle;
		}
			
		
	}
		private void BlendSkybox()
	{
		float temp = 0;
		
		switch(_tod)
		{
		case TimeOfDay.SunRise:
			temp = (_timeOfDay - sunRise )/_dayCycleInSeconds * skyboxBlendModifier;
			break;
		case TimeOfDay.SunSet:
			temp = (_timeOfDay - sunSet )/_dayCycleInSeconds * skyboxBlendModifier;
			temp = 1 - temp;
			break;
		}
	
	
		
		
		RenderSettings.skybox.SetFloat("_Blend", temp);
		
		//Debug.Log (temp);
	}
	
	private void SetupLighting()
	{
		RenderSettings.ambientLight = ambLightMin;
		
		for(int cnt = 0; cnt < _sunScript.Length; cnt++)
			
		{
			if(_sunScript[cnt].giveLight)
			{
				sun[cnt].GetComponent<Light>().intensity = _sunScript[cnt].minLightBrightness;
			}
		}
	}
	
	private void AdjustLighting(bool brighten)
		
	{
		float pos = 0;
		
		if(brighten)
		{
			pos = (_timeOfDay - sunRise) / _morningLength; //get the postion of the sun in the morning
				
		}
			
		else
		{
			pos = (sunSet - _timeOfDay) / _eveningLength; //get the postion of the sun in the evening
			
			
		}
		
		RenderSettings.ambientLight = new Color(ambLightMin.r + ambLightMax.r * pos, ambLightMin.g + ambLightMax.g * pos, ambLightMin.b + ambLightMax.b * pos);
		
		for(int cnt = 0; cnt < _sunScript.Length; cnt++)
			{
				if(_sunScript[cnt].giveLight)
				{
					
					_sunScript[cnt].GetComponent<Light>().intensity = _sunScript[cnt].maxLightBrightness * pos;	
				}
			}
	}
}
