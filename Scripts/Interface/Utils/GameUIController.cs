using UnityEngine;
using System.Collections;

public class GameUIController : MonoBehaviour 
{
	public static GameUIController runtime;
	public Transform root;
	
	void Awake()
	{
		runtime = this;
	}
	
	void Start()
	{
		GlobalDataKeeper.lives = 3;
		GlobalDataKeeper.curGasLeft = 1;
		GlobalDataKeeper.isPostGameCompleted = false;
		GlobalDataKeeper.isPreGameCompleted = false;
		GlobalDataKeeper.timeStart = Time.time;
		
		//Transform rootTrans = GameObject.Instantiate( root ) as Transform;
		GameObject.Instantiate( root );
	}
}