using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Collections;
using System.Linq;

public class BaseWindow : MonoBehaviour 
{
	//[HideInInspector]
	public List<BaseButton> buttons;
	[HideInInspector]
	public int curButton;
	
	// Use this for initialization
	protected virtual void Start ()
	{
		Init();
	}
	
	protected virtual void Init()
	{
		buttons.Clear();
		foreach (BaseButton btn in GetComponentsInChildren<BaseButton>())
		{
			buttons.Add( btn );
			btn.event_buttonClick +=onButtonClick;
		}
		
		if (this is winWin)
		{
			buttons = buttons.OrderBy( a => a.gameObject.name ).ToList();
		}
		
		if( MenuController.isInMenu && buttons.Count > 0 )
		{
			MenuController.runtime.cursor.localPosition = MenuController.runtime.cursor.localPosition = MenuController.runtime.getCursorPos(buttons[0]);
			buttons[ 0 ].OnPressJoystick( true );
			curButton = 0;
		}
	}
	
	protected virtual void Update()
	{
		
	}
	
	// обработчик нажатия кнопок.
	public void onButtonClick( BaseButton btn )// btnName )
	{
		switch (btn.name)
		{	
			// Ищет метод по названию нажатой кнопки.
			default:
			{
				System.Type curType = GetType();
		 		MethodInfo mi = null;
		 	 	while (mi == null && curType != typeof(BaseWindow).BaseType)
		 	 	{
		 	 	 	mi = curType.GetMethod(btn.name.Substring(0,btn.name.IndexOf("_")) + "Click", BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
		  	 		curType = curType.BaseType;
		 	 	} 
				if (mi != null)
				{
					mi.Invoke(this, new object[]{btn.name});
				}
				break;
			}
		}
	}
	
	public void btnInfoClick( string btnName )
	{
		return;
		Time.timeScale = 0;
		MenuController.runtime.isPaused = true;
		InterfaceController.openPopUp( MenuController.runtime.win_Info );
		//InterfaceController.OpenWindow( MenuController.runtime.win_Info );
	}
}

