using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Switch))]
public class SwitchEditor : Editor {

//    int selected = 4;
    Vector2 farpos;
    Vector2 nearpos;

    public override void OnInspectorGUI()
    {
       
        
        Switch obj = (Switch)target;
        if (obj.SwitchTexturesFar == null)
            obj.SwitchTexturesFar = new Texture2D[0];
        if (obj.SwitchTexturesNear== null)
            obj.SwitchTexturesNear = new Texture2D[0];

        GUILayout.Label("Lightmap Options");
        obj.options = (Brightness.AdjustLightmap)EditorGUILayout.EnumPopup(obj.options);
        GUILayout.Label("Execute ON");
        obj.executeTrigger = (Brightness.ExecuteONLite)EditorGUILayout.EnumPopup(obj.executeTrigger);
       // GUILayout.Label("Switch By");
       // obj.switchOptions = (Switch.TextureSwitchOptions)EditorGUILayout.EnumPopup(obj.switchOptions);

       
        if (obj.switchOptions == Switch.TextureSwitchOptions.Index)
        {
            if (obj.options == Brightness.AdjustLightmap.Both)
            {
                GUILayout.Label("Index Far");
                obj.startIndexFar = EditorGUILayout.IntSlider(obj.startIndexFar, -1, obj.SwitchTexturesFar.Length);
                GUILayout.Label("Index Near");
                obj.startIndexNear = EditorGUILayout.IntSlider(obj.startIndexNear, -1, obj.SwitchTexturesNear.Length);
            }
            else if (obj.options == Brightness.AdjustLightmap.Far)
            {
                GUILayout.Label("Index Far");
                obj.startIndexFar = EditorGUILayout.IntSlider(obj.startIndexFar, -1, obj.SwitchTexturesFar.Length);
            }
            else if (obj.options == Brightness.AdjustLightmap.Near)
            {
                GUILayout.Label("Index Near");
                obj.startIndexNear = EditorGUILayout.IntSlider(obj.startIndexNear, -1, obj.SwitchTexturesNear.Length);
            }
        }
        else if (obj.switchOptions == Switch.TextureSwitchOptions.Range)
        {
            GUILayout.Label("FPS");
            obj.timestamp = EditorGUILayout.IntSlider(obj.timestamp, 0, 45);

            if (obj.options == Brightness.AdjustLightmap.Both)
            {
                GUILayout.Label("Start Index Far");
                obj.startIndexFar = EditorGUILayout.IntSlider(obj.startIndexFar, -1, obj.SwitchTexturesFar.Length);
                GUILayout.Label("End Index Far");
                obj.endIndexFar = EditorGUILayout.IntSlider(obj.endIndexFar, -1, obj.SwitchTexturesFar.Length);
                if (obj.endIndexFar < obj.startIndexFar)
                    obj.endIndexFar = obj.startIndexFar;
                GUILayout.Label("Start Index Near");
                obj.startIndexNear = EditorGUILayout.IntSlider(obj.startIndexNear, -1, obj.SwitchTexturesNear.Length);
                GUILayout.Label("End Index Near");
                obj.endIndexNear = EditorGUILayout.IntSlider(obj.endIndexNear, -1, obj.SwitchTexturesNear.Length);
                if (obj.endIndexNear < obj.startIndexNear)
                    obj.endIndexNear = obj.startIndexNear;
            }
            else if (obj.options == Brightness.AdjustLightmap.Far)
            {
                GUILayout.Label("Start Index Far");
                obj.startIndexFar = EditorGUILayout.IntSlider(obj.startIndexFar, -1, obj.SwitchTexturesFar.Length);
                GUILayout.Label("End Index Far");
                obj.endIndexFar = EditorGUILayout.IntSlider(obj.endIndexFar, -1, obj.SwitchTexturesFar.Length);
                if (obj.endIndexFar < obj.startIndexFar)
                    obj.endIndexFar = obj.startIndexFar;
            }
            else if (obj.options == Brightness.AdjustLightmap.Near)
            {
                GUILayout.Label("Start Index Near");
                obj.startIndexNear = EditorGUILayout.IntSlider(obj.startIndexNear, -1, obj.SwitchTexturesNear.Length);
                GUILayout.Label("End Index Near");
                obj.endIndexNear = EditorGUILayout.IntSlider(obj.endIndexNear, -1, obj.SwitchTexturesNear.Length);
                if (obj.endIndexNear < obj.startIndexNear)
                    obj.endIndexNear = obj.startIndexNear;
            }
        }
        else if (obj.switchOptions == Switch.TextureSwitchOptions.All)
        {
        GUILayout.Label("FPS");
        obj.timestamp = EditorGUILayout.IntSlider(obj.timestamp,0,45);
        }
        DrawDefaultInspector();
    }
}
