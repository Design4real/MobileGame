using UnityEngine;
using System.Collections;

public class winWin : BaseWindow
{
	public GameObject winTexture;
	public GameObject looseTextureTime;
	public GameObject looseTextureLives;
	public UITexture background;
	
	protected override void Start()
	{
		base.Start();
		
		if( GameController.runtime.curState == GameController.GameState.win || GameController.runtime.curState == GameController.GameState.miniGame2)
		{
			winTexture.SetActive( true );
			looseTextureLives.SetActive( false );
			looseTextureTime.SetActive( false );
		}
		else
		{
			winTexture.SetActive( false );
			if( GameController.runtime.LivesCount == 0 )
			{
				looseTextureLives.SetActive( true );
				looseTextureTime.SetActive( false );
			}
			else
			{
				looseTextureLives.SetActive( false );
				looseTextureTime.SetActive( true );
			}
		}
	}
	
	public void btnMenuClick( string btnName )
	{
		Application.LoadLevel( "Menu" );
	}
	
	public void btnRestartClick( string btnName )
	{
		Application.LoadLevel( Application.loadedLevelName );
	}
	
	public void btnNextClick( string btnName )
	{
		if( Application.loadedLevelName == "s3" )
		{
			Application.LoadLevel( "Menu" );
		}
		else if( Application.loadedLevelName == "s1" )
		{
			Application.LoadLevel( "s4" );
		}
		else if( Application.loadedLevelName == "s4" )
		{
			Application.LoadLevel( "s3" );
		}
	}
}
