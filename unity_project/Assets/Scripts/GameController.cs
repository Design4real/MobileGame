using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class GameController : MonoBehaviour 
{
	public Transform UIPrefab;

	public int transportNumber;
	
	public static GameController runtime;
	
	public Transform[] mainGame_Objects;
	
	public LevelTransport levelTransport;
	
	public enum GameState { /*cutScene0,*/ miniGame1, cutScene1, mainGame, miniGame2, win, loose };
	
	public bool isPaused = false;
	
	//public Transform cutScene0;
	public Transform cutScene1;
	
	public Transform hitParticlesPrefab;
	private int livesCount;
	
	public int LivesCount
	{
		get{ return livesCount; }
		set
		{
			livesCount = value;
			winGame.runtime.ChangeLife();
			if( livesCount == 0 )
			{
				//curState = GameState.loose;
			}
		}
	}


	public List<Transform> miniGame1_objects;
	public ParkingTransport curParkingTransport;
	public List<ParkingTransport> parkingTransports;
	
	public Transport curTransport
	{
		get
		{
			if (curState == GameState.miniGame1)
			{
				return curParkingTransport;
			}
			else
			{
				return levelTransport;
			}
		}
	}
	
	public GameState curState;
	
	float maxIdleTime = 1.5f * 60;
	float idleTimeCounter;
	
	void Awake()
	{
		runtime = this;
		if (UIPrefab != null && GlobalDataKeeper.isGame)
		{
			Instantiate(UIPrefab);
		}
		livesCount = 5;
		
		curState = GameState.miniGame1;
		curParkingTransport = parkingTransports.FirstOrDefault(a => !a.reached);
	}

	// Use this for initialization
	void Start () 
	{
		idleTimeCounter = 0;
	}
	
	int tmp = 0;
	
	// Update is called once per frame
	void Update () 
	{
		//Debug.Log (curState + "___________________________________________");
		
		if (curState == GameState.loose && levelTransport.enabled)
		{
			levelTransport.enabled = false;
		}
		
		if (curState == GameState.miniGame1)
		{
			int transportModeInd = -1;
			if (curParkingTransport != null && curParkingTransport.reached)
			{
				transportModeInd = curParkingTransport.curModeInd;
				curParkingTransport.enabled = false;
				curParkingTransport = null;
			}
			if (curParkingTransport == null || (!curParkingTransport.enabled && !curParkingTransport.reached))
			{
				curParkingTransport = parkingTransports.FirstOrDefault(a => !a.reached);
				if (curParkingTransport == null)
				{
					skipMiniGame1();
				}
				else
				{
					curParkingTransport.enabled = true;
					if (transportModeInd != -1)
					{
						curParkingTransport.curModeInd = transportModeInd;
						curParkingTransport.changeMode(curParkingTransport.curMode);
					}
					if( winGame.runtime != null )
					{
						winGame.runtime.ModeButtonsChange();
					}
				}
			}
		}
		
		if (curState == GameState.cutScene1)
		{
			bool cutSceneEnded = true;
			foreach (Transform curTransform in cutScene1)
			{
				if (curTransform.animation != null && curTransform.animation.isPlaying)
				{
					cutSceneEnded = false;
					break;
				}
			}
			if (cutScene1.animation != null && cutScene1.animation.isPlaying)
			{
				cutSceneEnded = false;
			}
			if (cutSceneEnded)
			{
				Destroy(cutScene1.gameObject);
				curState = GameState.mainGame;
				levelTransport.gameObject.SetActive(true);
			}
		}
		
		if ((!Input.anyKey) &&
			(Mathf.Approximately(Common.input.GetAxis(SSSInput.InputType.Horizontal), 0)) &&
			(Mathf.Approximately(Common.input.GetAxis(SSSInput.InputType.Vertical), 0)))
		{
			idleTimeCounter += Time.deltaTime;
		}
		else
		{
			idleTimeCounter = 0;
		}
		if (idleTimeCounter >= maxIdleTime)
		{
			MenuController.isFirstLoad = true;
			Application.LoadLevel(0);
		}
	}



	public void anotherTransportParked()

	{



	}

	
	public void FixedUpdate()
	{
		if( curState == GameState.cutScene1 || curState == GameState.miniGame2 )
		{
			bool menuCancel = Common.input.GetButtonDown(SSSInput.InputType.Ok);
			if( menuCancel && menuCancel != MenuController.prevMenuCancel || Input.anyKey )
			{
				skipCutScene();
			}
			MenuController.prevMenuCancel = menuCancel;
		}
	}

	public void showMinigameStats()
	{
		//Debug.Log(parkingTransports.size);
		//Debug.Log("einer_geparkt_______"+parkedNum+"__________________________________________________");
		//parkedNum++;
	}


	public void skipMiniGame1()
	{
		curState = GameState.cutScene1;
		foreach (Transform curTrans in miniGame1_objects)
		{
			Destroy(curTrans.gameObject);
		}
		cutScene1.gameObject.SetActive(true);
		Input.ResetInputAxes();
	}
	
	public bool skipCutScene()
	{
		if (curState == GameState.cutScene1)
		{
			Destroy(cutScene1.gameObject);
			curState = GameState.mainGame;
			levelTransport.gameObject.SetActive(true);
			return true;
		}
		return false;		
	}
}
