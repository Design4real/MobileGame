using UnityEngine;
using System.Collections;

public class CamTargetPoint : MonoBehaviour 
{
	public Transform positionPoint;
	public Transform lookAtPoint;
	
	void Start()
	{
		positionPoint.renderer.enabled = false;
		lookAtPoint.renderer.enabled = false;
	}
}