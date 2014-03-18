using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Tint))]
public class TintEditor : Editor
{
//    int selected = 4;
    public override void OnInspectorGUI()
    {
        Tint obj = (Tint)target;

        GUILayout.Label("Tint Color");
        obj.TintColor = EditorGUILayout.ColorField(obj.TintColor);
        GUILayout.Label("Lightmap Options");
        obj.options = (Tint.AdjustTint)EditorGUILayout.EnumPopup(obj.options);
        GUILayout.Label("Execute ON");
        obj.executeTrigger = (Tint.ExecuteON)EditorGUILayout.EnumPopup(obj.executeTrigger);
      
    }
}
