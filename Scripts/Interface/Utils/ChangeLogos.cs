using UnityEngine;
using System.Collections;

public class ChangeLogos : MonoBehaviour 
{
	UISprite[] logos;
	float curTime = 0;
	int curLogo = 0;
	public float changeTime = 4;
	
	// Use this for initialization
	void Start ()
	{
		logos = transform.GetComponentsInChildren< UISprite >();
		for( int i = 0; i < logos.Length; i++ )
		{
			logos[ i ].alpha = 0;
		}
		logos[ curLogo ].alpha = 1;
	}
	
	// Update is called once per frame
	void Update ()
	{
		curTime += Time.deltaTime;
		if( curTime > changeTime )
		{
			TweenAlpha[] tweens = logos[ curLogo ].transform.GetComponents< TweenAlpha >();
			for( int i = 0; i < 2; i++ )
			{
				if( tweens[ i ].tweenGroup == 1 )
				{
					tweens[ i ].Reset();
					tweens[ i ].enabled = true;
				}
			}
			curLogo++;
			if( curLogo == logos.Length )
			{
				curLogo = 0;
			}
			tweens = logos[ curLogo ].transform.GetComponents< TweenAlpha >();
			for( int i = 0; i < 2; i++ )
			{
				if( tweens[ i ].tweenGroup == 0 )
				{
					tweens[ i ].Reset();
					tweens[ i ].enabled = true;
				}
			}
			curTime = 0;
		}
	}
}
