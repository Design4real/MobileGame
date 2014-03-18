// Custom Editor using SerializedProperties.
// Automatic handling of multi-object editing, undo, and prefab overrides.

#pragma strict

@CustomEditor(SubmergedEffect)

@CanEditMultipleObjects

class SubmergedEffectEditor extends Editor {
	
	var underwaterFogColorProp : SerializedProperty;
	
	var underwaterFogDensityProp : SerializedProperty;
	
	function OnEnable () {
	
		// Setup the SerializedProperties
		
		underwaterFogColorProp = serializedObject.FindProperty ("underwaterFogColor");
		
		underwaterFogDensityProp = serializedObject.FindProperty ("underwaterFogDensity");

	}
	
	function OnInspectorGUI() {
	
		// Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
		
		serializedObject.Update ();
		
		GUI.BringWindowToFront(0);
		
		EditorGUILayout.Space ();
			
		underwaterFogColorProp.colorValue = EditorGUILayout.ColorField(GUIContent ("Underwater Colour", "The colour of the fog underwater."), underwaterFogColorProp.colorValue);
		
		underwaterFogDensityProp.floatValue = EditorGUILayout.FloatField(GUIContent ("Underwater Density", "The density of the fog underwater."), underwaterFogDensityProp.floatValue);
		
		// Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
		
		serializedObject.ApplyModifiedProperties ();
	}
 }