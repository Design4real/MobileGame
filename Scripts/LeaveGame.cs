using UnityEngine;
using System.Collections;

public class LeaveGame : MonoBehaviour 
{
	public float timeToLeave = 5f;
	float curTime;

	// Use this for initialization
	void Start () 
	{
		curTime = 0;
	}
	
	// Update is called once per frame
	void Update () 
	{
		curTime += Time.deltaTime;
		if( curTime > timeToLeave )
		{
			Application.Quit();
		}
	}
	
	void FixedUpdate()
	{
		if( Input.anyKey )
		{
			Application.Quit();
		}
	}
}
