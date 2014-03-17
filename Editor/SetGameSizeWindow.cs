using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
 
public class SetGameSizeWindow: EditorWindow {
 
    private Vector2 _size = new Vector2(800, 600);
  	static GameObject font;
	
    [MenuItem ("Window/Set Game Size...")]
    static void Init () 
	{
       	SetGameSizeWindow window = (SetGameSizeWindow)(EditorWindow.GetWindow(typeof(SetGameSizeWindow)));
    }
 
    public static EditorWindow GetMainGameView() 
	{
        System.Type T = System.Type.GetType("UnityEditor.GameView,UnityEditor");
        System.Reflection.MethodInfo GetMainGameView = T.GetMethod("GetMainGameView",System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
        System.Object Res = GetMainGameView.Invoke(null,null);
        return (EditorWindow)Res;
		
		
    }
 
    void OnGUI ()
	{
		if( GUILayout.Button( "960 x 640" ) )
		{
			_size.y = 640;
			_size.x = 960;
			Reposition();
		}
		if( GUILayout.Button( "1024 x 768" ) ) 
		{
			_size.y = 768;
			_size.x = 1024;
			Reposition();
		}
		if( GUILayout.Button( "1332 x 999" ) ) 
		{
			_size.y = 999;
			_size.x = 1332;
			Reposition();
		}
		if( GUILayout.Button( "1680 x 1050" ) ) 
		{
			_size.y = 1050;
			_size.x = 1680;
			Reposition();
		}
		if( GUILayout.Button( "1280 x 1024" ) ) 
		{
			_size.y = 1024;
			_size.x = 1280;
			Reposition();
		}
		if( GUILayout.Button( "Hide" ) )
		{
			_size.y = 100;
			_size.x = 100;
			Reposition();
		}
		if( GUILayout.Button( "Clear saves" ) )
		{
			PlayerPrefs.DeleteAll();
		}
		if (GUILayout.Button( "Set Button" ))
		{
			Object[] btns = GameObject.FindObjectsOfType( typeof( UIButton ) );
			foreach( Object btn in btns )
			{
				((UIButton)btn).gameObject.AddComponent<BaseButton>();
				Object	_scale = ((UIButton)btn).gameObject.GetComponent<UIButtonScale>();
				GameObject.DestroyImmediate(_scale);
				Object	_ofset = ((UIButton)btn).gameObject.GetComponent<UIButtonOffset>();
				GameObject.DestroyImmediate(_ofset);
				Object	_sound = ((UIButton)btn).gameObject.GetComponent<UIButtonSound>();
				GameObject.DestroyImmediate(_sound);
				Object	_btn = ((UIButton)btn).gameObject.GetComponent<UIButton>();
				GameObject.DestroyImmediate(_btn);
			}
		}
		//TODO: пиздец коряво, переписать на выборку!!!!!
		if( GUILayout.Button( "ChanheTextColor" ) )
		{
			Object[] lbls = GameObject.FindObjectsOfType( typeof( UILabel ) );
			
			foreach( Object lbl in lbls )
			{
				UILabel label = (UILabel)lbl;
				if( label.transform.parent.name.Length >= 1 && label.transform.parent.name.Length <= 2 )
				{
					label.color = new Color( 219f / 255f, 242f / 255f, 127f / 255f );
				}
			}
		}
    }
	
	void Reposition()
	{
        EditorWindow gameView = GetMainGameView();
        Rect pos = gameView.position;
        pos.width = _size.x;
        pos.height = _size.y + 17;
        gameView.position = pos;
	}

	[CustomPropertyDrawer(typeof(EnumFlagsAttribute))]
	public class EnumFlagsAttributeDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect _position, SerializedProperty _property, GUIContent _label)
		{
			_property.intValue = EditorGUI.MaskField( _position, _label, _property.intValue, _property.enumNames );
		}
	}
 
}