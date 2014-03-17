using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WheelPair : MonoBehaviour
{
	[HideInInspector]
	public Transform wheelVisual;
	[HideInInspector]
	public Vector3 startPos;
	[HideInInspector]
	public float rotation;
	
	[HideInInspector]
	private WheelCollider wheelCollider;

	public float motorTorque
	{
		get
		{
			return wheelCollider.motorTorque;
		}
		set
		{
			wheelCollider.motorTorque = value;
			if (_isMotorTorqueReversed)
			{
				wheelCollider.motorTorque *= -1f;
			}
		}
	}
	
	public float brakeTorque
	{
		get
		{
			return wheelCollider.brakeTorque;
		}
		set
		{
			wheelCollider.brakeTorque = value;
		}
	}

	private bool _isMotorTorqueReversed;
	public bool isMotorTorqueReversed
	{
		get
		{
			return _isMotorTorqueReversed;
		}
		set
		{
			_isMotorTorqueReversed = value;
		}
	}
	
	[HideInInspector]
	public Platform platform;
	
	public float desiredSteer;
	
	private float wheelVisualRotationCoeff;
	
	private Vector3 wheelVisualPos;
	
	float initAngle_y;
	float initAngle_z;
	float angleCoeff;
	
	void Awake()
	{
		if (Mathf.Approximately(transform.localScale.x, -1))
		{
			initAngle_y = 180;
			initAngle_z = 180;
			angleCoeff = -1;
		}
		else
		{
			initAngle_y = 0;
			initAngle_z = 0;
			angleCoeff = 1;
		}
		
		wheelVisual = transform.GetChild(0).GetChild(0);
		startPos = wheelVisual.localPosition;
		rotation = 0;
		
		GameObject wheelCollider_go = new GameObject("WC_" + this.name);
		wheelCollider_go.tag = "Cargo";
		wheelCollider_go.layer = LayerMask.NameToLayer("WheelPair");
		wheelCollider_go.transform.parent = transform.parent;
		wheelCollider_go.transform.position = wheelVisual.position + new Vector3(-Mathf.Sign(wheelVisual.localPosition.x) * 0.45f, 0, 0) * transform.lossyScale.x * (transform.parent.localEulerAngles.y < 20 ? -1 : 1);
		wheelCollider = wheelCollider_go.AddComponent<WheelCollider>();
		wheelCollider.radius = 0.381f * Mathf.Abs(transform.lossyScale.x);
		//wheelCollider.suspensionDistance = 0.4f * Mathf.Abs(transform.lossyScale.x);
		//wheelCollider.suspensionDistance = 0.2f * Mathf.Abs(transform.lossyScale.x);
		JointSpring suspensionSpring = new JointSpring();
		if (Application.loadedLevelName.Contains("3"))
		{
			wheelCollider.suspensionDistance = 0.2f;			
			suspensionSpring.spring = 0;
			suspensionSpring.damper = 900;
		}
		else
		{
			wheelCollider.suspensionDistance = 0.1f * Mathf.Abs(transform.lossyScale.x);
			suspensionSpring.spring = 9000000;
			suspensionSpring.damper = 1000000;
		}
		wheelCollider.mass = 2;
		wheelCollider.suspensionSpring = suspensionSpring;
		WheelFrictionCurve curve = new WheelFrictionCurve();
		curve.asymptoteSlip = wheelCollider.sidewaysFriction.asymptoteSlip;
		curve.asymptoteValue = wheelCollider.sidewaysFriction.asymptoteValue;
		curve.extremumSlip = wheelCollider.sidewaysFriction.extremumSlip;
		curve.extremumValue = wheelCollider.sidewaysFriction.extremumValue;
		curve.stiffness = 0.2f * Mathf.Abs(transform.lossyScale.x);
		wheelCollider.sidewaysFriction = curve;
	}
	
	void Start()
	{
		wheelVisualRotationCoeff = Mathf.Approximately(platform.transform.localRotation.eulerAngles.y, 180) ? -1 : 1;
		wheelVisualPos = new Vector3();

		wheelCollider.transform.localRotation = Quaternion.Euler(90, 0, 0);
	}
	
	void FixedUpdate()
	{
		if (Mathf.Abs(wheelCollider.steerAngle - desiredSteer) > Transport.steerDelta)
		{
			float curSteerDelta = Mathf.Sign(desiredSteer - wheelCollider.steerAngle) * Transport.steerDelta;
			curSteerDelta = Mathf.Clamp(curSteerDelta, -Transport.steerDelta, Transport.steerDelta);
			wheelCollider.steerAngle += curSteerDelta;
		}
		updateVisual();
	}
	
	void updateVisual()
	{
		Profiler.BeginSample("2");
		float delta = Time.fixedDeltaTime;
		
		WheelHit hit;
		Vector3 curWheelVisualPos = wheelVisual.position;
		if (wheelCollider.GetGroundHit(out hit))
		{
			wheelVisualPos.x = curWheelVisualPos.x;
			wheelVisualPos.y = hit.point.y + wheelCollider.radius;
			wheelVisualPos.z = curWheelVisualPos.z;
			//wheelVisual.position = new Vector3(wheelVisual.position.x, hit.point.y + wheelCollider.radius/* + deltaY*/, wheelVisual.position.z);
			Profiler.EndSample();
		}
		else
		{
			wheelVisualPos.x = curWheelVisualPos.x;
			wheelVisualPos.y = wheelCollider.transform.position.y - wheelCollider.suspensionDistance;
			wheelVisualPos.z = curWheelVisualPos.z;
			//wheelVisual.position = new Vector3(wheelVisual.position.x, wheelCollider.transform.position.y - wheelCollider.suspensionDistance, wheelVisual.position.z);
			Profiler.EndSample();
		}
		wheelVisual.position = wheelVisualPos;
		rotation = Mathf.Repeat(rotation + delta * wheelCollider.rpm * 360.0f / 60.0f, 360.0f);
		wheelVisual.localRotation = Quaternion.Euler(rotation * wheelVisualRotationCoeff, 0, 0);
		
		transform.localRotation = Quaternion.Euler(0, initAngle_y , initAngle_z + wheelCollider.steerAngle * angleCoeff);
	}
	
}
