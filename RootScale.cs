using UnityEngine;
using System.Collections;

public class RootScale : MonoBehaviour 
{
	Vector2 baseSize = new Vector2(1920, 1080);
	
	void Start()
	{
		float scale = 1;
		float baseRatio = baseSize.x / baseSize.y;
		float curRatio = Screen.width / Screen.height;
		if (baseRatio > curRatio)
		{
			scale = Screen.width / baseSize.x;
		}
		else
		{
			scale = Screen.height / baseSize.y;
		}
	
		//transform.localScale = new Vector3(Screen.width / baseSize.x, Screen.height / baseSize.y, 1);
		transform.localScale = new Vector3(scale, scale, 1);
	}
}
