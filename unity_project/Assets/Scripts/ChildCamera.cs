using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Camera))]
public class ChildCamera : MonoBehaviour
{
	private Camera parentCam;

	void Awake()
	{
		parentCam = transform.parent.GetComponent<Camera>();
		if (parentCam == null)
		{
			Debug.LogError("Parent must have 'Camera' component.");
			enabled = false;
		}
	}

	void Update ()
	{
		camera.isOrthoGraphic = parentCam.isOrthoGraphic;
		camera.orthographicSize = parentCam.orthographicSize;
		camera.fieldOfView = parentCam.fieldOfView;
		camera.nearClipPlane = parentCam.nearClipPlane;
		camera.farClipPlane = parentCam.farClipPlane;
		camera.depth = parentCam.depth + 1;
		camera.clearFlags = CameraClearFlags.Depth;
		camera.rect = parentCam.rect;
		transform.localPosition = Vector3.zero;
		transform.localScale = Vector3.one;
		transform.localRotation = Quaternion.identity;
	}
}
