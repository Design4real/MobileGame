using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(Brightness))]
public class BrightnessEditor :Editor {
//    int selected = 4;
    public override void OnInspectorGUI()
    {
        Brightness obj = (Brightness)target;
		
		GUILayout.BeginHorizontal();
		if(obj.renderer.lightmapIndex<LightmapSettings.lightmaps.Length && obj.renderer.lightmapIndex>=0)
		{
      if(!LightmapManager.isReadable(LightmapSettings.lightmaps[obj.renderer.lightmapIndex].lightmapFar))
		{
		if(GUILayout.Button("Make Readable (Far)"))
			LightmapManager.SetIsReadable(LightmapSettings.lightmaps[obj.renderer.lightmapIndex].lightmapFar);
		}
		
		 if(!LightmapManager.isReadable(LightmapSettings.lightmaps[obj.renderer.lightmapIndex].lightmapNear))
		{
		if(GUILayout.Button("Make Readable (Near)"))
			LightmapManager.SetIsReadable(LightmapSettings.lightmaps[obj.renderer.lightmapIndex].lightmapNear);
		}
		}
       GUILayout.EndHorizontal();
		GUILayout.Label("Brightness Level");
        obj.level =  EditorGUILayout.Slider(obj.level, -1, 1);
		 GUILayout.Label("Operate on (GPU/CPU)");
        obj.operateOn =(Brightness.OperateON) EditorGUILayout.EnumPopup(obj.operateOn);
        GUILayout.Label("Lightmap Options");
        obj.options =(Brightness.AdjustLightmap) EditorGUILayout.EnumPopup(obj.options);
        GUILayout.Label("Execute ON");
        obj.executeTrigger = (Brightness.ExecuteON)EditorGUILayout.EnumPopup(obj.executeTrigger);
		obj.useSharedMaterials = EditorGUILayout.Toggle("Use Shared Materials",obj.useSharedMaterials);
		obj.timeScale = EditorGUILayout.FloatField("Time Scale",obj.timeScale);
		obj.useCurve = EditorGUILayout.Toggle("Use Curve",obj.useCurve);
		if(obj.useCurve)
		{
			obj.curve = EditorGUILayout.CurveField("Curve",obj.curve);
			obj.loopCurve = EditorGUILayout.Toggle("Loop Curve",obj.loopCurve);
			obj.startOnFirstKey = EditorGUILayout.Toggle("Start On 1st Key",obj.startOnFirstKey);
			
			if(obj.timeScale<0)
				obj.timeScale = 0;
		}
			else
		{
		GUILayout.Label("Minimum Brightness");
        obj.Min = EditorGUILayout.Slider(obj.Min, -1, 1);
        GUILayout.Label("Maximum Brightness");
        obj.Max = EditorGUILayout.Slider(obj.Max, -1, 1);
		}
	//	if(GUILayout.Button("Write EXR"))
	//	{
	//		SaveExr.Save(LightmapSettings.lightmaps[obj.renderer.lightmapIndex].lightmapFar,"D:\test2.exr");
		
	//EXRWriter.writeEXR(LightmapSettings.lightmaps[obj.renderer.lightmapIndex].lightmapFar,"D:\\Test.exr");
	//	}
		}
}
