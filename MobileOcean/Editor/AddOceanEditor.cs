using UnityEngine;
using System.Collections;
using UnityEditor;


public class AddOceanEditor : MonoBehaviour {
	
	[MenuItem ("GameObject/Mobile Ocean")]
	static void CreateOcean(){
		Instantiate( Resources.Load("Ocean"));
	}
}
