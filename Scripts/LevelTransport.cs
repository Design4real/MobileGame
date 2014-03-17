using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LevelTransport : Transport 
{	
	float timeElapsed;
	
	float maxTime;
	
	public string timeLeft
	{
		get
		{
			System.DateTime time = new System.DateTime();
			if( timeElapsed > maxTime )
			{
				timeElapsed = maxTime;
			}
			time = time.AddMilliseconds((maxTime - timeElapsed) * 1000.0f);
			return time.ToString("mm:ss");
		}
	}
	
	public float fuel;
	public float maxFuel;
	
	public Transform parkingTarget;
	
	float curBrightness;
	const float minBrightness = 0;
	const float maxBrightness = 60.0f / 255.0f;
	float brightnessDeltaSign = 1;

	public Camera aCamera;
	
	protected override void Awake()
	{
		base.Awake();
		timeElapsed = 0;
		//winGame.transport = this;

		foreach (Camera cam in cams.cameras)
		{
			cam.transform.parent = camsRoot;
			//cam.rect = new Rect(0.8f, 0.385f, 0.2f, 0.25f);
		}

		if (aCamera == null)
		{
			aCamera = new GameObject("Camera").AddComponent<Camera>();
		}
		aCamera.transform.parent = camsRoot;
		
		if (Application.loadedLevelName.Contains("1"))
		{
			transform.rotation = Quaternion.Euler(0, 90, 0);
		}
		else if (Application.loadedLevelName.Contains("4"))
		{
			transform.rotation = Quaternion.Euler(0, 340, 0);
		}
		if (Application.loadedLevelName == "s1")
		{
			maxTime = 7.0f * 60;
		}
		else if (Application.loadedLevelName == "s4")
		{
			maxTime = 9.0f * 60;
		}
		else if (Application.loadedLevelName == "s3")
		{
			maxTime = 5.0f * 60;
		}
	}
	
	
	// Use this for initialization
	protected override void Start () 
	{
		createCollider();
		base.Start();
		
		fuel = maxFuel = 600;
		GlobalDataKeeper.curGasLeft = fuel / maxFuel;
	}
	
	protected void createCollider()
	{
		Vector3 cargoSize = Vector3.zero;
		Vector3 cargoCenter = Vector3.zero;
		Vector3 transportSize = Vector3.zero;
		Vector3 transportCenter = Vector3.zero;
		
		if (Application.loadedLevelName.Contains("1"))
		{
			cargoSize = new Vector3(7.77f, 7.51f, 33.81f);
			cargoCenter = new Vector3(0, 6.1f, -0.1f);
			createCollider(cargoCenter, cargoSize, "collider_cargo");
			transportSize = new Vector3(7.4f, 1.3f, 38.8f);
			transportCenter = new Vector3(0, 0.97f, 2.4f);			
			createCollider(transportCenter, transportSize, "transport_cargo");
		}
		else if (Application.loadedLevelName.Contains("4"))
		{
			cargoSize = new Vector3(3.12f, 2.4f, 13.17f);
			cargoCenter = new Vector3(0, 2.51f, 0.2f);		
			createCollider(cargoCenter, cargoSize, "collider_cargo1");
			cargoSize = new Vector3(8.8f, 0.65f, 2.47f);
			cargoCenter = new Vector3(0, 1.89f, -3.77f);		
			createCollider(cargoCenter, cargoSize, "collider_cargo2");
			transportSize = new Vector3(2.61f, 0.32f, 11.23f);
			transportCenter = new Vector3(0, 0.66f, 0.24f);
			createCollider(transportCenter, transportSize, "transport_cargo");
		}
		else if (Application.loadedLevelName.Contains("3"))
		{
			cargoSize = new Vector3(3.12f, 2.37f, 8.66f);
			cargoCenter = new Vector3(0, 3.32f, 1.96f);		
			createCollider(cargoCenter, cargoSize, "collider_cargo");
			transportSize = new Vector3(2.3f, 0.44f, 11.8f);
			transportCenter = new Vector3(0, 2.5f, -0.1f);
			createCollider(transportCenter, transportSize, "transport_cargo");
		}
	}
	
	void createCollider(Vector3 center, Vector3 size, string name)
	{
		GameObject collider_go = new GameObject(name);
		collider_go.transform.parent = transform;
		collider_go.transform.localPosition = Vector3.zero;
		collider_go.transform.localRotation = Quaternion.identity;
		collider_go.layer = LayerMask.NameToLayer("WheelPair");
		BoxCollider bc = collider_go.AddComponent<BoxCollider>();
		collider_go.tag = "Cargo";
		bc.size = size;
		bc.center = center;
		bc.material.bounciness = 0;
		bc.material.bounceCombine = PhysicMaterialCombine.Minimum;
		bc.material.dynamicFriction = 0;
		bc.material.dynamicFriction2 = 0;
		//bc.material.frictionCombine = PhysicMaterialCombine.Minimum;
		bc.material.frictionCombine = PhysicMaterialCombine.Average;
		bc.material.frictionDirection = Vector3.zero;
		bc.material.frictionDirection2 = Vector3.zero;
		//bc.material.frictionCombine = PhysicMaterialCombine.Minimum;
		bc.material.frictionCombine = PhysicMaterialCombine.Average;
		bc.material.staticFriction = 0;
		bc.material.staticFriction2 = 0;
	}
	
	// Update is called once per frame
	protected override void Update () 
	{
		base.Update();
		
		camsRoot.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, transform.rotation.eulerAngles.y, 0);
		camsRoot.position = transform.position;
		
		if (target != null)
		{
			reached = true;
			foreach (Platform curPlatform in platforms)
			{
				if (!target.collider.bounds.Contains(new Vector3(curPlatform.renderer.bounds.min.x,
																 target.collider.bounds.center.y,
																 curPlatform.renderer.bounds.min.z)) ||
					!target.collider.bounds.Contains(new Vector3(curPlatform.renderer.bounds.max.x,
																 target.collider.bounds.center.y,
																 curPlatform.renderer.bounds.max.z)))
				{
					reached = false;
					break;
				}
			}
			if (reached)
			{
				if (GameController.runtime.curState == GameController.GameState.miniGame2)
				{
					GameController.runtime.curState = GameController.GameState.win;
				}
				else if (GameController.runtime.curState == GameController.GameState.mainGame)
				{
					target.gameObject.SetActive(false);
					target = parkingTarget;
					target.gameObject.SetActive(true);
					GameController.runtime.curState = GameController.GameState.miniGame2;
				}
			}
		}
		
		if (GameController.runtime != null)
		{
			if (GameController.runtime.curState != GameController.GameState.miniGame2)
			{
				if (GameController.runtime.curState == GameController.GameState.mainGame && !GameController.runtime.isPaused )
				{
					timeElapsed += Time.deltaTime;
				}
//				
//				if (!Mathf.Approximately(wheelLines[0][0].wheelCollider.motorTorque, 0) || rigidbody.velocity.magnitude >= maxSpeed)
//				{
//						fuel -= 0.1f;
//						GlobalDataKeeper.curGasLeft = fuel / maxFuel;
//				}
				
				
				if (timeElapsed >= maxTime || fuel <= 0)
				{
					GameController.runtime.curState = GameController.GameState.loose;
					rigidbody.constraints = RigidbodyConstraints.FreezeAll;
				}
			}
			if (target != null)
			{
				curBrightness += (maxBrightness - minBrightness) * Time.deltaTime * brightnessDeltaSign;
				curBrightness = Mathf.Clamp(curBrightness, minBrightness, maxBrightness);
				if (curBrightness >= maxBrightness || curBrightness <= minBrightness)
				{
					brightnessDeltaSign *= -1;
				}
				target.renderer.material.color = new Color(0, 140.0f / 255.0f, 0, curBrightness);
			}
		}
		
		if (((transform.localRotation.eulerAngles.z > 70 && transform.localRotation.eulerAngles.z < 90) || (transform.localRotation.eulerAngles.z < -70 && transform.localRotation.eulerAngles.z > -90)) ||
			(transform.localRotation.eulerAngles.z > 290 && transform.localRotation.eulerAngles.z < 310))
		{
			GameController.runtime.LivesCount = 0;
			GameController.runtime.curState = GameController.GameState.loose;
		}
	}
}
