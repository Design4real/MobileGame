using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(BaseButton))]
[CanEditMultipleObjects]
public class BaseButtonInspector : Editor 
{	
	int indexPressed;
	int indexDisabled;
	
	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		BaseButton button = target as BaseButton;
		
		if( button != null )
		{
			UISlicedSprite sprite = button.GetComponentInChildren<UISlicedSprite>();
			
			if( sprite == null )
			{
				return;
			}
			
			string[] spriteList = sprite.atlas.GetListOfSprites().ToArray();
			
			if( button.pressedSpriteName == null )
			{
				indexPressed = 0;
			}
			else
			{
				for( int i = 0; i < spriteList.Length; i++ )
				{
					if( spriteList[ i ] == button.pressedSpriteName )
					{
						indexPressed = i; 
						break;
					}
				}
			}
			
			if( button.disabledSpriteName == null )
			{
				indexDisabled = 0;
			}
			else
			{
				for( int i = 0; i < spriteList.Length; i++ )
				{
					if( spriteList[ i ] == button.disabledSpriteName )
					{
						indexDisabled = i; 
						break;
					}
				}
			}
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField( "Pressed sprite name", GUILayout.Width( 150 ) );
			indexPressed = 0;//
			if (GUILayout.Button(button.pressedSpriteName))
			{
				SpriteSelector.Show(sprite.atlas, sprite.name, pressedSpriteChange); 
			}	
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.LabelField( "Disabled sprite name", GUILayout.Width( 150 ) );
			indexDisabled = 0;
			if (GUILayout.Button(button.disabledSpriteName))
			{
				SpriteSelector.Show(sprite.atlas, sprite.name, disabledSpriteChange); 
			}	
		//	EditorGUILayout.Popup( indexDisabled, spriteList );
			EditorGUILayout.EndHorizontal();
			
			EditorGUILayout.BeginHorizontal();
			
			EditorGUILayout.LabelField( "Is need scale:" );
			button.isNeedScaling = EditorGUILayout.Toggle( button.isNeedScaling );
			
			EditorGUILayout.EndHorizontal();
			
		//	button.disabledSpriteName = spriteList[ indexDisabled ];
			//button.pressedSpriteName = spriteList[ indexPressed ];
			
			GameObject prefab = Resources.Load( "Prefabs/Interface/" + button.gameObject.transform.parent.gameObject.name ) as GameObject;
			//EditorUtility.SetDirty( prefab );
			if( prefab != null )
			{
				
				Transform prefabButtonTransform = prefab.transform.FindChild( button.gameObject.name );
				if( prefabButtonTransform != null )
				{
					BaseButton newButton = prefabButtonTransform.GetComponent<BaseButton>();
					
					newButton.disabledSpriteName = button.disabledSpriteName;
					newButton.pressedSpriteName = button.pressedSpriteName;
					EditorUtility.SetDirty( prefab );
				}
			}
			
		}
	}
	
	void pressedSpriteChange(string spriteName)
	{
		BaseButton button = target as BaseButton;
		button.pressedSpriteName = spriteName;
	}
	void disabledSpriteChange(string spriteName)
	{
		BaseButton button = target as BaseButton;
		button.disabledSpriteName = spriteName;
	}
}
