using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

[CustomEditor(typeof(LightProbes))]
public class LightProbesEditor : Editor {


	bool toggleList = false;
	bool[] toggleProbes = null;
	public override void OnInspectorGUI ()
	{
	
		//DrawDefaultInspector();
		
		LightProbes probes = (LightProbes) target;
		float[] newVals = new float[probes.coefficients.Length];
		Array.Copy(probes.coefficients,newVals,probes.coefficients.Length);
		if(toggleProbes==null)
		toggleProbes = new bool[probes.count];
		EditorGUILayout.LabelField("Cell Count",probes.cellCount.ToString());
		EditorGUILayout.LabelField("Probe Count",probes.count.ToString());
		toggleList = EditorGUILayout.Foldout(toggleList,"Coefficients");
		
		
		if(toggleList)
		{
			int num =0;
		int probeNum =0;
		 toggleProbes[probeNum]= EditorGUILayout.Foldout(toggleProbes[probeNum],"Probe "+probeNum.ToString());
			
		for(int i =0;i<probes.coefficients.Length;i++)
		{
			if(toggleProbes[probeNum])
					{
			newVals[i] =EditorGUILayout.Slider(i.ToString(),probes.coefficients[i],-1,1);
					}
					if(num==27)
			{
				 num = 0;
				 probeNum++;
				 toggleProbes[probeNum]= EditorGUILayout.Foldout(toggleProbes[probeNum],"Probe "+probeNum.ToString());
				
				}
			num++;
		}}
		
	if(GUI.changed)
		{
			probes.coefficients = newVals;
			EditorUtility.SetDirty(target);
		}
		}
	
}
