using UnityEngine;
using System.Collections;
using System;

public class winGame : BaseWindow
{
	public static winGame runtime;
	
	public UILabel lifeLabel;
	public UILabel timeLabel;
	public BaseButton[] mode_btns;
	public GameObject minigameTutorial1;
	public GameObject minigameTutorial2;
	public GameObject pause;

	public GameObject special_custom;
	public GameObject special_xbox;
	
	bool minigameOpened = false;
	bool prevTutorial;
	
	public static Transport transport
	{
		get
		{
			return GameController.runtime.curTransport;

		}
	}
	
	
	
	void Awake()
	{
		runtime = this;
		minigameOpened = false;

		GameController.runtime.screen_cutscene.gameObject.SetActive (true);
		CFInput.ctrl = GameController.runtime.screen_cutscene.GetComponent<TouchController>();
		special_custom.gameObject.SetActive(MenuController.isCustomInputInUse);
		special_xbox.gameObject.SetActive(!MenuController.isCustomInputInUse);
	}
	
	protected override void Start()
	{
		//OpenMinigame();
		base.Start ();
		
		ChangeTime();
		ModeButtonsChange();
	}

	protected override void Update ()
	{
		base.Update();
		GameController.runtime.anotherTransportParked ();
		if( !MenuController.runtime.isPaused )
		{
			ChangeTime();
		}
		
		if( ( GameController.runtime.curState != GameController.GameState.miniGame1 && GameController.runtime.curState != GameController.GameState.miniGame2 ) && minigameOpened )
		{
			CloseMinigame();
		}
		
		if( ( GameController.runtime.curState == GameController.GameState.miniGame1 || GameController.runtime.curState == GameController.GameState.miniGame2 ) && !minigameOpened )
		{
			OpenMinigame();
		}
		
		if( GameController.runtime.curState == GameController.GameState.loose || GameController.runtime.curState == GameController.GameState.win )
		{
			OpenWin();
		}
	}
	
	void FixedUpdate()
	{
		if( !MenuController.isInMenu && 
			GameController.runtime.curState != GameController.GameState.cutScene1 && 
			GameController.runtime.curState != GameController.GameState.miniGame2 && 
			GameController.runtime.curState != GameController.GameState.miniGame1 )
		{

			bool tutorial = Common.input.GetButtonDown(SSSInput.InputType.Tutorial);
			if( tutorial && tutorial != prevTutorial )
			{
				MenuController.isInMenu = true;
				GameController.runtime.isPaused = true;
				InterfaceController.openPopUp( MenuController.runtime.win_Info );
			}
			prevTutorial = tutorial;
			bool menuCancel = Common.input.GetButtonUp(SSSInput.InputType.Cancel);
			if( menuCancel && menuCancel != MenuController.prevMenuCancel)
			{
				GameController.runtime.screen_maingame.gameObject.SetActive (false);
				GameController.runtime.screen_cancel.gameObject.SetActive (true);
				CFInput.ctrl = GameController.runtime.screen_cancel.GetComponent<TouchController>();
				MenuController.isInMenu = true;
				winSure.isFromLevel = true;
				GameController.runtime.isPaused = true;
				InterfaceController.openPopUp( MenuController.runtime.win_Sure );
			}
			MenuController.prevMenuCancel = menuCancel;
		}
		if( GameController.runtime.curState == GameController.GameState.miniGame1 && MenuController.isInMenu == false )
		{
			bool menuOk = Common.input.GetButtonUp(SSSInput.InputType.Ok);
			if( menuOk  && menuOk != MenuController.prevMenuOk )
			{
				GameController.runtime.skipMiniGame1();
			}
			MenuController.prevMenuOk = menuOk;
			
			bool menuCancel = Common.input.GetButtonDown(SSSInput.InputType.Cancel);
			if( menuCancel && menuCancel != MenuController.prevMenuCancel)
			{
				GameController.runtime.screen_minigame.gameObject.SetActive (false);
				GameController.runtime.screen_cancel.gameObject.SetActive (true);
				CFInput.ctrl = GameController.runtime.screen_cancel.GetComponent<TouchController>();
				MenuController.isInMenu = true;
				winSure.isFromLevel = true;
				GameController.runtime.isPaused = true;
				InterfaceController.openPopUp( MenuController.runtime.win_Sure );

			}
			MenuController.prevMenuCancel = menuCancel;
			
		}
		if( GameController.runtime.curState == GameController.GameState.miniGame2 && MenuController.isInMenu == false)
		{
			bool menuOk = Common.input.GetButtonDown(SSSInput.InputType.Ok);
			if( menuOk && menuOk != MenuController.prevMenuOk )
			{
				GameController.runtime.curState = GameController.GameState.win;
			}
			MenuController.prevMenuOk = menuOk;
			
			bool menuCancel = Common.input.GetButtonDown(SSSInput.InputType.Cancel);
			if( menuCancel && menuCancel != MenuController.prevMenuCancel )
			{
				GameController.runtime.screen_minigame2.gameObject.SetActive (false);
				GameController.runtime.screen_cancel.gameObject.SetActive (true);
				CFInput.ctrl = GameController.runtime.screen_cancel.GetComponent<TouchController>();
				MenuController.isInMenu = true;
				winSure.isFromLevel = true;
				GameController.runtime.isPaused = true;
				InterfaceController.openPopUp( MenuController.runtime.win_Sure );
			}
			MenuController.prevMenuCancel = menuCancel;
		}
	}
	
