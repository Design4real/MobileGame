using UnityEngine;
using System.Collections;

public class winInfo : BaseWindow 
{
	public MovieClipBehaviour tutorial_custom;
	private int tutorial_custom_actualLastFrameNum = 500;
	public MovieClipBehaviour tutorial_xbox;
	private int tutorial_xbox_actualLastFrameNum = 750;
	
	public static winInfo runtime;
	
	//MovieTexture movie;
	
	private MovieClipBehaviour curClip;
	private int curClip_actualLastFrameNum;
	
	public void Awake()
	{
		Input.ResetInputAxes();
		
		tutorial_xbox.gameObject.SetActive(false);
		tutorial_custom.gameObject.SetActive(false);
		if (MenuController.isCustomInputInUse)// || true)
		{
			curClip = tutorial_custom;
			curClip_actualLastFrameNum = tutorial_custom_actualLastFrameNum;
		}
		else
		{
			curClip = tutorial_xbox;
			curClip_actualLastFrameNum = tutorial_xbox_actualLastFrameNum;
		}
		curClip.gameObject.SetActive(true);
		
		runtime = this;
	}
	
	protected override void Start ()
	{
		base.Start ();
		
		transform.parent = null;
		transform.localScale = Vector3.one;
	}
	
	public void btnCloseClick( string btnName )
	{
		Time.timeScale = 1;
		InterfaceController.ClosePopUP();
		if (Application.loadedLevel != 0)
		{
			MenuController.runtime.isPaused = false;
			MenuController.isInMenu = false;
			MenuController.runtime.cursor.gameObject.SetActive(false);
			GameController.runtime.isPaused = false;
		}
		else
		{
			InterfaceController.curWindow = MenuController.runtime.win_LevelSelect;
		}
	}
	
	protected override void Update ()
	{
		base.Update ();
		
		//Debug.Log(curClip.movieClip.getCurrentFrame().ToString("D4") + "/" + curClip.movieClip.getTotalFrames().ToString("D4"));
		
		
		if (curClip.movieClip.getCurrentFrame() >= curClip_actualLastFrameNum)
		{
			//curClip.movieClip.setFrame(0);
			btnCloseClick("");
		}
	}
	
	public void FixedUpdate()
	{
		if( Input.anyKey )
		{
			btnCloseClick("");
		}
		Input.ResetInputAxes();
		/*
		float menuCancel = Input.GetAxisRaw("Cancel");
		if( menuCancel != 0 && menuCancel != MenuController.prevMenuCancel )
		{
			MenuController.isInMenu = false;
			GameController.runtime.isPaused = false;
			InterfaceController.ClosePopUP();
		}
		MenuController.prevMenuCancel = menuCancel;
		*/
	}
	
	void OnDestroy()
	{
		if (runtime == this)
		{
			runtime = null;
		}
	}
}
