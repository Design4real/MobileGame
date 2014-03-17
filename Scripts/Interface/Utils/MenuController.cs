using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MenuController : MonoBehaviour 
{
	public static MenuController runtime;
	
	[HideInInspector]
	public string loadingLevel;
	public GameObject language;
	public GameObject panel;
	public Transform cursorPrefab; 
	
	public Transform win_MainMenu;
	public Transform win_LevelSelect;
	public Transform win_Info;
	public Transform win_Win;
	public Transform win_Game;
	public Transform win_pop_UpConfirm;
	public Transform win_Sure;
	
	[HideInInspector]
	public Transform win_previous;
	[HideInInspector]
	public Transform win_cur;
	
	[HideInInspector]
	public float scale;
	[HideInInspector]
	public static float scaleX;
	[HideInInspector]
	public static float scaleY;
	
	public float baseWidth;
	public float baseHeight;
	
	public UIFont[] font;
	[HideInInspector]
	public bool isButtonClicked = false;
	[HideInInspector]
	public bool isPaused = false;
	[HideInInspector]
	public bool isUsingJoystick = true;
	public static bool isInMenu = false;
	[HideInInspector]
	public Transform cursor;
	
	public static bool isFirstLoad = true;
	private Transform curHit;
	private bool wasBtnPressed = false;
	private float prevMenuSwich = 0;
	public static float prevMenuOk = 0;
	public static float prevMenuCancel = 0;
	
	public static bool isCustomInputInUse
	{
		get
		{
			return Common.selectedInputSourceType == SSSInput.InputSourceType.Custom;
		}
	}

	public Transform dbg_win_cur;
	public Transform dbg_win_prev;
	public Transform dbg_cursor;
	public bool dbg_isInMenu;
	public Transform dbg_IC_curWindow;
	
	void Awake()
	{
		Screen.showCursor = false;
		runtime = this;
		scale = (float)Screen.height / baseHeight;
		scaleY = scale;
		scaleX =(float)Screen.width / baseWidth;
		
		if( Application.loadedLevelName == "Menu" )
		{
			isInMenu = true;
			if (isFirstLoad)
			{
				InterfaceController.OpenWindow( win_MainMenu );
				isFirstLoad = false;
			}
			else
			{
				InterfaceController.OpenWindow( win_LevelSelect );
			}
			//GlobalDataKeeper.isGame = true;
			//Application.LoadLevelAdditive( "s1" );
		}
		else
		{
			isInMenu = false;
			InterfaceController.OpenWindow( win_Game );
		}
		
	}

	void OnGUI()
	{
		return;
		int cachedSize = GUI.skin.label.fontSize;
		Color cachedColor = GUI.color;
		Color cachedContentColor = GUI.contentColor;
		FontStyle cachedFontStyle = GUI.skin.label.fontStyle;
		
		GUI.color = Color.white;
		GUI.contentColor = Color.red;
		GUI.skin.label.fontSize = 30;
		GUI.skin.label.fontStyle = FontStyle.Bold;
		
		GUILayout.BeginVertical();
		GUILayout.Label ("win_cur: " + (dbg_win_cur == null ? "null" : dbg_win_cur.ToString()), GUILayout.Height(40), GUILayout.Width(1000));
		GUILayout.Space(1);
		GUILayout.Label ("win_previous: " + (dbg_win_prev == null ? "null" : dbg_win_prev.ToString()), GUILayout.Height(40), GUILayout.Width(1000));
		GUILayout.Space(1);
		GUILayout.Label ("cursor: " + (dbg_cursor == null ? "null" : dbg_cursor.ToString()), GUILayout.Height(40), GUILayout.Width(1000));
		GUILayout.Space(1);
		GUILayout.Label ("isInMenu: " + dbg_isInMenu, GUILayout.Height(40), GUILayout.Width(1000));
		GUILayout.Space(1);
		GUILayout.Label ("IC_curWin: " + (dbg_IC_curWindow == null ? "null" : dbg_IC_curWindow.ToString()), GUILayout.Height(40), GUILayout.Width(1000));
		GUILayout.Space(1);
		GUILayout.EndVertical();
		
		GUI.skin.label.fontSize = cachedSize;
		GUI.color = cachedColor;
		GUI.contentColor = cachedContentColor;
		GUI.skin.label.fontStyle = cachedFontStyle;
	}

	void Update()
	{
		/*dbg_win_cur = win_cur;
		dbg_win_prev = win_previous;
		dbg_cursor = cursor;
		dbg_isInMenu = isInMenu;
		dbg_IC_curWindow = InterfaceController.curWindow;*/

		Ray ray = transform.parent.camera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		if( Physics.Raycast(ray, out hit) )
		{
			if( hit.collider.GetComponent< BaseButton >() != null )
			{
				if( hit.collider.transform != curHit )
				{
					MouseOver( hit.collider.transform );
				}
				else
				{
					MouseOverSameButton();
				}
			}
			else
			{
				MouseOut();
				MouseUp();
			}
		}
		else
		{
			MouseOut();
			MouseUp();
		}
	}
	
	void FixedUpdate()
	{
		if( isInMenu )
		{
			if (InterfaceController.curWindow == win_MainMenu)
			{
				SSSInput.InputSourceType selectedSourceType;
				if (Common.input.GetButtonDown(SSSInput.InputType.Cancel, out selectedSourceType))
				{
					Common.input.setInputSourceType(selectedSourceType);
				}
				if (Input.anyKeyDown && Common.input.GetAxis(SSSInput.InputType.Cancel) == 0)
				{
					Input.ResetInputAxes();
					win_cur.GetComponent< BaseWindow >().buttons[ win_cur.GetComponent< BaseWindow >().curButton ].OnClick();
					return;
				}
			}
			
			float menuSwich = 0;
			
			if (InterfaceController.curWindow == win_Win || InterfaceController.curWindow == win_LevelSelect)
			{
				menuSwich = -Common.input.GetAxis(SSSInput.InputType.Vertical);
			}
			else if (InterfaceController.curWindow == win_Sure)
			{
				menuSwich = Common.input.GetAxis(SSSInput.InputType.Horizontal);
			}
			else
			{
				menuSwich = -Common.input.GetAxis(SSSInput.InputType.Vertical);
				if (menuSwich == 0)
				{
					menuSwich = Common.input.GetAxis(SSSInput.InputType.Horizontal);
				}
			}
			menuSwich = menuSwich == 0 ? 0 : Mathf.Sign(menuSwich);
			
			if( menuSwich != 0 && menuSwich != prevMenuSwich && win_cur != null )
			{
				win_cur.GetComponent< BaseWindow >().curButton += (int)menuSwich;
				if( win_cur.GetComponent< BaseWindow >().curButton < win_cur.GetComponent< BaseWindow >().buttons.Count && win_cur.GetComponent< BaseWindow >().curButton >= 0 )
				{
					BaseButton btn = win_cur.GetComponent< BaseWindow >().buttons[ win_cur.GetComponent< BaseWindow >().curButton ];
					
					cursor.localPosition = getCursorPos(btn);
					
					win_cur.GetComponent< BaseWindow >().buttons[ win_cur.GetComponent< BaseWindow >().curButton ].OnPressJoystick( true );
					if( (int)menuSwich != 0 )
					{
						win_cur.GetComponent< BaseWindow >().buttons[ win_cur.GetComponent< BaseWindow >().curButton - (int)menuSwich ].OnPressJoystick( false );
					}
				}
				else
				{
					win_cur.GetComponent< BaseWindow >().curButton -= (int)menuSwich;
				}
				if (Input.anyKey)
				{
					Input.ResetInputAxes();
				}
			}
			else
			{
				if( ( InterfaceController.curWindow == win_LevelSelect/* || InterfaceController.curWindow == win_MainMenu*/ ) && cursor.gameObject.activeSelf )
				{
					if( Input.anyKey && Common.input.GetAxisRaw(SSSInput.InputType.Cancel) == 0 && Common.input.GetAxisRaw(SSSInput.InputType.Horizontal) == 0)
					{
						win_cur.GetComponent< BaseWindow >().buttons[ win_cur.GetComponent< BaseWindow >().curButton ].OnClick();
						Input.ResetInputAxes();
					}
				}
				else if( !winLevelSelect.isTutorialPlaying && !winLevelSelect.isDescriptionShow && !winLevelSelect.isTutorialPlaying &&
				        InterfaceController.curWindow != win_Sure && InterfaceController.curWindow != win_MainMenu)
				{
					float menuOk = Common.input.GetAxisRaw(SSSInput.InputType.Ok);
					if (Common.input.selectedInputSourceType == SSSInput.InputSourceType.Xbox && Input.anyKey &&
						Common.input.GetAxisRaw(SSSInput.InputType.Vertical) == 0 && Common.input.GetAxisRaw(SSSInput.InputType.Horizontal) == 0 &&
						InterfaceController.curWindow == win_Win)
					{
						menuOk = 1;
					}
					if( menuOk != 0 && menuOk != prevMenuOk && win_cur.GetComponent< BaseWindow >().buttons.Count > 0 )
					{
						win_cur.GetComponent< BaseWindow >().buttons[ win_cur.GetComponent< BaseWindow >().curButton ].OnClick();
						Input.ResetInputAxes();
					}
					prevMenuOk = menuOk;
				}
			}
			prevMenuSwich = menuSwich;
		}
	}


	
	// mouse is over the hit element
	void MouseOver( Transform hit )
	{
		return;
		// if another button haven't been pressed
		if( !wasBtnPressed )
		{
			if( curHit != null )
			{
				if( !curHit.GetComponent< BaseButton >().isTweenScale )
				{
					curHit.transform.localScale /= 1.03f;
				}
			}
			if( !hit.GetComponent< BaseButton >().isTweenScale )
			{
				hit.localScale *= 1.03f;
			}
			curHit = hit;
		}
		// else check if mouse button has bees upped
		MouseUp();
	}
	
	// mouse is over the same button
	void MouseOverSameButton()
	{
		return;
		// if another button haven't been pressed
		if( wasBtnPressed )
		{
			curHit.transform.GetComponent< BaseButton >().OnPress( true );
			if( !curHit.GetComponent< BaseButton >().isTweenScale )
			{
				curHit.transform.localScale *= 1.03f;
			}
			wasBtnPressed = false;
		}
	}
	
	// mouse is out of button
	void MouseOut()
	{
		return;
		if( curHit != null )
		{
			if( isButtonClicked )
			{
				curHit.transform.GetComponent< BaseButton >().OnPress( false );
				if( !curHit.GetComponent< BaseButton >().isTweenScale )
				{
					curHit.transform.localScale /= 1.03f;
				}
				wasBtnPressed = true;
				return;
			}
			else if( !wasBtnPressed )
			{
				if( !curHit.GetComponent< BaseButton >().isTweenScale )
				{
					curHit.transform.localScale /= 1.03f;
				}
				curHit = null;
			}
		}
	}
	
	// if some button was pressed - end it
	void MouseUp()
	{
		return;
		if( Input.GetMouseButtonUp( 0 ) && wasBtnPressed )
		{
			wasBtnPressed = false;
			curHit = null;
		}
	}
	
	public void AddCursor()
	{
		cursor = Instantiate( cursorPrefab ) as Transform;
		cursor.parent = transform;
	}
	
	
	public Vector3 getCursorPos(BaseButton btn)
	{
		Vector3 deltaPos = new Vector3( btn.sprite.transform.localScale.x / 2 * btn.transform.localScale.x/* - cursor.localScale.x / 2*/, 
													-btn.sprite.transform.localScale.y / 2.5f * btn.transform.localScale.y,
													-10 );
					
		return btn.transform.localPosition + deltaPos;
	}
}