	public void ChangeLife()
	{
		//lifeLabel.text = GameController.runtime.LivesCount.ToString();
	}
	
	public void ChangeTime()
	{
		timeLabel.text = GameController.runtime.levelTransport.timeLeft;
	}
	
	public void btnRMClick( string btnName )
	{
		if (transport.cams.curCameraInd != 0)
		{
			transport.cams.curCameraInd = 0;
			transport.changeCam();
		}
	}
	
	public void btnModeClick( string btnName )
	{
		if( transport == null )
		{
			return;
		}
		
		int modeNum = int.Parse( btnName.Substring( btnName.IndexOf( "_" ) + 1, 1 ) ) - 1;
		
		if( transport.curModeInd != modeNum )
		{
			mode_btns[ transport.curModeInd ].enabledSpriteName = mode_btns[ transport.curModeInd ].disabledSpriteName;
			mode_btns[ transport.curModeInd ].ApplySprite();
			mode_btns[ transport.curModeInd ].label.color = mode_btns[ transport.curModeInd ].nonActiveColor;
			mode_btns[ transport.curModeInd ].transform.FindChild("selection").gameObject.SetActive(false);
			
			transport.curModeInd = modeNum;
			transport.changeMode( transport.curMode );
			mode_btns[ transport.curModeInd ].enabledSpriteName = mode_btns[ transport.curModeInd ].pressedSpriteName;
			mode_btns[ transport.curModeInd ].ApplySprite();
			mode_btns[ transport.curModeInd ].label.color = mode_btns[ transport.curModeInd ].activeColor;
			mode_btns[ transport.curModeInd ].transform.FindChild("selection").gameObject.SetActive(true);
		}
	}
	
	public void btnSkipClick( string btnName )
	{
		/*
		GameController.runtime.parkingTransports.ForEach(a => a.enabled = false);
		GameController.runtime.parkingTransports.ForEach(a => a.reached = true);
		GameController.runtime.showMiniGameResultsTime = -1;
		*/
		OpenWin();
	}
	
	private void OpenMinigame()
	{
		pause.SetActive( true );
		pause.GetComponent< Blinking >().Enable();
		minigameOpened = true;
		
		if( GameController.runtime.curState == GameController.GameState.miniGame1 )
		{
			minigameTutorial1.SetActive( true );
			minigameTutorial2.SetActive( false );
		}
		if( GameController.runtime.curState == GameController.GameState.miniGame2 )
		{
			minigameTutorial2.SetActive( true );
			minigameTutorial1.SetActive( false );
		}
	}
	
	public void CloseMinigame()
	{
		pause.SetActive( false );
		minigameOpened = false;
		minigameTutorial1.SetActive( false );
		minigameTutorial2.SetActive( false );
	}
	
	public void ModeButtonsChange()
	{
		if( transport == null )
		{
			return;
		}
		
		for( int i = 0; i < mode_btns.Length; i++ )
		{
			int mode = transport.curModeInd;
			if( i != mode )
			{
				if( mode_btns[ i ].enabledSpriteName == mode_btns[ i ].pressedSpriteName )
				{
					mode_btns[ i ].enabledSpriteName = mode_btns[ i ].disabledSpriteName;
					mode_btns[ i ].ApplySprite();
				}
				mode_btns[ i ].transform.FindChild("selection").gameObject.SetActive(false);
			}
			else
			{
				if( mode_btns[ i ].enabledSpriteName != mode_btns[ i ].pressedSpriteName )
				{
					mode_btns[ i ].enabledSpriteName = mode_btns[ i ].pressedSpriteName;
					mode_btns[ i ].ApplySprite();
				}
				mode_btns[ i ].transform.FindChild("selection").gameObject.SetActive(true);
			}
		}
	}
	
	public void OpenWin()
	{
		MenuController.isInMenu = true;
		InterfaceController.openPopUp( MenuController.runtime.win_Win );
	}
	
	public void btnMenuClick( string btnName )
	{
		Application.LoadLevel( "Menu" );
	}
}