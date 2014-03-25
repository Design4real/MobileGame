using UnityEngine;
using System.Collections;

public class winMainMenu : BaseWindow
{
	public GameObject video;
	
	void Awake()
	{
		Input.ResetInputAxes();
//		MovieTexture movie = ( (MovieTexture)video.GetComponent< UITexture >().material.mainTexture );
//		movie.anisoLevel = 8;
//		movie.loop = true;
//		movie.Play();
		
		Debug.Log(Common.selectedInputSourceTypeString);
		//Common.input.setInputSourceType(SSSInput.InputSourceType.None);
	}
	
	public void btnLevelSelectClick( string btnName )
	{
		if (Common.selectedInputSourceType == SSSInput.InputSourceType.None)
		{
			return;
		}
		InterfaceController.OpenWindow( MenuController.runtime.win_LevelSelect );
	}
	
	void FixedUpdate()
	{
		MenuController.isInMenu = true;
		
		if (InterfaceController.curWindow != MenuController.runtime.win_Sure)
		{
			bool menuCancel = Common.input.GetButtonDown(SSSInput.InputType.Cancel);
			if( menuCancel && menuCancel != MenuController.prevMenuCancel )
			{
				winSure.isFromLevel = false;
				InterfaceController.openPopUp( MenuController.runtime.win_Sure );
			}
			MenuController.prevMenuCancel = menuCancel;
			
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
		foreach (string curJoystickName in Input.GetJoystickNames())
		{
			GUILayout.Label(curJoystickName, GUILayout.Height(40), GUILayout.Width(1000));
			GUILayout.Space(1);
		}
		GUILayout.Label(Common.selectedInputSourceTypeString);
		GUILayout.EndVertical();
		
		GUI.skin.label.fontSize = cachedSize;
		GUI.color = cachedColor;
		GUI.contentColor = cachedContentColor;
		GUI.skin.label.fontStyle = cachedFontStyle;
	}
}
