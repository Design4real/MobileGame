using UnityEngine;
using System.Collections;
using System.Reflection;

public class BaseButton : MonoBehaviour
{
	
	public delegate void ButtonEventHandler( BaseButton button );//(BaseButton button);
	//public delegate void ButtonDragEventHandler(BaseButton button, Vector2 delta);
	
	public event ButtonEventHandler event_buttonPress;
	public event ButtonEventHandler event_buttonClick;
	public event ButtonEventHandler event_buttonUnpress;
	//public event ButtonDragEventHandler event_buttonDrag;
	
	/// <summary>
	/// Массив открываемых этой кнопкой панелей( если ничего не открывает - оставляем пустым )
	/// </summary>
	//public GameObject[] openingPanels;
	
	/// <summary>
	/// Массив закрываемых этой кнопкой панелей( если ничего не закрывает - оставляем пустым )
	/// </summary>
	//public GameObject[] closingPanels;
	
	/// <summary>
	/// Строка, выводящаяся в лог вместо функционала кнопки
	/// </summary>
	//public string debugString;
	
	/// <summary>
	/// Имя спрайта, котом заменяется спрайт кнопки при нажатии - нажатое состояние
	/// </summary>
	[HideInInspector]
	public string pressedSpriteName;
	/// <summary>
	/// Имя спрайта, котом заменяется спрайт кнопки при блокировке - неактивное состояние
	/// </summary>
	[HideInInspector]
	public string disabledSpriteName;
	/// <summary>
	/// Имя спрайта базового, ненажатого состояния кнопки
	/// </summary>
	[HideInInspector]
	public string enabledSpriteName;
	[HideInInspector]
	public UILabel label;
	[HideInInspector]
	public bool isNeedScaling = true;
	public bool isNeedChangeTextColor = false;
	public Color activeColor;
	public Color nonActiveColor;
	
	public bool isShowUpdateLog = false;
	public bool isTweenScale;
	
//	bool isScaled = false;
	
	
//	public Color disabledTextColor;
//	private Color enabledTextColor;
	
	public UISlicedSprite sprite;
	public UISprite pictureSprite;
	public UILabel pictureLabel;
	//public UIAtlas atlas;
	
	
	bool touchLeftDuringDrag;
	
	private static bool isObjectUnderTouch(Transform obj, UICamera.MouseOrTouch touch)
	{
		return isObjectUnderTouch(obj, touch.pos);
	}
	
	public static bool isObjectUnderTouch(Transform obj, Vector2 touchPos)
	{
		Camera nguiCam = UICamera.currentCamera;
		
		if( nguiCam != null )
		{
			Ray inputRay = nguiCam.ScreenPointToRay( touchPos );    
			RaycastHit hit;
			
			return (Physics.Raycast( inputRay.origin, inputRay.direction, out hit, Mathf.Infinity ) && hit.transform == obj );
		}
		return false;
	}
	
	void Awake()
	{
		Transform back = transform.FindChild( "Background" );
		Transform picture = transform.FindChild( "Picture" );
		if( back != null )
		{
			sprite = back.GetComponent<UISlicedSprite>();
		}
		else
		{
			sprite = GetComponentInChildren<UISlicedSprite>();
		}
		if( sprite == null )
		{
			sprite = GetComponent<UISlicedSprite>();
		}
		
		if( picture != null )
		{
			pictureSprite = picture.GetComponent<UISprite>();
			if( pictureSprite == null )
			{
				pictureLabel = picture.GetComponent< UILabel >();
				Debug.Log( pictureLabel );
			}
		}
		
		label = GetComponentInChildren<UILabel>();
		if( label != null )
		{
			label.color = nonActiveColor;
		}
		//atlas = sprite.atlas;
		if( sprite != null )
		{
			enabledSpriteName = sprite.spriteName;
		}
	/*	if( label != null )
		{
			enabledTextColor = label.color;
		} */
		if( !enabled )
		{
			OnDisable();
		}
		else
		{
			OnEnable();
		}
		
		if( GetComponent< UIButtonScale >() != null )
		{
			isTweenScale = true;
		}
	}
	
