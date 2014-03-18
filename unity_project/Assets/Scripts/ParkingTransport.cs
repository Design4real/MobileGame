using UnityEngine;
using System.Collections;




public class ParkingTransport : Transport 
{

	int numberPlatforms =4;

	float curBrightness;
	const float minBrightness = 100.0f / 255.0f;
	float brightnessDeltaSign = 1;
	
	protected override void Awake()
	{
		base.Awake();
		curBrightness = minBrightness;
		/*foreach (Camera cam in cams.cameras)
		{
			cam.rect = new Rect(0.8f, 0.385f, 0.2f, 0.25f);
		}*/
	}
	
	protected override void Start () 
	{

		base.Start();
		if (Application.loadedLevelName.Contains("1"))
		{
			transform.rotation = Quaternion.Euler(0, 90, 0);
		}
		//if (Application.loadedLevelName.Contains("4"))
		//{
		//	transform.rotation = Quaternion.Euler(0, 340, 0);
		//}
		rigidbody.constraints = RigidbodyConstraints.None;
		enabled = false;
		curModeInd = movementModes.FindIndex( a => a is SteerMode.FrontWheel );
		prevSteerMode = null;
		changeMode(curMode);
	}
	
	protected override void Update () 
	{
		base.Update();
		
		bool tmp_reached;
		if (!Application.loadedLevelName.Contains("1"))
		{
			tmp_reached = 	target.renderer.bounds.Contains(new Vector3(platforms[0].renderer.bounds.min.x,
																		target.renderer.bounds.center.y,
																		platforms[0].renderer.bounds.min.z)) && 
							target.renderer.bounds.Contains(new Vector3(platforms[0].renderer.bounds.max.x,
																		target.renderer.bounds.center.y,
																		platforms[0].renderer.bounds.max.z));
			
		}
		else
		{
			tmp_reached = 	target.GetChild(0).renderer.bounds.Contains(new Vector3(platforms[0].renderer.bounds.min.x,
																					target.GetChild(0).renderer.bounds.center.y,
																					platforms[0].renderer.bounds.min.z)) && 
							target.GetChild(0).renderer.bounds.Contains(new Vector3(platforms[0].renderer.bounds.max.x,
																					target.GetChild(0).renderer.bounds.center.y,
																					platforms[0].renderer.bounds.max.z));
		}
			
		
		if (tmp_reached)
		{
			setTargetColor(Color.green, 100);

			reached = true;

		}
		else
		{
			setTargetColor(Color.yellow, 40);
			reached = false;
		}

		//blinker;
		curBrightness += (1 - minBrightness) * Time.deltaTime * brightnessDeltaSign;
		curBrightness = Mathf.Clamp(curBrightness, minBrightness, 1);
		if (curBrightness >= 1 || curBrightness <= minBrightness)
		{
			brightnessDeltaSign *= -1;
		}
		platforms[0].renderer.material.color = new Color(curBrightness, curBrightness, curBrightness, 1);
	}
	
	void setTargetColor(Color color, float alpha)
	{
		color.a = alpha / 255.0f;
		target.renderer.material.color = color;
	}
	
	void OnEnable()
	{
		setTargetColor(reached ? Color.green : Color.yellow, 40);
		rigidbody.drag = 0;
		rigidbody.constraints = RigidbodyConstraints.None;
		curBrightness = 1;
		if (platforms.Count > 0)
		{

			platforms[0].renderer.material.shader = Shader.Find("Self-Illumin/Bumped Specular");
		}
		/*foreach (Camera curCam in cams.cameras)
		{
			curCam.gameObject.SetActive(true);
		}*/
		//platforms[0].renderer.material.color = new Color(curBrightness, curBrightness, curBrightness, 1);
		//target.renderer.enabled = true;
	}
	
	void OnDisable()
	{
		curBrightness = minBrightness;
		platforms[0].renderer.material.color = new Color(curBrightness, curBrightness, curBrightness, 1);
		//target.renderer.enabled = false;
		setTargetColor(reached ? Color.green : Color.yellow, 10);
		rigidbody.constraints = RigidbodyConstraints.FreezeAll;
		platforms[0].renderer.material.shader = Shader.Find("Bumped Specular");
		
		/*foreach (Camera curCam in cams.cameras)
		{
			curCam.gameObject.SetActive(false);
		}*/
	}
}
