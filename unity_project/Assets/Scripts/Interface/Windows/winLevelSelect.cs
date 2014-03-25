using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class winLevelSelect : BaseWindow
{
	public GameObject video;
	public GameObject tutorialVideo;
	public GameObject[] levelsDescription;
	public List<GameObject> allStuff;
	
	public static bool isTutorialPlaying = false;
	public static bool isDescriptionShow = false;
	
	int num;
	string lvlNum;
	
	private Vector3 cursorScale = Vector3.zero;
	
	void Awake()
	{
//		MovieTexture movie = ( (MovieTexture)video.GetComponent< UITexture >().material.mainTexture );
//		movie.anisoLevel = 8;
//		movie.loop = true;
//		movie.Play();
		isDescriptionShow = false;
		for( int i = 0; i < levelsDescription.Length; i++ )
		{
			levelsDescription[ i ].SetActive( false );
		}
	}
	
	public void btnLevelClick( string btnName )
	{
		Debug.Log( "Description" );
		lvlNum = btnName.Substring( btnName.IndexOf("_") + 1, 1 );
		num = int.Parse( lvlNum ) - 1;
		levelsDescription[ num ].SetActive( true );
		isDescriptionShow = true;
		allStuff.ForEach(a => a.SetActive(false));
		MenuController.runtime.cursor.gameObject.SetActive( false );
		Input.ResetInputAxes();
	}
	
	public void btnTutorialClick( string btnName )
	{
		/*tutorialVideo.SetActive( true );
		MovieTexture movie = ( (MovieTexture)tutorialVideo.GetComponent< UITexture >().material.mainTexture );
		movie.anisoLevel = 8;
		movie.loop = true;
		movie.Stop();
		movie.Play();*/
		Debug.Log("btnName");
		cursorScale = MenuController.runtime.cursor.localScale;
		InterfaceController.openPopUp( MenuController.runtime.win_Info );
		isTutorialPlaying = true;
		MenuController.runtime.cursor.gameObject.SetActive( false );
	}
	
	public void FixedUpdate()
	{
		if (winInfo.runtime != null)
		{
			return;
		}
		MenuController.isInMenu = true;
		if (MenuController.runtime.win_cur == transform)
		{
			if ((MenuController.runtime.cursor == null || MenuController.runtime.cursor.parent != transform || !MenuController.runtime.cursor.gameObject.activeSelf) &&
				curButton >= 0 && !isDescriptionShow)
			{
				GameObject.DestroyImmediate( MenuController.runtime.cursor.gameObject );
				MenuController.runtime.cursor = GameObject.Instantiate( MenuController.runtime.cursorPrefab ) as Transform;
				MenuController.runtime.cursor.parent = transform;
				MenuController.runtime.cursor.localScale = new Vector3( MenuController.runtime.cursor.GetComponent< UISprite >().sprite.outer.width, MenuController.runtime.cursor.GetComponent< UISprite >().sprite.outer.height, 1 );
				MenuController.runtime.cursor.localPosition = MenuController.runtime.getCursorPos(buttons[curButton]);
			}
		}
		if( !isTutorialPlaying && !isDescriptionShow )
		{
			bool menuCancel = Common.input.GetButtonDown(SSSInput.InputType.Cancel);
			if( menuCancel && menuCancel != MenuController.prevMenuCancel )
			{
				InterfaceController.OpenWindow( MenuController.runtime.win_MainMenu );
			}
			MenuController.prevMenuCancel = menuCancel;
		}
		else if( isDescriptionShow )
		{
			bool menuCancel = Common.input.GetButtonDown(SSSInput.InputType.Cancel);

			if( menuCancel && menuCancel != MenuController.prevMenuCancel )
			{
				isDescriptionShow = false;
				allStuff.ForEach(a => a.SetActive(true));
				levelsDescription[ num ].SetActive( false );
				MenuController.runtime.cursor.gameObject.SetActive( true );
				Input.ResetInputAxes();
				MenuController.prevMenuCancel = menuCancel;
				return;
			}
			MenuController.prevMenuCancel = menuCancel;
			
			//float menuOk = Input.GetAxisRaw( "Ok" );
			//if( menuOk != 0 && menuOk != MenuController.prevMenuOk )
			if( Input.anyKey )
			{
				isDescriptionShow = false;
				if (lvlNum == "2")
				{
					lvlNum = "4";
				}
				MenuController.isInMenu = false;
				Application.LoadLevel( "s" +  lvlNum);
			}
			//MenuController.prevMenuOk = menuOk;
		}
		else
		{
			if( isTutorialPlaying )
			{
				/*MovieTexture movie = ( (MovieTexture)tutorialVideo.GetComponent< UITexture >().material.mainTexture );
				movie.Stop();
				isTutorialPlaying = false;
				tutorialVideo.SetActive( false );
				Input.ResetInputAxes();*/
				isTutorialPlaying = false;
				MenuController.runtime.cursor.gameObject.SetActive( true );
				MenuController.runtime.cursor.localScale = cursorScale;
			}
		}
	}
}
