using UnityEngine;
using System.Collections;

public class ReflectionCameraControl : MonoBehaviour {
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void OnPostRender() {
		camera.targetTexture = MirrorReflection.m_ReflectionTexture;
	}
}
