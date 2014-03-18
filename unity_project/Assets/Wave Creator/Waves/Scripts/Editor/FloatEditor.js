// Custom Editor using SerializedProperties.
// Automatic handling of multi-object editing, undo, and prefab overrides.

#pragma strict

@CustomEditor(Float)

@CanEditMultipleObjects

class FloatEditor extends Editor {

	var offsetProp : SerializedProperty;
	
	var randomRotationFactorProp : SerializedProperty;
	
	function OnEnable () {
	
		// Setup the SerializedProperties
		
		offsetProp = serializedObject.FindProperty ("offset");
		
		randomRotationFactorProp = serializedObject.FindProperty ("randomRotationFactor");

	}
	
	function OnInspectorGUI() {
	
		// Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
		
		serializedObject.Update ();
		
		GUI.BringWindowToFront(0);
		
		EditorGUILayout.Space ();
		
		offsetProp.floatValue = EditorGUILayout.FloatField(GUIContent ("Offset", "The height this object floats in the water."), offsetProp.floatValue);
			
		randomRotationFactorProp.floatValue = EditorGUILayout.FloatField(GUIContent ("Random Rotation Factor", "The amount this object shakes in the water."), randomRotationFactorProp.floatValue);
		
		// Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
		
		serializedObject.ApplyModifiedProperties ();
	}
 }