using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class ArrowInit : MonoBehaviour 
{
	public float startDelta;
	public GameObject[] arrows;
	public float emitDelay;
	
	float curTime;
	int curArrow;
	bool isForward = false;
	bool isBack = false;
	bool isDelay = false;
	bool[] isBackAnimating;
	TweenAlpha[] arrowsForward;
	TweenAlpha[] arrowsBackward;
	
	void Awake()
	{
		arrows = arrows.Where( a => a != null && a.GetComponents<TweenAlpha>().Length == 2 ).ToArray();
		arrowsBackward = new TweenAlpha[ arrows.Length ];
		arrowsForward = new TweenAlpha[ arrows.Length ];
		isBackAnimating = new bool[ arrows.Length ];
		
		for( int i = 0; i < arrows.Length; i++ )
		{
			isBackAnimating[ i ] = false;
			TweenAlpha[] arrAlpha = arrows[ i ].GetComponents< TweenAlpha >();
			for( int j = 0; j < 2 && j < arrAlpha.Length; j++ )
			{
				if( arrAlpha[ j ].tweenGroup == 1 )
				{
					arrowsForward[ i ] = arrAlpha[ j ];
				}
				else
				{
					arrowsBackward[ i ] = arrAlpha[ j ];
				}
			}
		}
		
		curTime = 0;
		isForward = true;
		isBack = false;
		isDelay = false;
		if (arrowsForward[0] != null)
		{
			arrowsForward[ 0 ].enabled = true;
		}
		curArrow = 1;
	}

	void Update()
	{
		curTime += Time.deltaTime;
		if( isDelay && curTime > emitDelay )
		{
			//Debug.Log( curTime );
			//Debug.Log( emitDelay );
			isDelay = false;
			isForward = true;
		}
		if( curArrow < arrowsForward.Length && curTime > startDelta && isForward )
		{
			arrowsForward[ curArrow ].Reset();
			isBackAnimating[ curArrow ] = false;
			arrowsForward[ curArrow++ ].enabled = true;
			curTime = 0;
			if( curArrow == arrowsForward.Length )
			{
				isForward = false;
				curArrow = 0;
			}
		}
		
		bool isAllHide = true;
		
		for( int i = 0; i < arrowsBackward.Length; i++ )
		{
			if( !arrowsForward[ i ].enabled && !isBackAnimating[ i ] )
			{
				arrowsBackward[ i ].Reset();
				arrowsBackward[ i ].enabled = true;
				isBackAnimating[ i ] = true;
			}
			if( isBackAnimating[ i ] && arrowsBackward[ i ].enabled )
			{
				isAllHide = false;
			}
		}
		
		if( isAllHide && !isForward && !isDelay && isBackAnimating[ isBackAnimating.Length - 1 ] )
		{
			//Debug.Log( "Hide" );
			isDelay = true;
			curTime = 0;
		}
	}
}