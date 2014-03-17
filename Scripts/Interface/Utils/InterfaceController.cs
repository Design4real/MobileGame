using UnityEngine;
using System.Collections;

public class InterfaceController 
{
	private static bool popwindow = false;
	private static Transform win;
	private static Transform prev_window;
	public static Transform curWindow;
	private static Transform popupWindow;
	private static Transform win_add;
	
	public static void ClosePopUP()
	{
		MenuController.runtime.cursor.parent = MenuController.runtime.win_previous;
		MenuController.runtime.win_cur = MenuController.runtime.win_previous;
		GameObject.DestroyImmediate( popupWindow.gameObject );
		if( MenuController.isInMenu )
		{
			//MenuController.runtime.cursor.localPosition = MenuController.runtime.win_cur.GetComponent< BaseWindow >().buttons[ 0 ].transform.localPosition + new Vector3( MenuController.runtime.win_cur.GetComponent< BaseWindow >().buttons[ 0 ].sprite.transform.localScale.x / 2 - MenuController.runtime.cursor.localScale.x / 2, -MenuController.runtime.win_cur.GetComponent< BaseWindow >().buttons[ 0 ].sprite.transform.localScale.y / 2.5f, -10 );
			MenuController.runtime.cursor.localPosition = MenuController.runtime.getCursorPos(MenuController.runtime.win_cur.GetComponent< BaseWindow >().buttons[ 0 ]);
			MenuController.runtime.win_cur.GetComponent< BaseWindow >().buttons[ 0 ].OnPressJoystick( true );
			MenuController.runtime.win_cur.GetComponent< BaseWindow >().curButton = 0;
		}
		else
		{
			GameObject.DestroyImmediate( MenuController.runtime.cursor.gameObject );
		}
		curWindow = null;
	}
	
	public static void openPopUp( Transform openWindow )
	{
		win = openWindow;
		popwindow = true;
		changeWindow();
	}
	
	public static void OpenWindow( Transform openWindow )
	{
		win = openWindow;
		changeWindow();
	}
	
	public static void OpenPreviousWindow()
	{
		win = prev_window;
		changeWindow();
	}
	
	private static void changeWindow()
	{
		if ( win != curWindow )
		{
			if ( !popwindow )
			{
				int count = MenuController.runtime.gameObject.transform.GetChildCount();
				for( int i = count - 1; i >= 0; i-- )
				{
					GameObject.DestroyImmediate( MenuController.runtime.gameObject.transform.GetChild( 0 ).gameObject );
				}
			}
			
			win_add = GameObject.Instantiate( win ) as Transform;
			win_add.parent = MenuController.runtime.gameObject.transform;
			win_add.localScale = new Vector3( 1, 1, 1 );
			
			if (!popwindow)
			{
				win_add.localPosition = new Vector3 (0,0,win_add.position.z);
			}
			else
			{
				MenuController.runtime.win_previous = MenuController.runtime.win_cur;
				win_add.localPosition = new Vector3 (0,0,win_add.localPosition.z);
				popupWindow = win_add;
			}
			
			if( MenuController.isInMenu )
			{
				if( popwindow )
				{
					if( MenuController.runtime.cursor != null )
					{
						GameObject.DestroyImmediate( MenuController.runtime.cursor.gameObject );
					}
				}
				if( MenuController.runtime.cursor == null )
				{
					MenuController.runtime.cursor = GameObject.Instantiate( MenuController.runtime.cursorPrefab ) as Transform;
				}
				
				MenuController.runtime.cursor.parent = win_add;
				MenuController.runtime.cursor.localScale = new Vector3( MenuController.runtime.cursor.GetComponent< UISprite >().sprite.outer.width, MenuController.runtime.cursor.GetComponent< UISprite >().sprite.outer.height, 1 );
			}
			
			MenuController.runtime.win_cur = win_add;
			
			popwindow = false;
			prev_window = curWindow;
			curWindow = win;
		}
		
	}
}
