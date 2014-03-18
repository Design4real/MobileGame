//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;
using UnityEditor;

/// <summary>
/// Inspector class used to edit UISpriteAnimations.
/// </summary>

[CustomEditor(typeof(UISpriteAnimation))]
public class UISpriteAnimationInspector : Editor
{
	/// <summary>
	/// Draw the inspector widget.
	/// </summary>

	public override void OnInspectorGUI ()
	{
		NGUIEditorTools.DrawSeparator();
		EditorGUIUtility.LookLikeControls(80f);
		UISpriteAnimation anim = target as UISpriteAnimation;

		int fps = EditorGUILayout.IntField("Framerate", anim.framesPerSecond);
		fps = Mathf.Clamp(fps, 0, 60);
		
		if (anim.framesPerSecond != fps)
		{
			NGUIEditorTools.RegisterUndo("Sprite Animation Change", anim);
			anim.framesPerSecond = fps;
			EditorUtility.SetDirty(anim);
		}

		string namePrefix = EditorGUILayout.TextField("Name Prefix", (anim.namePrefix != null) ? anim.namePrefix : "");

		if (anim.namePrefix != namePrefix)
		{
			NGUIEditorTools.RegisterUndo("Sprite Animation Change", anim);
			anim.namePrefix = namePrefix;
			EditorUtility.SetDirty(anim);
		}

		bool loop = EditorGUILayout.Toggle("Loop", anim.loop);

		if (anim.loop != loop)
		{
			NGUIEditorTools.RegisterUndo("Sprite Animation Change", anim);
			anim.loop = loop;
			EditorUtility.SetDirty(anim);
		}
		
		//sss
		float scale = EditorGUILayout.FloatField("Scale", anim.scale );
		
		if( anim.scale != scale )
		{
			NGUIEditorTools.RegisterUndo("Sprite Animation Change", anim);
			anim.scale = scale;
			EditorUtility.SetDirty(anim);
		}
		
		bool configurable = EditorGUILayout.Toggle("Configurable", anim.configurable);
		
		if( anim.configurable != configurable )
		{
			NGUIEditorTools.RegisterUndo("Sprite Animation Change", anim);
			anim.configurable = configurable;
			EditorUtility.SetDirty(anim);
		}
		
		if( anim.configurable )
		{
			int length = EditorGUILayout.IntField( "Frames count:", anim.frameCount );
			
			if( anim.frameCount != length )
			{
				NGUIEditorTools.RegisterUndo("Sprite Animation Change", anim);
				anim.frameCount = length;
				EditorUtility.SetDirty(anim);
			}
			
			for( int i = 0; i < length; i++ )
			{
				int frameNumber = EditorGUILayout.IntField( ( "Frames " + i.ToString() ), anim.frameNumbers[ i ] );
				
				if( anim.frameNumbers[ i ] != frameNumber )
				{
					NGUIEditorTools.RegisterUndo("Sprite Animation Change", anim);
					anim.frameNumbers[ i ] = frameNumber;
					EditorUtility.SetDirty(anim);
				}
			}
		}
		//sss
	}
}