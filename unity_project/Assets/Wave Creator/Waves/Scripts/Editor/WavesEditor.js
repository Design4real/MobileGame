// Custom Editor using SerializedProperties.
// Automatic handling of multi-object editing, undo, and prefab overrides.

#pragma strict

@CustomEditor(Waves)

@CanEditMultipleObjects

class WavesEditor extends Editor {

	var waveHeightProp : SerializedProperty;
	
	var waveFrequencyProp : SerializedProperty;
	
	var speedProp : SerializedProperty;
	
	var centerProp : SerializedProperty;
	
	var options : String[] = ["Flow Towards: ", "Flow Away From: "];
	
	var index : int = 0;
	
	var calculateUnderTerrainProp : SerializedProperty;
	
	@MenuItem ("GameObject/Create Other/Waves")

	static function ShowWaveCreator () {

   		EditorWindow.GetWindow (WaterCreator);
	}
	
	function OnEnable () {
	
		// Setup the SerializedProperties
		
		waveHeightProp = serializedObject.FindProperty ("waveHeight");
		
		waveFrequencyProp = serializedObject.FindProperty ("waveFrequency");
		
		speedProp = serializedObject.FindProperty ("speed");
		
		centerProp = serializedObject.FindProperty ("origin");
		
		calculateUnderTerrainProp = serializedObject.FindProperty ("calculateUnderTerrain");

	}
	
	function OnInspectorGUI() {
	
		// Update the serializedProperty - always do this in the beginning of OnInspectorGUI.
		
		serializedObject.Update ();
		
		EditorGUILayout.Space ();
		
			waveHeightProp.floatValue = Mathf.Clamp(EditorGUILayout.FloatField(GUIContent ("Wave Height", "The maximum height a wave could be."), waveHeightProp.floatValue),0,Mathf.Infinity);
			
			waveFrequencyProp.floatValue = Mathf.Clamp(EditorGUILayout.FloatField(GUIContent ("Wave Frequency", "The number of waves."), waveFrequencyProp.floatValue),0,Mathf.Infinity);
			
			if (index == 0) {
			
				speedProp.floatValue = Mathf.Clamp(EditorGUILayout.FloatField(GUIContent ("Wave Speed", "The speed of the waves."), Mathf.Abs(speedProp.floatValue)),0,Mathf.Infinity);
			}
			
			else {
			
				speedProp.floatValue = -Mathf.Clamp(EditorGUILayout.FloatField(GUIContent ("Wave Speed", "The speed of the waves."), Mathf.Abs(speedProp.floatValue)),0,Mathf.Infinity);
			}
			
			calculateUnderTerrainProp.boolValue = EditorGUILayout.Toggle (GUIContent ("Calculate Under Terrain", "If this is unticked the there will be no waves calculated under the terrain, preferable in most cases"), calculateUnderTerrainProp.boolValue);
		
		EditorGUILayout.BeginHorizontal("label");
		
			index = EditorGUILayout.Popup (index, options, GUILayout.MinWidth(145), GUILayout.MaxWidth(145));
		
			EditorGUILayout.LabelField(GUIContent ("x: ", "The x coordinate of the position the water flows from or to."), GUILayout.MinWidth(15), GUILayout.MaxWidth(15));
			centerProp.vector2Value.x = EditorGUILayout.FloatField(centerProp.vector2Value.x);
			
			EditorGUILayout.LabelField(GUIContent ("z: ", "The z coordinate of the position the water flows from or to."), GUILayout.MinWidth(15), GUILayout.MaxWidth(15));
			centerProp.vector2Value.y = EditorGUILayout.FloatField(centerProp.vector2Value.y);
		
		EditorGUILayout.EndHorizontal();
		
		// Apply changes to the serializedProperty - always do this in the end of OnInspectorGUI.
		
		serializedObject.ApplyModifiedProperties ();
	}
 }