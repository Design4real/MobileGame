using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Transport : MonoBehaviour 
{
	//public float maxSteer = 90; //Максимальный угол поворота колес
    public float maxAccel = 10; //Максимальный крутящий момент
    public float maxBrake = 35; //Максимальный тормозной момент
	
	public float maxSpeed = 5.0f;
	public float maxAngleSpeed = 0.07f;
	
	public static float steerDelta = 0.5f;
	
	protected float curSteer;
	
	public Transform centerOfMass;
	
	public List<Platform> platforms;
	
	protected Vector3 wheelsCenter;
	
	public List<SteerMode.Abstract> movementModes;
	
	public float angleCoeff;
	
	public int curModeInd;
	public SteerMode.Abstract curMode
	{
		get
		{
			if (curModeInd > movementModes.Count - 1)
			{
				curModeInd = 0;
			}
			if (curModeInd < 0)
			{
				curModeInd = movementModes.Count -1;
			}
			return movementModes[curModeInd];
		}
	}
	
	public Transform target;
	
	public bool reached;
	
	bool wasNextCameraPressed;
	
	public List<List<WheelPair>> wheelLines;
	
	[System.SerializableAttribute]
	public class Cameras
	{
		public List<Camera> cameras;
		[HideInInspector]
		public int curCameraInd;
	}
	[System.SerializableAttribute]
	public class CamerasPoints
	{
		public List<Transform> points;
		public List<float> fovs;
	}
	public Cameras cams;
	[HideInInspector]
	public CamerasPoints camsPoints;
	float prevNextMode;
	
	Dictionary<WheelPair, float> circleModeAngles;

	public Camera secondaryCam;
	private float secondaryCameraShowTime = 7.0f;
	private float secondaryCameraShowTime_counter = 0f;
	
	protected System.Type prevSteerMode;
	
	protected Transform camsRoot;
	
	protected virtual void Awake()
	{
		reached = false;
		wasNextCameraPressed = false;

		SteerMode.Circle mode_circle = new SteerMode.Circle();
		SteerMode.DiagonalAlong mode_diagAlong = new SteerMode.DiagonalAlong();
		SteerMode.DiagonalCross mode_diagCross = new SteerMode.DiagonalCross();
		SteerMode.FrontWheel mode_frontWheel = new SteerMode.FrontWheel();
		SteerMode.RearWheel mode_rearWheel = new SteerMode.RearWheel();
		SteerMode.AllWheelAlong mode_allWheelAlong = new SteerMode.AllWheelAlong();
		SteerMode.AllWheelCross mode_allWheelCross = new SteerMode.AllWheelCross();

		movementModes = new List<SteerMode.Abstract>();
		movementModes.Add(mode_frontWheel);
		movementModes.Add(mode_rearWheel);
		movementModes.Add(mode_allWheelAlong);
		movementModes.Add(mode_allWheelCross);
		movementModes.Add(mode_circle);
		movementModes.Add(mode_diagAlong);
		movementModes.Add(mode_diagCross);

		camsRoot = (new GameObject("camsRoot")).transform;
		camsRoot.parent = this.transform;
		camsRoot.localRotation = Quaternion.identity;
		camsRoot.localPosition = Vector3.zero;
		camsRoot.parent = this.transform.parent;

		if (secondaryCam != null)
		{
			float screenAspect = ((float)Screen.width) / ((float)Screen.height);
			Vector2 borderSize =  new Vector2(0.015f / screenAspect, 0.015f);
			Rect secondaryCamRect = new Rect(0, 0, 0.2275f/* * screenAspect*/, 0.37f);
			secondaryCamRect.x = 1f - secondaryCamRect.width;// - borderSize.x;
			secondaryCamRect.y = 1f - secondaryCamRect.height - borderSize.y;// - 0.12f;
			
			secondaryCam.transform.parent = camsRoot;
			secondaryCam.rect = secondaryCamRect;
			secondaryCam.depth = 10;
			secondaryCameraShowTime_counter = 0;
			secondaryCam.gameObject.SetActive(false);
			
			secondaryCamRect.x -= borderSize.x;
			secondaryCamRect.y -= borderSize.y;
			secondaryCamRect.height += borderSize.y * 2;
			secondaryCamRect.width += borderSize.x;// * 2;

			Camera secondaryCamBG = (new GameObject("secondaryCamBG")).AddComponent<Camera>();
			secondaryCamBG.transform.position = Vector3.one * 1000000;
			secondaryCamBG.transform.parent = secondaryCam.transform;
			secondaryCamBG.backgroundColor = new Color(212f / 255f, 0, 0, 1);
			secondaryCamBG.clearFlags = CameraClearFlags.SolidColor;
			secondaryCamBG.depth = 9;
			secondaryCamBG.rect = secondaryCamRect;
			secondaryCamBG.cullingMask = 0;
		}
		prevSteerMode = null;
	}
	
	protected virtual void Start () 
	{
		
		rigidbody.centerOfMass = centerOfMass.localPosition;
		platforms = transform.GetComponentsInChildren<Platform>().ToList();
		
		curSteer = 0;

		camsPoints = new CamerasPoints();
		camsPoints.fovs = new List<float>();
		camsPoints.points = new List<Transform>();
		foreach (Camera curCamera in cams.cameras)
		{
			camsPoints.points.Add(curCamera.transform);
			camsPoints.fovs.Add(curCamera.fieldOfView);
			if (curCamera != secondaryCam)
			{
				tryDestroy(curCamera, "AntialiasingAsPostEffect");
				tryDestroy(curCamera, "DepthOfField34");
				tryDestroy(curCamera, "SSAOEffect");
				tryDestroy(curCamera, "FlareLayer");
				tryDestroy(curCamera, "GUILayer");
				tryDestroy(curCamera, "Camera");
				RemoveChildren(curCamera.transform);
			}
		}

		if (this is LevelTransport)
		{
			changeCam(true);
		}
		
		/*cams.localPositions = new List<Vector3>();
		cams.startY = new List<float>();
		for (int i = 0; i < cams.cameras.Count; i++)
		{
			cams.localPositions.Add(cams.cameras[i].transform.localPosition);
			cams.startY.Add(cams.cameras[i].transform.position.y);
		}*/
		
		
		initWheelLines();
		
		Quaternion cachedRotation = transform.rotation;
		transform.rotation = Quaternion.identity;
		movementModes.ForEach( a => a.init(wheelLines) );
		transform.rotation = cachedRotation;
		
		curModeInd = 0;
		changeMode(curMode);
		
		if (Application.loadedLevelName == "s1")
		{
			maxAccel = 9.0f;
			maxSpeed = 3.5f;
		}
		else if (Application.loadedLevelName == "s4")
		{
			maxAccel = 11.5f;
			maxSpeed = 3.5f;
		}
		else if (Application.loadedLevelName == "s3")
		{
			maxAccel = 13.5f;
			maxSpeed = 2.0f;
		}
		
		secondaryCameraShowTime_counter = 0;
	}
	
	void initWheelLines()
	{
		wheelLines = new List<List<WheelPair>>();
		
		List<Platform> tmp = new List<Platform>(platforms);
		tmp = tmp.OrderByDescending(a => a.transform.localPosition.z).ToList();
		for (int i = 0; i < tmp.Count; i += 2)
		{
			for (int j = 0; j < tmp[i].wheelLines.Count; j++)
			{
				wheelLines.Add(new List<WheelPair>());
				wheelLines[wheelLines.Count - 1].AddRange(tmp[i].wheelLines[j]);
				if (tmp.Count > i + 1)
				{
					wheelLines[wheelLines.Count - 1].AddRange(tmp[i + 1].wheelLines[j]);
				}
			}
		}
	}

	//#if UNITY_EDITOR
	void OnGUI()
	{
		
		int cachedSize = GUI.skin.label.fontSize;
		Color cachedColor = GUI.color;
		Color cachedContentColor = GUI.contentColor;
		FontStyle cachedFontStyle = GUI.skin.label.fontStyle;
		TextAnchor cachedAlign = GUI.skin.label.alignment;
		
		GUI.color = Color.white;
		GUI.contentColor = Color.white;
		GUI.skin.label.fontSize = 17;
		GUI.skin.label.fontStyle = FontStyle.Bold;
		GUI.skin.label.alignment = TextAnchor.LowerLeft;
		
		if (secondaryCam != null && secondaryCam.gameObject.activeSelf)
		{
			Rect lblRect = new Rect(secondaryCam.rect.x * Screen.width + 10, (1f - secondaryCam.rect.y) * Screen.height - 45, 0, 45);
			lblRect.width = Screen.width - lblRect.x - 5;
			GUI.Label(lblRect, curMode.name);
		}
		
		GUI.skin.label.fontSize = cachedSize;
		GUI.color = cachedColor;
		GUI.contentColor = cachedContentColor;
		GUI.skin.label.fontStyle = cachedFontStyle;
		GUI.skin.label.alignment = cachedAlign;
		
		//drawAccelGUI();
		return;
		if (Camera.current == null)
		{
			return;
		}

		for (int i = 0; i < wheelLines.Count; i++)
		{
			for (int j = 0; j < wheelLines[i].Count; j++)
			{
				string content = string.Format("{0}, {1}", i, j);
				//content = curMode.deltas[i][j].ToString();
				//content = wheelLines[i][j].motorTorque.ToString();
				content = wheelLines[i][j].desiredSteer.ToString("F0");
				//content = i.ToString() + ", " + j.ToString();
				Vector2 size = GUI.skin.label.CalcSize(new GUIContent(content));
				Vector2 begin = ((Vector2)Camera.current.WorldToScreenPoint(wheelLines[i][j].transform.position)) - size / 2;
				if ((i % 2 == 0))
				{
					begin.y -= size.y * 0.5f;
				}
				else
				{
					begin.y += size.y * 0.5f;
				}
				Vector2 end = begin + size;
				if (wheelLines[i][j].isMotorTorqueReversed)
				{
					GUI.color = Color.red;
				}
				else
				{
					GUI.color = Color.green;
				}
				GUI.Label(new Rect(begin.x, begin.y, end.x, end.y), content);

				//UnityEditor.Handles.Label(wheelLines[i][j].transform.position, content);
			}
		}

		GUILayout.Label(curMode.name + " (" + curMode.maxAngle + ")", GUILayout.ExpandWidth(true), GUILayout.ExpandHeight(false));
	}
	//#endif
	
	void drawAccelGUI()
	{
		Screen.showCursor = true;
		int cachedSize = GUI.skin.label.fontSize;
		Color cachedColor = GUI.color;
		Color cachedContentColor = GUI.contentColor;
		
		GUI.color = Color.white;
		GUI.contentColor = Color.white;
		GUI.skin.label.fontSize = 15;
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;
		
		//GUILayout.BeginArea(new Rect(0, Screen.height / 2, 1000, 1000));
		GUILayout.BeginVertical();
		
		GUILayout.Space(200);
		
		GUILayout.Label("---------------------------");
		GUILayout.Space(1);
		GUILayout.BeginHorizontal();
		GUILayout.Label("max accel:", GUILayout.Height(50), GUILayout.Width(80));
		if (GUILayout.Button("-", GUILayout.Height(50), GUILayout.Width(30)))
		{
			maxAccel -= 0.1f;
		}
		GUILayout.Label(maxAccel.ToString("F1"), GUILayout.Height(50), GUILayout.Width(50));
		if (GUILayout.Button("+", GUILayout.Height(50), GUILayout.Width(30)))
		{
			maxAccel += 0.1f;
		}
		GUILayout.EndHorizontal();
		
		GUILayout.BeginHorizontal();
		GUILayout.Label("max speed:", GUILayout.Height(50), GUILayout.Width(80));
		if (GUILayout.Button("-", GUILayout.Height(50), GUILayout.Width(30)))
		{
			maxSpeed -= 0.1f;
		}
		GUILayout.Label(maxSpeed.ToString("F1"), GUILayout.Height(50), GUILayout.Width(50));
		if (GUILayout.Button("+", GUILayout.Height(50), GUILayout.Width(30)))
		{
			maxSpeed += 0.1f;
		}
		GUILayout.EndHorizontal();
		GUILayout.Space(1);
		GUILayout.Label("---------------------------");
		
		GUILayout.EndVertical();
		//GUILayout.EndArea();

		GUI.skin.label.fontSize = cachedSize;
		GUI.color = cachedColor;
		GUI.contentColor = cachedContentColor;
	}
	
	protected virtual void Update () 
	{
		int newModeInd = -1;
		if (Common.input.GetButtonDown(SSSInput.InputType.Steering_FrontWheel))
		{
			newModeInd = movementModes.FindIndex( a => a is SteerMode.FrontWheel );
		}
		if (Common.input.GetButtonDown(SSSInput.InputType.Steering_RearWheel))
		{
			newModeInd = movementModes.FindIndex( a => a is SteerMode.RearWheel );
		}
		if (Common.input.GetButtonDown(SSSInput.InputType.Steering_AllWheelAlong))
		{
			newModeInd = movementModes.FindIndex( a => a is SteerMode.AllWheelAlong );
			//curSteer = curMode.maxAngle;//moved to changeMode()
		}
		if (Common.input.GetButtonDown(SSSInput.InputType.Steering_AllWheelCross))
		{
			newModeInd = movementModes.FindIndex( a => a is SteerMode.AllWheelCross );
		}
		if (Common.input.GetButtonDown(SSSInput.InputType.Steering_Circle))
		{
			newModeInd = movementModes.FindIndex( a => a is SteerMode.Circle );
		}
		if (Common.input.GetButtonDown(SSSInput.InputType.Steering_DiagonalAlong))
		{
			newModeInd = movementModes.FindIndex( a => a is SteerMode.DiagonalAlong );
		}
		if (Common.input.GetButtonDown(SSSInput.InputType.Steering_DiagonalCross))
		{
			newModeInd = movementModes.FindIndex( a => a is SteerMode.DiagonalCross );
		}
		if (newModeInd != -1 && curModeInd != newModeInd)
		{
			curModeInd = newModeInd;
			changeMode(curMode);
		}
		
		if (MenuController.isInMenu || GameController.runtime.curState == GameController.GameState.loose || GameController.runtime.curState == GameController.GameState.win)
		{
			secondaryCam.gameObject.SetActive(false);
		}
		else
		{
			if (secondaryCam != null && secondaryCam.gameObject.activeSelf)
			{
				secondaryCameraShowTime_counter -= Time.deltaTime;
			}
			if (secondaryCameraShowTime_counter > 0 && secondaryCam != null && !secondaryCam.gameObject.activeSelf)
			{
				secondaryCam.gameObject.SetActive(true);
			}
			if (secondaryCameraShowTime_counter <= 0 && secondaryCam != null && secondaryCam.gameObject.activeSelf)
			{
				secondaryCam.gameObject.SetActive(false);
			}
		}
	}
	
	public void changeCam(bool instant = true)
	{
		if (cams.curCameraInd == -1)
		{
			cams.curCameraInd = cams.cameras.Count - 1;
		}
		else if (cams.curCameraInd == cams.cameras.Count)
		{
			cams.curCameraInd = 0;
		}
		
		if (cams.curCameraInd > cams.cameras.Count - 1)
		{
			return;
		}
		
		if (this is LevelTransport)
		{
			Camera aCamera = (this as LevelTransport).aCamera;
			if (instant)
			{
				aCamera.transform.localPosition = camsPoints.points[cams.curCameraInd].transform.localPosition;
				aCamera.transform.localRotation = camsPoints.points[cams.curCameraInd].transform.localRotation;
				aCamera.fieldOfView = camsPoints.fovs[cams.curCameraInd];
				changeCamPos_passedTime = changeCamPos_time;
			}
			else
			{
				changeCamPos_fromPos = aCamera.transform.localPosition;
				changeCamPos_fromRot = aCamera.transform.localRotation.eulerAngles;
				changeCamPos_fromFOV = aCamera.fieldOfView;
				changeCamPos_toPos = camsPoints.points[cams.curCameraInd].transform.localPosition;
				changeCamPos_toRot = camsPoints.points[cams.curCameraInd].transform.localRotation.eulerAngles;
				changeCamPos_toFOV = camsPoints.fovs[cams.curCameraInd];
				changeCamPos_passedTime = 0;

				Vector3 deltaRot = changeCamPos_toRot - changeCamPos_fromRot;

				float middleAngle = Vector3.Angle((changeCamPos_fromPos + changeCamPos_toPos) / 2f, transform.up);
				middleAngle += (changeCamPos_fromRot.y + changeCamPos_toRot.y) / 2f;
				if (Mathf.Abs(deltaRot.y) > 180)
				{
					changeCamPos_toRot.y -= 360f * Mathf.Sign(deltaRot.y);
				}
				if (Mathf.Abs(middleAngle) > 270)
				{
					changeCamPos_toRot.y -= 360f * Mathf.Sign(changeCamPos_toRot.y);
				}
				if (Mathf.Abs(deltaRot.x) > 180)
				{
					changeCamPos_toRot.x -= 360f * Mathf.Sign(deltaRot.x);
				}
				if (Mathf.Abs(deltaRot.z) > 180)
				{
					changeCamPos_toRot.z -= 360f * Mathf.Sign(deltaRot.z);
				}

				if (!changeCamPos_isInProgress)
				{
					StartCoroutine(changeCamPos());
				}
			}
		}
	}

	bool changeCamPos_isInProgress = false;
	Vector3 changeCamPos_fromPos;
	Vector3 changeCamPos_toPos;
	Vector3 changeCamPos_fromRot;
	Vector3 changeCamPos_toRot;
	float changeCamPos_fromFOV;
	float changeCamPos_toFOV;
	float changeCamPos_time = 0.5f;
	float changeCamPos_passedTime;

	IEnumerator changeCamPos()
	{
		changeCamPos_isInProgress = true;
		try
		{
			while (true)
			{
				changeCamPos_passedTime += Time.deltaTime;
				float progress = changeCamPos_passedTime / changeCamPos_time;
				progress = Mathf.Clamp(progress, 0, 1);

				Camera aCamera = (this as LevelTransport).aCamera;
				aCamera.transform.localPosition = Vector3.Slerp(changeCamPos_fromPos, changeCamPos_toPos, progress);
				aCamera.transform.localRotation = Quaternion.Euler(Vector3.Slerp(changeCamPos_fromRot, changeCamPos_toRot, progress));
				aCamera.fieldOfView = Mathf.Lerp(changeCamPos_fromFOV, changeCamPos_toFOV, progress);
				yield return true;

				if (progress >= 1)
				{
					break;
				}
			}
		}
		finally
		{
			changeCamPos_isInProgress = false;
		}
	}
	
	public void changeMode(SteerMode.Abstract mode)
	{
		curSteer = 0;
		if (curMode is SteerMode.AllWheelAlong)
		{
			curSteer = curMode.maxAngle;
		}
		if( winGame.runtime != null )
		{
			winGame.runtime.ModeButtonsChange();
		}
		
		bool isMustShowSecondaryCam = true;
		List<System.Type> tmpTypesList = new List<System.Type>();
		tmpTypesList.Add(prevSteerMode);
		tmpTypesList.Add(curMode.GetType());
		if ((tmpTypesList.Contains(typeof(SteerMode.FrontWheel)) && tmpTypesList.Contains(typeof(SteerMode.RearWheel))) ||
			prevSteerMode == null)
		{
			isMustShowSecondaryCam = false;
		}
		prevSteerMode = mode.GetType();

		//bool isMustShowSecondaryCam = (steerModesWithSecondaryCam & mode.getSteerModeType()) == mode.getSteerModeType();
		if (secondaryCam != null && isMustShowSecondaryCam)
		{
			secondaryCameraShowTime_counter = secondaryCameraShowTime;
		}
	}
	
	protected void printValue(string text, ref float val, bool allowChange = true, float delta = 0.05f)
	{
		GUILayout.BeginHorizontal();
		
		GUILayout.Label(text);
		if (allowChange)
		{
			if (GUILayout.RepeatButton("-"))
			{
				val -= delta;
			}
		}
		
		GUILayout.Label(val.ToString("F4"));
		
		if (allowChange)
		{
			if (GUILayout.RepeatButton("+"))
			{
				val += delta;
			}
		}
		
		GUILayout.EndHorizontal();
	}
	
	protected void printValue(string text, float val)
	{
		printValue(text, ref val, false);
	}
	
	void FixedUpdate()
	{
		float nextMode = Common.input.GetAxisRaw(SSSInput.InputType.NextMode);
		if ( nextMode != 0 && nextMode != prevNextMode )
		{
			//curModeInd = 0;
			curModeInd++;
			changeMode( curMode );
		}
		prevNextMode = nextMode;
		
		if (Common.input.GetAxisRaw(SSSInput.InputType.NextCamera) != 0 && !wasNextCameraPressed)
		{
			cams.curCameraInd++;
			changeCam(false);
			wasNextCameraPressed = true;
		}
		if (Common.input.GetAxisRaw(SSSInput.InputType.NextCamera) == 0 && wasNextCameraPressed )
		{
			wasNextCameraPressed = false;
		}
		
		float accel = 0;
		float steer = 0;
		
		accel = Common.input.GetAxis(SSSInput.InputType.Vertical);
		steer = Common.input.GetAxisRaw(SSSInput.InputType.Horizontal);
		
		accel = accel * 1.5f;
		if (accel > 1)
		{
			accel = 1;
		}
		
		/*if (steer == 0)
		{
			curSteer -= steerDelta * getSign(curSteer);
		}*/
			
		curSteer += steerDelta * getSign(steer);
		curSteer = curMode.clampSteerAngle(curSteer);
		//!curSteer = Mathf.Clamp(curSteer, -curMode.maxAngle, curMode.maxAngle);
		
		for (int i = 0; i < wheelLines.Count; i++)
		{
			for (int j = 0; j < wheelLines[i].Count; j++)
			{
				//wheelLines[i][j].wheelCollider.steerAngle = curMode.getAngle(i, j, curSteer);
				wheelLines[i][j].desiredSteer = curMode.getAngle(i, j, curSteer);
			}
		}
		
		if (accel == 0 || !curMode.isCanMove(curSteer))
		{
			foreach (Platform curPlatform in platforms)
			{
				foreach (WheelPair wheel in curPlatform.wheelPairs)
				{
					wheel.brakeTorque = maxBrake;// * (curPlatform.isReversed ? -1f : 1f);
					wheel.motorTorque = 0;
				}
			}
		}
		else
		{
			if ((rigidbody.velocity.magnitude >= maxSpeed && Mathf.Sign(platforms[0].wheelPairs[0].motorTorque) == Mathf.Sign(accel)))
			{
				accel = 0;
			}
			foreach (Platform curPlatform in platforms)
			{
				foreach (WheelPair wheel in curPlatform.wheelPairs)
				{
					wheel.brakeTorque = 0;
					wheel.motorTorque = accel * maxAccel * (curPlatform.isReversed ? -1f : 1f);
				}
			}
		}
	}
	
	float getWheelPairLine(WheelPair wheelPair)
	{
		return wheelLines.FindIndex(a => a.Contains(wheelPair));
	}
	
	public static float getSign(float val)
	{
		if (val > 0)
		{
			return 1;
		}
		else if (val < 0)
		{
			return -1;
		}
		return 0;
	}

	public static void tryDestroy(Component obj, string nameToDestroy)
	{
		Component componentToDestroy = null;
		if (obj == null || (componentToDestroy = obj.GetComponent(nameToDestroy)) == null)
		{
			return;
		}
		Destroy(componentToDestroy);
	}

	public static void RemoveChildren( Transform transform )
	{
		int count = transform.childCount;
		for( int i = count - 1; i >= 0; i-- )
		{
			GameObject.DestroyImmediate( transform.GetChild( i ).gameObject );
		}
	}
}
