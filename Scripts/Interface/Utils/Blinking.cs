using UnityEngine;
using System.Collections;

public class Blinking : MonoBehaviour
{
	TweenAlpha[] tween;
	int curTween = 0;
	
	// Use this for initialization
	void Awake () 
	{
		tween = gameObject.GetComponents< TweenAlpha >();
		for( int i = 0; i < tween.Length; i++ )
		{
			tween[ i ].enabled = false;
		}
	}
	
	public void Enable()
	{
		curTween = 0;
		tween[ curTween ].enabled = true;
	}
	
	public void NextTween()
	{
		curTween++;
		if( curTween == tween.Length )
		{
			curTween = 0;
		}
		tween[ curTween ].Reset();
		tween[ curTween ].enabled = true;
	}
}
