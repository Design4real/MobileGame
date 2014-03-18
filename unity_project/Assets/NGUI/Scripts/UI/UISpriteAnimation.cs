//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Very simple sprite animation. Attach to a sprite and specify a bunch of sprite names and it will cycle through them.
/// </summary>

[ExecuteInEditMode]
[RequireComponent(typeof(UISprite))]
[AddComponentMenu("NGUI/UI/Sprite Animation")]
public class UISpriteAnimation : MonoBehaviour
{
	[HideInInspector][SerializeField] int mFPS = 30;
	[HideInInspector][SerializeField] string mPrefix = "";
	[HideInInspector][SerializeField] bool mLoop = true;
	//sss
	[HideInInspector][SerializeField] float mScale = 1;
	[HideInInspector][SerializeField] bool mConfigurable = false;
	[HideInInspector][SerializeField] int[] mFrameNumbers;
	[HideInInspector][SerializeField] int mFrameCount;
	int baseFPS;
	//sss

	UISprite mSprite;
	float mDelta = 0f;
	int mIndex = 0;
	bool mActive = true;
	List<string> mSpriteNames = new List<string>();

	/// <summary>
	/// Number of frames in the animation.
	/// </summary>

	public int frames { get { return mSpriteNames.Count; } }

	/// <summary>
	/// Animation framerate.
	/// </summary>

	public int framesPerSecond { get { return mFPS; } set { mFPS = value; } }

	/// <summary>
	/// Set the name prefix used to filter sprites from the atlas.
	/// </summary>

	public string namePrefix { get { return mPrefix; } set { if (mPrefix != value) { mPrefix = value; RebuildSpriteList(); } } }

	/// <summary>
	/// Set the animation to be looping or not
	/// </summary>

	public bool loop { get { return mLoop; } set { mLoop = value; } }
	
	//sss
	public float scale { get { return mScale; } set { mScale = value; } }
	public bool configurable { get { return mConfigurable; } set { mConfigurable = value; } }
	public int frameCount { get { return mFrameCount; } set { mFrameCount = value; frameNumbers = new int[ mFrameCount ]; } }
	public int[] frameNumbers { get { return mFrameNumbers; } set { mFrameNumbers = value; } }
	//sss

	/// <summary>
	/// Returns is the animation is still playing or not
	/// </summary>

	public bool isPlaying { get { return mActive; } }

	/// <summary>
	/// Rebuild the sprite list first thing.
	/// </summary>

	void Start () { RebuildSpriteList(); baseFPS = mFPS; }

	/// <summary>
	/// Advance the sprite animation process.
	/// </summary>

	void Update ()
	{
		mFPS = (int)( baseFPS * Time.timeScale );
		
		if (mActive && mSpriteNames.Count > 1 && Application.isPlaying && mFPS > 0f)
		{
			mDelta += Time.deltaTime;
			float rate = 1f / mFPS;
			
			//sss
			//костыльный костыль
			if ( /*true ||*/ rate < mDelta)
			{
				
				mDelta = (rate > 0f) ? mDelta - rate : 0f;
				if( mConfigurable )
				{
					if (++mIndex >= mFrameCount)
					{
						mIndex = 0;
						mActive = loop;
					}
				}
				else if (++mIndex >= mSpriteNames.Count)
				{
					mIndex = 0;
					mActive = loop;
				}

				if (mActive)
				{
					//sss
					if( mConfigurable )
					{
						mSprite.spriteName = mSpriteNames[mFrameNumbers[mIndex] - 1];
						mSprite.transform.localScale = new Vector3( mSprite.atlas.GetSprite( mSprite.spriteName ).outer.width * mScale, mSprite.atlas.GetSprite( mSprite.spriteName ).outer.height * mScale, 1 );
					}
					else
					{
						mSprite.spriteName = mSpriteNames[mIndex];
						//делаем спрайт размером с текущий кадр, модифицированный на константу
						mSprite.transform.localScale = new Vector3( mSprite.atlas.GetSprite( mSprite.spriteName ).outer.width * mScale, mSprite.atlas.GetSprite( mSprite.spriteName ).outer.height * mScale, 1 );
						//делало спрайт размером с первый кадр анимации
						//mSprite.MakePixelPerfect();
					}
					//sss
				}
			}
		}
	}

	/// <summary>
	/// Rebuild the sprite list after changing the sprite name.
	/// </summary>

	void RebuildSpriteList ()
	{
		if (mSprite == null) mSprite = GetComponent<UISprite>();
		mSpriteNames.Clear();

		if (mSprite != null && mSprite.atlas != null)
		{
			List<UIAtlas.Sprite> sprites = mSprite.atlas.spriteList;

			for (int i = 0, imax = sprites.Count; i < imax; ++i)
			{
				UIAtlas.Sprite sprite = sprites[i];

				if (string.IsNullOrEmpty(mPrefix) || sprite.name.StartsWith(mPrefix))
				{
					mSpriteNames.Add(sprite.name);
				}
			}
			mSpriteNames.Sort();
		}
	}
	
	/// <summary>
	/// Reset the animation to frame 0 and activate it.
	/// </summary>
	
	public void Reset()
	{
		mActive = true;
		mIndex = 0;
	}
}