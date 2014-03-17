//using UnityEngine;
//using UnityEditor;
//using System.Collections;
//using System.Collections.Generic;
//using System.Linq;
//
//[CustomEditor(typeof(ParkingTransport))]
//[CanEditMultipleObjects]
//public class TestEd : Editor 
//{
//	public override void OnInspectorGUI()
//	{
//		if (GUILayout.Button("test"))
//		{
//			Selection.objects = GameObject.FindObjectsOfType(typeof(MeshRenderer)).Select(a => 
//			{
//				if ((a as MeshRenderer).materials != null && (a as MeshRenderer).materials.Any(b => b.shader.name.Contains("Transparent")))
//				{
//					return (a as Component).gameObject;
//				}
//				else
//				{
//					return null;
//				}
//			}).ToArray();
//		}
//	}
//	
//}