	void Start()
	{
		// Выпилить скэйл временная мера
		/*
		if( isNeedScaling && !isScaled )
		{
			transform.localScale = new Vector3(transform.localScale.x,
											   transform.localScale.y/MenuController.ScaleRatio,
											   transform.localScale.z);
			isScaled = true;
		}
		*/
	}
	
	public void OnClick()
	{
		if( enabled )
		{
			if (event_buttonClick != null && !touchLeftDuringDrag)
			{
				event_buttonClick( this );	
			}
		}
	} 
	
	public void OnPress( bool isPressed )
	{
		if( enabled && !MenuController.runtime.isUsingJoystick )
		{
			Debug.Log( "Press " + isPressed );
			if( sprite != null )
			{
				sprite.spriteName = isPressed ? pressedSpriteName : enabledSpriteName;
			}
			if( isPressed )
			{
				MenuController.runtime.isButtonClicked = true;
				if (event_buttonPress != null)
				{
					event_buttonPress( this );
				}
				touchLeftDuringDrag = false;
				if( label != null )
				{
					label.color = activeColor;
				}
			}
			else
			{
				MenuController.runtime.isButtonClicked = false;
				if (event_buttonUnpress != null)
				{
					event_buttonUnpress( this );
				}
				if( label != null )
				{
					label.color = nonActiveColor;
				}
			}
		}
	}
	
	public void OnPressJoystick( bool isPressed )
	{
		if( enabled )
		{
			if( sprite != null )
			{
				sprite.spriteName = isPressed ? pressedSpriteName : enabledSpriteName;
			}
			if( isPressed )
			{
				MenuController.runtime.isButtonClicked = true;
				if (event_buttonPress != null)
				{
					event_buttonPress( this );
				}
				touchLeftDuringDrag = false;
				if( label != null )
				{
					label.color = activeColor;
				}
			}
			else
			{
				MenuController.runtime.isButtonClicked = false;
				if (event_buttonUnpress != null)
				{
					event_buttonUnpress( this );
				}
				if( label != null )
				{
					label.color = nonActiveColor;
				}
			}
		}
	}
	
	void Update()
	{
	}
	
	void OnMouseOver()
	{
		/*if( enabled )
		{
			transform.localScale *= 1.05f;
		}*/
	}
	
	void OnMouseOut()
	{
		/*if( enabled )
		{
			transform.localScale /= 1.05f;
		}*/
	}
	
	/*void OnDrag(Vector2 delta)
	{
		if (event_buttonDrag != null)
		{
			event_buttonDrag(this, delta);
		}
		if (!isObjectUnderTouch(transform, UICamera.currentTouch))
		{
			touchLeftDuringDrag = true;
		}
	} */
	
	void OnDisable()
	{
		if( sprite != null )
		{
			sprite.spriteName = disabledSpriteName;
		}
		/*
		if( isNeedScaling && !isScaled )
		{
			transform.localScale = new Vector3(transform.localScale.x,
											   transform.localScale.y/MenuController.ScaleRatio,
											   transform.localScale.z);
			isScaled = true;
		}
					*/					   
		if( label != null )
		{
			label.gameObject.SetActive( false );
		}
	}
	
	void OnEnable()
	{
		if( sprite != null )
		{
			sprite.spriteName = enabledSpriteName;
		}
		if( label != null )
		{
			label.gameObject.SetActive( true );
		}
	}	
	
	public void ApplySprite()
	{
		if( sprite != null )
		{
			sprite.spriteName = enabledSpriteName;
		}
	}
	
	public void SetPictureSprite( string spriteName )
	{
		if( pictureSprite != null )
		{
			pictureSprite.spriteName = spriteName;
		}
	}
	
	public void SetPictureAtlas( UIAtlas atlas )
	{
		if( pictureSprite != null )
		{
			pictureSprite.atlas = atlas;
		}
	}
}
