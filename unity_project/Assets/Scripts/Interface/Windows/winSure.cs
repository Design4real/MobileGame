using UnityEngine;
using System.Collections;
using System.Linq;
using System.Collections.Generic;

public class winSure : BaseWindow
{
	public static bool isFromLevel;
	public UITexture background;
	public Texture game;
	public Texture level;
	
	public GameObject textGame;
	public GameObject textLevel;
	
	void Awake()
	{
		if( isFromLevel )
		{
			textGame.SetActive( false );
			background.material.mainTexture = level;
		}
		else
		{
			textLevel.SetActive( false );
			background.material.mainTexture = game;
		}
	}
	
	protected override void Init ()
	{
		base.Init ();
		buttons[ 0 ].OnPressJoystick( false );
		buttons = buttons.OrderByDescending( a => a.gameObject.name ).ToList();
		MenuController.runtime.cursor.localPosition = MenuController.runtime.getCursorPos(buttons[0]);
		buttons[ 0 ].OnPressJoystick( true );
	}
	
	public void btnAkayClick( string btnName )
	{
		if( isFromLevel )
		{
			Application.LoadLevel( "Menu" );
		}
		else
		{
			Application.LoadLevel( "Exit" );
		}
	}
	
	public void btnCancelClick( string btnName )
	{
		if( isFromLevel )
		{
			MenuController.isInMenu = false;
			GameController.runtime.isPaused = false;
		}
		InterfaceController.ClosePopUP();
	}



	void FixedUpdate()
	{
		bool menuOk = Common.input.GetButtonDown(SSSInput.InputType.Ok);
		if( menuOk && menuOk != MenuController.prevMenuOk )
		{
			Debug.Log(buttons[curButton].name);
			buttons[curButton].OnClick();
			Input.ResetInputAxes();
		}
		MenuController.prevMenuOk = menuOk;
	}
}
